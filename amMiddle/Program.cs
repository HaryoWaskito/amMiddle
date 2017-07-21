using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Security.Cryptography;

namespace amMiddle
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //string localHost = string.Format("http://localhost:{0}", GetOpenPort());

            var host = new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    //.UseUrls(localHost)
                    .UseApplicationInsights()
                    .Build();

            string localHost = (((Microsoft.AspNetCore.Hosting.Server.Features.ServerAddressesFeature)host.ServerFeatures.Select(x => x.Value).ToList()[0]).Addresses).ToList()[0];
            CreateFileTemp(EncryptString(localHost.Split(':')[2], "Gu3G4nt3ngB4ng3t"));

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

            CreateFileTemp(EncryptString(unusedPort.ToString(), "Gu3G4nt3ngB4ng3t"));

            return unusedPort.ToString();
        }

        public static string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static void CreateFileTemp(string encryptValue)
        {
            string path = @"C:\Temp\VivaLaVida.txt";
            Directory.CreateDirectory("C:\\Temp");

            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(encryptValue);
                }
            }
        }
    }
}