using System;
using System.Net;
using System.Text.RegularExpressions;

namespace RegularExp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input url");
            var urlStr = Console.ReadLine();

            var htmlText = new HtmlText();
            var hText = htmlText.GetHtmlText(urlStr);

            var imageUrl = new Image();
                
            var mathes = imageUrl.FindImageUrl(hText);

            foreach(Match match in mathes)
            {
                var fileDownload = new FileDownload();
                fileDownload.DownloadFile(match.Groups["url"].Value, @"C:\Users\Public\Pictures");
                Console.WriteLine(match.Groups["url"].Value);
            }            

            htmlText.Dispose();
        }

        

    }
}
