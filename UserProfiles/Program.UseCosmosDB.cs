using Microsoft.Azure.Cosmos;
using UserProfiles.Application.Repositories;

namespace UserProfiles
{
    public static class ProgramCosmosDBExtension
    {
        public static IApplicationBuilder UseCosmosDB(this IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userProfileMaxThroughput = configuration.GetValue<int>(Constants.CosmosDBOptions.UserProfileContainerMaxThroughput);
 
                var userProfileClient = serviceScope.ServiceProvider.GetRequiredService<IUserProfileRepository>();
                
                var throughputProperties = ThroughputProperties.CreateAutoscaleThroughput(userProfileMaxThroughput);
                var containerProperties = new ContainerProperties(Constants.CosmosDBOptions.IdentifierKey, Constants.CosmosDBOptions.PartitionKey);
                containerProperties.Id = Constants.CosmosDBOptions.ContainerId;

                userProfileClient.EnsureDatabaseExists(null).GetAwaiter().GetResult();
                userProfileClient.EnsureContainerExists(containerProperties, throughputProperties).GetAwaiter().GetResult();
            }

            return app;
        }
    }
}
