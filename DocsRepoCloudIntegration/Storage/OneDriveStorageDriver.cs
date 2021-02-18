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
    class OneDriveStorageDriver : StorageBase, IStorageDriver
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

        private async ValueTask<string> GetWorkingDriveId()
        {
            var result = await _graphServiceClient.Users[$"{{{ _options.CurrentValue.CloudDriveUserId }}}"].Drive.Request().GetAsync();
            return result.Id;
        }

        public async Task CreatFolderIfNotExists(string folderPath)
        {
            var foldersToCreate = GetWorkingDriveId();
            DriveItem driveItem = new DriveItem() { };
            var result = await _graphServiceClient.Users["{5bf33692-4f7d-4139-b592-c058661d91f6}"].Drive.Items.Request().AddAsync();
        }

        public async Task<IEnumerable<string>> ListAsync(string path = "")
        {
            var result = await _graphServiceClient.Users["{5bf33692-4f7d-4139-b592-c058661d91f6}"].Drive.Root.ItemWithPath($"{path}/Templates/DocumentosNuevaOferta").Children.Request().GetAsync();
            return result.Select(di => $"{di.Name} - {di.FileSystemInfo.LastModifiedDateTime}" );
        }

        public Task CopyFile(string source, string target, bool ovewrite = true)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFolder(string path, bool recursive = true)
        {
            throw new NotImplementedException();
        }

        public string GenerateFilePath(string path, string fileName, bool useUniqueString = false)
        {
            throw new NotImplementedException();
        }

        public string[] GetFilesInFolder(string folder)
        {
            throw new NotImplementedException();
        }

        public Task<MemoryStream> GetMemoryStreamFromFile(string fullPath)
        {
            throw new NotImplementedException();
        }

        public string Save(byte[] fileContent, string path, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }

        public string Save(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }

        public string Save(string path, string fileName, bool useUniqueString, out string savedFileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }

        public string SaveOnTempFolder(byte[] fileContent, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }

        public Task Move(string sourceFileName, string pathDest)
        {
            throw new NotImplementedException();
        }
    }
}
