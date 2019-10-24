using System;
using System.Collections.Generic;
using DA.Models;
using HtmlAgilityPack;

namespace BL.Searchers
{
    public class GoogleSearcher : ISearcher
    {
        private const string Address = "https://www.google.ru/search?q=";

        public string GetAddress()
        {
            return Address;
        }
        public List<SearchResult> SearchResults(HtmlDocument pageDocument)
        {
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