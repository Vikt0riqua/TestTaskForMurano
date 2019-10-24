using System;
using System.Collections.Generic;
using System.Linq;
using DA.Models;
using HtmlAgilityPack;

namespace BL.Searchers
{
    public class YandexSearcher : ISearcher
    {
        private const string Address = "https://yandex.ru/search/?text=";
        private const string xPathForResults = ".//ul[@aria-label='Результаты поиска']//li";


        public string GetAddress()
        {
            return Address;
        }

        public List<SearchResult> SearchResults(HtmlDocument pageDocument)
        {
            var liElements = pageDocument.DocumentNode.SelectNodes(xPathForResults);
           return liElements.Select(ResultFromLiElement).Where(m => m != null).Take(10).ToList();
        }
        
        /* Элементы с результатом выглядят следующим образом
         <li>
            ...
              <div> 
                  <h2 class = "organic__title-wrapper"> 
                        <div></div>
                        <a href="ссылка на сайт">Заголовок</a>
                  </h2>
                  
                  <div></div>
                  
                  <div class="organic__content-wrapper">
                        ...
                        <div class="text-container"> 
                            Текст результата (Если есть кроткий и длинный текст, то сам текст в<span>)
                        </div>
                        ...
                 </div>
              </div>
            ...
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
                var aElement = li.SelectSingleNode(".//h2[contains(@class, 'organic__title-wrapper')]//a");
                var header = aElement?.InnerText.Replace("&quot;", "");
                var link = aElement?.GetAttributeValue("href", "");
                var txtElement = li.SelectSingleNode(".//div[contains(@class,'organic__content-wrapper')]//div[contains(@class,'text-container')]");
                var fullTextElement = txtElement?.SelectSingleNode(".//span[contains(@class,'extended-text__full')]");
                var text = fullTextElement == null ? txtElement?.InnerText : DeleteExtraWord(fullTextElement.InnerText);
                return new SearchResult {Header = header,Link = link,ResultText = text?.Replace("&quot;","\"")};

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Метод, который удаляет слово "Скрыть" в конце результата поиска из полного текста.
        /// </summary>
        /// <param name="text">полный текст результата поиска</param>
        /// <returns></returns>
        private static string DeleteExtraWord(string text)
        {
            return text.Substring(0, text.Length - 6);
        }
    }
}