using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
                    .UseApplicationInsights()
                    .Build();

            string localHost = (((Microsoft.AspNetCore.Hosting.Server.Features.ServerAddressesFeature)host.ServerFeatures.Select(x => x.Value).ToList()[0]).Addresses).ToList()[0];
            CreateFileTemp(EncryptString(localHost.Split(':')[2], ENCRYPT_CODE));

            host.Run();
        }

        #region Private Static Variables and Functions

        private static string EncryptString(string text, string keyString)
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

        private static void CreateFileTemp(string encryptValue)
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

        private static string ENCRYPT_CODE = "Gu3G4nt3ngB4ng3t";
        
        #endregion
    }
}