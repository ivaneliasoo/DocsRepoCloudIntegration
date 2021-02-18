using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocsRepoCloudIntegration
{
    internal abstract class StorageBase
    {
        public string SystemBaseFolder { get; set; } = "ZadERP";
        public string TempFolder { get; set; } = "TEMP";
        
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
    }
}
