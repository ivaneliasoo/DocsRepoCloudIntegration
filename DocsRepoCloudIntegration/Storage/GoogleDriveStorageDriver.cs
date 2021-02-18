using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DocsRepoCloudIntegration
{
    class GoogleDriveStorageDriver : CloudStorageBase, IStorageDriver
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

        public ValueTask<bool> DeleteFileInFolder(string path, string name)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFilesInFolder(string path)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFolder(string path)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFolder(string path, bool recursive = true)
        {
            throw new NotImplementedException();
        }

        public ValueTask<bool> FileExists(string filePath)
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

        public string SaveOnTempFolder(Stream fileContent, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveOnTempFolderAsync(Stream fileContent, string fileName, bool useUniqueString)
        {
            throw new NotImplementedException();
        }
    }

}
