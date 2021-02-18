using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DocsRepoCloudIntegration
{
    public interface IStorageDriver
    {
        Task<IEnumerable<string>> ListAsync(string path = "");
        Task CopyFile(string source, string target, bool ovewrite = true);
        Task CreatFolderIfNotExists(string folderPath);
        Task DeleteFile(string filePath);
        Task DeleteFolder(string path,  bool recursive = true);
        string GenerateFilePath(string path, string fileName, bool useUniqueString = false);
        string[] GetFilesInFolder(string folder);
        Task<MemoryStream> GetMemoryStreamFromFile(string fullPath);
        string Save(byte[] fileContent, string path, string fileName, bool useUniqueString);
        string Save(Stream fileStream, string path, string fileName, bool useUniqueString);
        Task<string> SaveAsync(Stream fileStream, string path, string fileName, bool useUniqueString);
        string SaveOnTempFolder(byte[] fileContent, string fileName, bool useUniqueString);
        Task Move(string sourceFileName, string pathDest);
    }
}
