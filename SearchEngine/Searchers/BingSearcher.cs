using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using HtmlAgilityPack;
using SearchEngine.Models;

namespace SearchEngine.Searchers
{
    public class BingSearcher : ISearcher
    {
        private const string Address = "https://www.bing.com/search?q=";
        private const string xpath = ".//ol//li[@class='b_algo']";

        public string CreateLinkForSearch(string searchString)
        {
            return Address + searchString.Trim().Replace("+","%2B").Replace(" ", "+");
        }

        public List<SearchResult> SearchResults(string resultFromSearcher)
        {
            var pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(resultFromSearcher);
            var liElements = pageDocument.DocumentNode.SelectNodes(xpath).Where(n => n.ChildNodes.Count >= 2).Take(10);
            return liElements.Select(ResultFromLiElement).Where(m => m != null).ToList();
        }

        /* Элементы с результатом выглядят следующим образом
         <li>
            <h2>
                <a href="ссылка на сайт">Заголовок</a>
            </h2>
            <div>
                <div></div>
                <div>
                    <p>Текст результата</p>
                </div>
            </div>
           </li>
        */
        /// <summary>
        /// Формирование объекта результата SearchResult из html
        /// </summary>
        /// <param name="li"> элемент результата</param>
        /// <returns></returns>
        public static SearchResult ResultFromLiElement(HtmlNode li)
        {
            try
            {
                var headerElement = li.ChildNodes[0].ChildNodes.FindFirst("a");
                var link = headerElement?.GetAttributeValue("href","");
                var header = headerElement?.InnerText;
                var textElement = li.ChildNodes[1].ChildNodes[1];
                var text = DeleteExtraSpanWithDate(textElement?.InnerText);
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