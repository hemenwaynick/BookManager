using Microsoft.Extensions.Configuration;

namespace BookManager.Config
{
    public static class Configuration
    {
        public static string GetConnectionString(string key)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(@"Config\appsettings.json")
                .Build();

            return config[key];
        }
    }
}
