using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocsRepoCloudIntegration
{
    public static class CloudStorageIntegration
    {
        public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration, Action<StorageOptions> setupAction = null)
        {
            if(setupAction != null)
            {
                services.Configure(setupAction);
            }
            return services;
        }
    }
}
