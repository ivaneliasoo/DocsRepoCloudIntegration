using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DocsRepoCloudIntegration
{
    class FileSystemDriver : StorageBase, IStorageDriver
    {
        private readonly ILogger<OneDriveStorageDriver> _logger;

        public FileSystemDriver(ILogger<OneDriveStorageDriver> logger, IOptionsMonitor<StorageOptions> options)
            : base(options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public FileSystemDriver(StorageOptions options)
            : base(options)
        {

        }

        public FileSystemDriver()
        {

        }

        public Task CopyFile(string source, string target, bool ovewrite = true)
        {
            if (File.Exists(source))
                File.Copy(source, target, ovewrite);

            return Task.CompletedTask;
        }

        public Task CreatFolderIfNotExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            return Task.CompletedTask;
        }

        public Task DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }

        public Task DeleteFolder(string path, bool recursive = true)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, recursive);

            return Task.CompletedTask;
        }

        public string GenerateFilePath(string path, string fileName, bool useUniqueString = false)
        {
            //Reemplazar con underscore los caranteres invalidos en el path
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));

            if (useUniqueString)
                return Path.Combine(path, fileName.Insert(fileName.LastIndexOf('.'), DateTime.Now.Ticks.ToString()));
            else
                return Path.Combine(path, fileName);
        }

        public string[] GetFilesInFolder(string folder)
        {
            try
            {
                return Directory.GetFiles(folder);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<MemoryStream> GetMemoryStreamFromFile(string fullPath)
        {
            MemoryStream msResult;
            if (File.Exists(fullPath))
            {
                using (var file = File.OpenRead(fullPath))
                {
                    msResult = new MemoryStream();
                    await file.CopyToAsync(msResult).ConfigureAwait(false);
                    file.Close();
                }
            }
            else
                throw new FileNotFoundException("El Archivo no existe", fullPath);

            return msResult;

        }

        public Task<IEnumerable<string>> ListAsync(string path = "")
        {
            throw new NotImplementedException();
        }

        public Task Move(string sourceFileName, string pathDest)
        {
            if (File.Exists(sourceFileName))
            {
                try
                {
                    var fileName = Path.GetFileName(sourceFileName);

                    File.Move(sourceFileName, $"{pathDest}{fileName}");

                    return Task.CompletedTask;
                }
                catch (Exception e)
                {
                    throw new Exception("No se ha podido mover el archivo: " + e.Message);
                }
            }
            else
            {
                throw new FileNotFoundException("El archivo no existe", sourceFileName);
            }
        }

        public string Save(byte[] fileContent, string path, string fileName, bool useUniqueString)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                stream.Write(fileContent, 0, fileContent.Length);
            }
            return filePath;
        }

        public string Save(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);

            CreatFolderIfNotExists(path);

            using (var stream = File.Create(filePath))
            {
                fileStream.CopyTo(stream);
            }
            return filePath;
        }

        public async Task<string> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            string filePath = GenerateFilePath(path, fileName, useUniqueString);
            await CreatFolderIfNotExists(path);

            using (var stream = File.Create(filePath))
            {
                await fileStream.CopyToAsync(stream).ConfigureAwait(false);
            }
            return filePath;
        }

        public string SaveOnTempFolder(byte[] fileContent, string fileName, bool useUniqueString)
        {
            return Save(fileContent, TempFolder, fileName, useUniqueString);
        }
    }
}
