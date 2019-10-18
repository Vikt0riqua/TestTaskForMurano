using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using AngleSharp.Dom;
using SearchEngine.Models;

namespace SearchEngine.Searchers
{
    public class YandexSearcher : ISearcher
    {
        private const string Address = "https://yandex.ru/search/?text=";
        private const string LiItemSelector = "ul[aria-label=\"Результаты поиска\"] li";

        public string CreateLinkForSearch(string searchString)
        {
            return Address + searchString.Trim().Replace("%", "%25").Replace(" ", "%20");
        }

        public List<SearchResult> SearchResults(IDocument resultFromSearcher)
        {
            var liElements = GetCorrectLiElementList(resultFromSearcher);
            return liElements.Select(ResultFromLiElement).Where(m => m != null).ToList();
        }

        public static IEnumerable<IElement> GetCorrectLiElementList(IDocument resultFromSearcher)
        {
            var liItems = resultFromSearcher.QuerySelectorAll(LiItemSelector);
            return liItems.Where(m => m.ChildNodes[0].ChildNodes.Length >= 3).Take(10);
        }

        public static SearchResult ResultFromLiElement(IElement li)
        {
            try
            {
                var root = li.GetElementsByTagName("div")[0];
                var titleElement = root.GetElementsByTagName("h2")[0];
                var link = titleElement.GetElementsByTagName("a")[0].GetAttribute("href");
                var header = titleElement.GetElementsByTagName("a")[0].Text();
                var text = root.GetElementsByClassName("extended-text__full").Length > 0 
                    ? DeleteExtraWord(root.GetElementsByClassName("extended-text__full")[0].Text()) 
                    : root.GetElementsByClassName("text-container")[0].Text();
                return new SearchResult {Header = header,Link = link,ResultText = text};
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string DeleteExtraWord(string text)
        {
            return text.Substring(0, text.Length - 7);
        }
    }
}