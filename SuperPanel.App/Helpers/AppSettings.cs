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
        
        public static int ServiceRetryCount = Configuration.GetValue<int>("Constants:ServiceRetryCount");

        public static string[] PaginationOptions = Configuration.GetSection("Constants:PaginationOptions").Get<string[]>();

        public static string ExternalContactsApiUrl = Configuration.GetValue<string>("Constants:ExternalContactsApiUrl");
    }
}
