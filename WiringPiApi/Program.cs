using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WiringPiApi
{
    public class Program
    {
        #region ///  Methods  ///

        public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args).UseUrls("http://*:8000").UseStartup<Startup>().Build();

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        #endregion
    }
}