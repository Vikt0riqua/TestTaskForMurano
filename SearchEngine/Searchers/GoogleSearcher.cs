using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using HtmlAgilityPack;
using SearchEngine.Models;

namespace SearchEngine.Searchers
{
    public class GoogleSearcher : ISearcher
    {
        private const string Address = "https://www.google.ru/search?q=";

        public string CreateLinkForSearch(string searchString)
        {
            return Address + searchString.Trim().Replace("+","%2B").Replace(" ", "+");
        }

        public List<SearchResult> SearchResults(string resultFromSearcher)
        {
            var pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(resultFromSearcher);
            var liElements = pageDocument.DocumentNode.SelectNodes(".//div[@class='g']");
            //var elementsList = resultFromSearcher.QuerySelectorAll("div.rc");
            return null; //elementsList.Take(10).Select(ResultFromElement).Where(m => m != null).ToList();
        }

        /*public static SearchResult ResultFromElement(IElement div)
        {
            try
            {
                var headerElement = div.QuerySelectorAll("div.r a")[0];
                var link = headerElement.GetAttribute("href");
                var header = headerElement.GetElementsByTagName("h3")[0].Text();
                var text = DeleteExtraSpanWithDate(div.QuerySelectorAll("div.s span.st")[0].Text());
                return new SearchResult {Header = header,Link = link,ResultText = text};
            }
            catch (Exception)
            {
                return null;
            }
        }*/

        private static string DeleteExtraSpanWithDate(string text)
        {
            if (!text.Contains("<span class=\"f\">")) return text;
            var index = text.LastIndexOf("</span>", StringComparison.Ordinal);
            text = text.Substring(index);
            return text;

        }
    }
}