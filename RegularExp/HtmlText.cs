using System.Net;

namespace RegularExp
{
    class HtmlText
    {
        private readonly WebClient _webClient = new WebClient();
        public string GetHtmlText(string urlStr)
        {
            _webClient.Headers.Add("User-Agent: Other");
            return _webClient.DownloadString(urlStr);
        }
        public void Dispose() => _webClient.Dispose();        
    }
}
