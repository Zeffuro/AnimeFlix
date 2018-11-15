using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace AnimeFlix
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //.UseKestrel(options =>
                //{
                //    options.UseHttps(path, "CertificatePassword")
                //        .UseConnectionLogging();
                //})
                .UseUrls("http://*:5000")
                .Build();
        }
    }
}