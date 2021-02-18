using System;

namespace DocsRepoCloudIntegration
{
    public class StorageOptions
    {
        public StorageDriver Driver { get; set; }
        public Uri CloudUrlBase { get; set; }

        public string RutaTemporal { get; set; }
        public string RaizCarpetas { get; set; }
        public string TempFiles { get; set; }
        public string RutaImagenes { get; set; }
        public string Tenant { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CloudDriveUserId { get; set; }
        public string PathTemplate { get; set; }
    }

    public enum StorageDriver
    {
        FileSystem,
        OneDrive,
        GoogleDrive
    }
}