namespace DocsRepoCloudIntegration.Storage
{
    public interface IStorageDriverFactory
    {
        IStorageDriver CreateStorageDriver(StorageDriver driver);
    }
}