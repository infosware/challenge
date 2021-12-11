using Microsoft.Extensions.Configuration;
using System.IO;

namespace SuperPanel.App.Helpers
{
    public class AppSettings
    {
        private static IConfiguration config;
        public static IConfiguration Configuration
        {
            get
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                config = builder.Build();
                return config;
            }
        }


        public static int DefaultPageSize = Configuration.GetValue<int>("Constants:DefaultPageSize");
        
        public static string[] PaginationOptions = Configuration.GetSection("Constants:PaginationOptions").Get<string[]>();
    }
}
