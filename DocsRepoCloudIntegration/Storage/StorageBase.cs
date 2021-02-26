using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocsRepoCloudIntegration
{
    internal abstract class StorageBase
    {
        public string SystemBaseFolder { get; set; } = "ZadERP";
        public string TempFolder { get; set; } = "TEMP";
        public int SmallFileSize { get; set; } = 4; // In MB

        public StorageBase(IOptionsMonitor<StorageOptions> options)
        {

        }
        public StorageBase(StorageOptions options)
        {

        }

        public StorageBase()
        {

        }

        internal void Configure(Action<StorageOptions> setup)
        {

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
    }
}
