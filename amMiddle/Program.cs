using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace amMiddle
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .UseUrls(string.Format(" http://localhost:{0}", GetOpenPort()))
                    .UseApplicationInsights()
                    .Build();

            host.Run();
        }

        private static string GetOpenPort()
        {
            int PortStartIndex = 1;
            int PortEndIndex = 65535;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndPoints.Select(p => p.Port).ToList<int>();
            int unusedPort = 0;

            for (int port = PortStartIndex; port < PortEndIndex; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    unusedPort = port;
                    break;
                }
            }
            return unusedPort.ToString();
        }
    }
}