using System;
using System.IO;
using System.Net;

namespace RegularExp
{
    class FileDownload
    {
        private readonly WebClient _webClient = new WebClient();
        public void Dispose() => _webClient.Dispose();
        public void DownloadFile(string url, string toPath)
        {
            Directory.CreateDirectory(toPath);

            _webClient.DownloadFile(url, Path.Combine(toPath, Path.GetFileName(url)));
            Console.WriteLine($"{url} is done");
        }
    }
}
