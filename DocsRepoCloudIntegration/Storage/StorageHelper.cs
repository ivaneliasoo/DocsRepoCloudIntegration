using System;
using System.IO;
using System.Threading.Tasks;

namespace DocsRepoCloudIntegration
{
    public class StorageHelper : IStorageHelper
    {
        private readonly IOptions<Storage> _storageSettings;

        public StorageHelper(IOptions<StorageSettings> storageSettings)
        {
            _storageSettings = storageSettings ?? throw new ArgumentNullException(nameof(storageSettings));
            if (TempFolderRoute is null)
                TempFolderRoute = _storageSettings.Value.rutaTemporal;
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}
