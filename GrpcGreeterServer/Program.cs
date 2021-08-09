using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Hosting;

namespace GrpcGreeter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // Additional configuration is required to successfully run gRPC on macOS.
        // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())          
            .UseKestrel((builderContext, options) =>
            {
                var sslPort = 5001;
               
                options.Listen(IPAddress.Any, sslPort,
                    listenOptions =>
                    {
                       
                            listenOptions.UseHttps("certs/local1.cer");
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;                            
                        
                    });

                options.ConfigureHttpsDefaults(o =>
                {
                    o.ClientCertificateMode = ClientCertificateMode.AllowCertificate;
                    o.AllowAnyClientCertificate();
                    o.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
                });
            })
            .UseStartup<Startup>();
    }
}
