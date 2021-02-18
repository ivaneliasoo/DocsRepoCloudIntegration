using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DocsRepoCloudIntegration
{
    class GoogleDriveStorageDriver : StorageBase, IStorageDriver
    {
        public Task CopyFile(string source, string target, bool ovewrite = true)
        {
            throw new NotImplementedException();
        }

        public Task CreatFolderIfNotExists(string folderPath)
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

        public Task<IEnumerable<string>> ListAsync(string path = "")
        {
            throw new NotImplementedException();
        }

        public Task Move(string sourceFileName, string pathDest)
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

        public Task<string> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }

        public string SaveOnTempFolder(byte[] fileContent, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }
    }

}
