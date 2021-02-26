using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocsRepoCloudIntegration
{
    public class OneDriveStorageDriver : StorageBase, IStorageDriver
    {
        private readonly ILogger<OneDriveStorageDriver> _logger;
        private readonly IOptionsMonitor<StorageOptions> _options;
        private readonly IConfidentialClientApplication _clientApplication;
        private readonly ClientCredentialProvider _authProvider;
        private readonly GraphServiceClient _graphServiceClient;

        public OneDriveStorageDriver(ILogger<OneDriveStorageDriver> logger, IOptionsMonitor<StorageOptions> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _clientApplication = ConfidentialClientApplicationBuilder.Create(_options.CurrentValue.ClientId)
                .WithTenantId(_options.CurrentValue.Tenant)
                .WithClientSecret(_options.CurrentValue.ClientSecret)
                .Build();

            _authProvider = new ClientCredentialProvider(_clientApplication);
            _graphServiceClient = new GraphServiceClient(_authProvider);
        }

        private async Task<(IDriveItemRequestBuilder client, string driveId)> BuildDriveClient()
        {
            var driveId = await GetWorkingDriveId();
            return (_graphServiceClient.Drives[driveId].Root, driveId);
        }

        private async ValueTask<string> GetWorkingDriveId()
        {
            var result = await _graphServiceClient
                .Users[_options.CurrentValue.CloudDriveUserId]
                .Drive
                .Request()
                .GetAsync();

            return result.Id;
        }

        // TODO: maybe here I can pass a base folder that's already known 
        //       (for any reason) in order to reduce the excecution time
        public async Task CreateFolderIfNotExists(string folderPath, string folderAsRoot = "")
        {
            var clientBase = await BuildDriveClient();
            var driveId = clientBase.driveId;

            var currentFolder = string.IsNullOrWhiteSpace(folderAsRoot) ? clientBase.client : clientBase.client.ItemWithPath(folderAsRoot);
            folderPath = $"{SystemBaseFolder}/{folderPath}";
            var foldersToCreate = folderPath.Split("/", StringSplitOptions.RemoveEmptyEntries);
            string path = "";

            var existentItem = await currentFolder.ItemWithPath(folderPath).Request().GetAsync();

            if (existentItem != null) return;

            foreach (var folderName in foldersToCreate)
            {
                path = string.Format("{0}{1}{2}", path, string.IsNullOrEmpty(path) ? "" : "/", folderName);
                IDriveItemRequestBuilder builder = clientBase.client.ItemWithPath(path);
                DriveItem existent;
                try
                {
                    existent = await builder.Request().GetAsync();
                }
                catch (Exception)
                {
                    existent = null;
                }

                if (existent is null)
                {
                    try
                    {
                        var folderCreator = await currentFolder.Children.Request().AddAsync(new DriveItem()
                        {
                            Name = folderName,
                            Folder = new Folder(),
                            AdditionalData = new Dictionary<string, object>()
                                                {
                                                    {"@microsoft.graph.conflictBehavior", "rename"}
                                                }
                        });
                        builder = _graphServiceClient.Drives[driveId].Root
                                                        .ItemWithPath(path);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error creando carpetas");
                        throw;
                    }
                }
                currentFolder = builder;
            }

        }

        public async Task<IEnumerable<string>> ListAsync(string path = "")
        {
            path = $"{this.SystemBaseFolder}/{path}";
            var result = await _graphServiceClient.Users[_options.CurrentValue.CloudDriveUserId].Drive.Root.ItemWithPath(path).Children.Request().GetAsync();
            return result.Select(di => $"{di.Name} - {di.FileSystemInfo.CreatedDateTime}");
        }

        public async Task CopyFile(string source, string target, bool ovewrite = true)
        {
            try
            {
                var fileName = Path.GetFileName(source);
                var baseClient = await BuildDriveClient();
                var findedTarget = await baseClient.client.ItemWithPath(string.Join("/", SystemBaseFolder, target)).Request().GetAsync();
                var findedSource = await baseClient.client.ItemWithPath(string.Join("/", SystemBaseFolder, source)).Copy(fileName, new ItemReference { DriveId = baseClient.driveId, Id = findedTarget.Id }).Request().PostAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al copiar archivo");
                throw;
            }
        }

        public async Task DeleteFile(string filePath)
        {
            try
            {
                var baseClient = await BuildDriveClient();
                await baseClient.client.ItemWithPath(string.Join("/", SystemBaseFolder, filePath)).Request().DeleteAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al copiar archivo");
                throw;
            }
        }

        public async Task DeleteFolder(string path, bool recursive = true)
        {
            await DeleteFile(path);
        }

        public async Task<string[]> GetFilesInFolder(string folder)
        {
            var result = await ListAsync(folder);
            if (result.Any())
            {
                return result.ToArray();
            }
            return Array.Empty<string>();
        }

        public async Task<MemoryStream> GetMemoryStreamFromFile(string fullPath)
        {
            try
            {
                var baseClient = await BuildDriveClient();
                var file = await baseClient.client.ItemWithPath(string.Join("/", SystemBaseFolder, fullPath)).Content.Request().GetAsync();
                return (MemoryStream)file;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al copiar archivo");
                throw;
            }
        }

        public async Task<string> Save(byte[] fileContent, string path, string fileName, bool useUniqueString)
        {
            return await SaveAsync(new MemoryStream(fileContent), path, fileName, useUniqueString);
        }

        public string Save(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            //not that elegant but I'll avoid it for sure, so to use only the async one
            return SaveAsync(fileStream, path, fileName, useUniqueString).GetAwaiter().GetResult();
        }

        public async Task<string> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            try
            {
                string filePath = GenerateFilePath(path, fileName, useUniqueString);

                await CreateFolderIfNotExists(path);

                string targetPath = string.Join("/", SystemBaseFolder, filePath);

                long fileSize = (fileStream.Length * 1024) * 1024; // in MB

                if (fileSize > SmallFileSize)
                {
                    await UploadResumableFile(fileStream,targetPath);
                }
                else
                {
                    await UploadFile(fileStream, targetPath);
                }

                return targetPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al guardar el archivo");
                throw;
            }
        }

        public async Task<string> SaveOnTempFolder(byte[] fileContent, string fileName, bool useUniqueString)
        {
            return await Save(fileContent, TempFolder, fileName, useUniqueString);
        }

        public async Task Move(string sourceFileName, string pathDest)
        {
            try
            {
                var fileName = Path.GetFileName(sourceFileName);
                var baseClient = await BuildDriveClient();
                var findedTarget = await baseClient.client.ItemWithPath(string.Join("/", SystemBaseFolder, pathDest)).Request().GetAsync();
                await baseClient.client.ItemWithPath(string.Join("/", SystemBaseFolder, sourceFileName)).Request().UpdateAsync(
                    new DriveItem
                    {
                        Name = fileName,
                        ParentReference = new ItemReference { DriveId = baseClient.driveId, Id = findedTarget.Id }
                    });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No se ha podido mover el archivo");
                throw;
            }
        }

        private async Task UploadFile(Stream fileStream, string targetPath)
        {
            var baseClient = await BuildDriveClient();

            await baseClient.client.ItemWithPath(targetPath)
                            .Content.Request().PutAsync<DriveItem>(fileStream);
        }

        private async Task UploadResumableFile(Stream fileStream, string targetPath)
        {
            var baseClient = await BuildDriveClient();
        }
    }
}
