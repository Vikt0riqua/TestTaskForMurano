using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DA.Models;
using HtmlAgilityPack;

namespace BL.Searchers
{
    public class YandexSearcher : ISearcher
    {
        private const string Address = "https://yandex.ru/search/?text=";
        private const string XPathForResults = ".//ul[@aria-label='Результаты поиска']//li";
        private const string ExtraWord = "Скрыть";
        

        public string GetAddress()
        {
            return Address;
        }

        public List<SearchResult> SearchResults(HtmlDocument pageDocument)
        {
            try
            {
                var liElements = pageDocument?.DocumentNode.SelectNodes(XPathForResults);
                return liElements == null ? new List<SearchResult>() : liElements.Select(ResultFromLiElement).Where(m => m != null).Take(10).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при поиске элементов li на странице с результатами для Yandex: " + e);
            }
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
        public SearchResult ResultFromLiElement(HtmlNode li)
        {
            try
            {
                if (li == null) return null;
                var aElement = li.SelectSingleNode(".//h2[contains(@class, 'organic__title-wrapper')]//a");
                var header = WebUtility.HtmlDecode(aElement?.InnerText);
                if (string.IsNullOrEmpty(header)) return null;
                var link = aElement?.GetAttributeValue("href", "");
                if (string.IsNullOrEmpty(link)) return null;
                var txtElement = li.SelectSingleNode(".//div[contains(@class,'organic__content-wrapper')]//div[contains(@class,'text-container')]");
                var fullTextElement = txtElement?.SelectSingleNode(".//span[contains(@class,'extended-text__full')]");
                var text = fullTextElement == null ? txtElement?.InnerText : DeleteExtraWord(fullTextElement.InnerText);
                if (string.IsNullOrEmpty(text)) return null;
                return new SearchResult {Header = header,Link = link,ResultText = WebUtility.HtmlDecode(text)};
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при формировании результата из HTMLNode для YandexBrowser: " + e);
            }
        }

        /// <summary>
        /// Метод, который удаляет слово "Скрыть" в конце результата поиска из полного текста.
        /// </summary>
        /// <param name="text">полный текст результата поиска</param>
        /// <returns></returns>
        public string DeleteExtraWord(string text)
        {
            if (text != null)
            {
                return text.EndsWith(ExtraWord) ? text.Substring(0, text.Length - ExtraWord.Length - 1) : text;
            }
            return "";
        }
    }
}