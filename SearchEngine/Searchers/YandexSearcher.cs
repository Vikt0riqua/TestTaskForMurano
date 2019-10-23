using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using HtmlAgilityPack;
using SearchEngine.Models;

namespace SearchEngine.Searchers
{
    public class YandexSearcher : ISearcher
    {
        private const string Address = "https://yandex.ru/search/?text=";
        private const string xPathForResults = ".//ul[@aria-label='Результаты поиска']//li";

        public string CreateLinkForSearch(string searchString)
        {
            return Address + searchString.Trim().Replace("%", "%25").Replace(" ", "%20");
        }

        public List<SearchResult> SearchResults(string resultFromSearcher)
        {
            var pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(resultFromSearcher);
            var liElements = GetCorrectLiElementList(pageDocument);
           return liElements.Select(ResultFromLiElement).Where(m => m != null).ToList();
        }

        /// <summary>
        /// Функция возвращает коллекцию HtmlNode, где каждый node это результат выданный поисковиком
        /// </summary>
        /// <param name="resultFromSearcher">HtmlDocument со страницей поисковика</param>
        /// <returns></returns>
        public static IEnumerable<HtmlNode> GetCorrectLiElementList(HtmlDocument resultFromSearcher)
        {
            var liItems = resultFromSearcher.DocumentNode.SelectNodes(xPathForResults);
            return liItems.Where(m => m.ChildNodes[0].ChildNodes.Count >= 3).Take(10);
        }
        
        /* Элементы с результатом выглядят следующим образом
         <li>
              <div> 
                  <h2> 
                    <div></div>
                    <a href="ссылка на сайт">Заголовок</a>
                  </h2>
                  <div></div>
                  <div>Текст результата</div>
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
                var root = li.ChildNodes[0]; // <div> 
                var titleElement = root?.ChildNodes[0].ChildNodes.FindFirst("a"); // <a>
                var link = titleElement?.GetAttributeValue("href", "");
                var header = titleElement?.InnerText.Replace("&quot;", "");
                var textElement = root?.ChildNodes[2]; // <div>
                var text = textElement?.InnerText;
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