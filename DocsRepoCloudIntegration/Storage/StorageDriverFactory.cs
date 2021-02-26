using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsRepoCloudIntegration.Storage
{
    public class StorageDriverFactory : IStorageDriverFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IOptionsMonitor<StorageOptions> _options;

        public StorageDriverFactory(ILoggerFactory loggerFactory, IOptionsMonitor<StorageOptions> options)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }
        public IStorageDriver CreateStorageDriver(StorageDriver driver)
        {
            return driver switch
            {
                StorageDriver.FileSystem => new FileSystemDriver(_loggerFactory.CreateLogger<FileSystemDriver>(), _options),
                StorageDriver.OneDrive => new OneDriveStorageDriver(_loggerFactory.CreateLogger<OneDriveStorageDriver>(), _options),
                StorageDriver.GoogleDrive => new GoogleDriveStorageDriver(_loggerFactory.CreateLogger<GoogleDriveStorageDriver>(), _options),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
