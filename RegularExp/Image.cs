using System.Text.RegularExpressions;

namespace RegularExp
{
    class Image
    {
        public MatchCollection FindImageUrl(string htmlText)
        {
            const string pattern = @"<img.+? alt=[\""\\]+.+?[\""\\] src=[\""\\]+(?<url>\S+)[\""\\]";
  
            return Regex.Matches(htmlText, pattern, RegexOptions.IgnoreCase);
        }  

    }
}