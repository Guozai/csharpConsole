// Many places used Matthew Bolger's code from InventoryPriceManagement and 
// Program.cs at http://coreteaching01.csit.rmit.edu.au/~e87149/wdt/

using Microsoft.Extensions.Configuration;

namespace Assignment1
{
    public static class Program
    {
        private static IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public static string ConnectionString { get; } = Configuration["ConnectionString"];

        private static void Main()
        {
            Menu menu = new Menu();
            menu.Start();
        }
    }
}
