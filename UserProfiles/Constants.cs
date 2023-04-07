namespace UserProfiles
{
    public static class Constants
    {
        public static class ConnetionStrings
        {
            public const string CosmosDbConnectionString = nameof(CosmosDbConnectionString);
        }

        public static class CosmosDBOptions
        {
            public const string ContainerId = "UserProfileModels";
            public const string Database = "UserProfile";
            public const string UserProfileContainerMaxThroughput = nameof(UserProfileContainerMaxThroughput);
            public const string IdentifierKey = "id";
            public const string PartitionKey = "/_partitionKey";
        }
    }
}
