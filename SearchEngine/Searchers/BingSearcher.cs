using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using AngleSharp.Dom;
using SearchEngine.Models;

namespace SearchEngine.Searchers
{
    public class BingSearcher : ISearcher
    {
        private const string Address = "https://www.bing.com/search?q=";
        private const string LiItemSelector = "ol li.b_algo";

        public string CreateLinkForSearch(string searchString)
        {
            return Address + searchString.Trim().Replace("+","%2B").Replace(" ", "+");
        }

        public List<SearchResult> SearchResults(IDocument resultFromSearcher)
        {
            var liElements = resultFromSearcher.QuerySelectorAll(LiItemSelector).Take(10);
            return liElements.Select(ResultFromLiElement).Where(m => m != null).ToList();
        }

        public static SearchResult ResultFromLiElement(IElement li)
        {
            try
            {
                var headerElement = li.QuerySelectorAll("div.b_title h2 a")[0];
                var link = headerElement.GetAttribute("href");
                var header = headerElement.Text();
                var text = DeleteExtraSpanWithDate(li.QuerySelectorAll("div.b_caption p")[0].Text());
                return new SearchResult {Header = header,Link = link,ResultText = text};
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        private static string DeleteExtraSpanWithDate(string text)
        {
            if (!text.Contains("<span class=\"news_dt\">")) return text;
            var index = text.LastIndexOf("</span>", StringComparison.Ordinal);
            text = text.Substring(index);
            return text;

        }
    }
}