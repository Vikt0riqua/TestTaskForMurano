using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DA.Models;
using HtmlAgilityPack;

namespace BL.Searchers
{
    public class GoogleSearcher : ISearcher
    {
        private const string Address = "https://www.google.ru/search?newwindow=1&source=hp&q=";
        private const string XPathForResults = ".//div[contains(@class,'g')]//div[contains(@class,'rc')]";

        public string GetAddress()
        {
            return Address;
        }
        public List<SearchResult> SearchResults(HtmlDocument pageDocument)
        {
            try
            {
                var divElements = pageDocument.DocumentNode.SelectNodes(XPathForResults);
                return divElements == null ? new List<SearchResult>() : divElements.Select(ResultFromElement).Where(m => m != null).Take(10).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при поиске элементов li на странице с результатами для Google: " + e);
            }
        }

        /* Элементы с результатом выглядят следующим образом
        <div class="rc">
            <div class="r">
                <a href="ссылка на сайт">
                    <h3>Заголовок</h3>
                </a>
            </div>
            <div class="s">
                <div>
                    <span class="st">
                        текст результата
                    </span>
                </div>
            </div>
        </div>
        */
        /// <summary>
        /// Формирование объекта результата SearchResult из html
        /// </summary>
        /// <param name="div"> элемент результата</param>
        /// <returns></returns>
        public SearchResult ResultFromElement(HtmlNode div)
        {
            try
            {
                var aElement = div?.SelectSingleNode(".//div[contains(@class,'r')]//a");
                if (aElement == null) return null;
                //header
                var headerElement = aElement.SelectSingleNode(".//h3");
                var header = WebUtility.HtmlDecode(headerElement?.InnerText);
                if (string.IsNullOrEmpty(header)) return null;
                //link
                var link = aElement.GetAttributeValue("href", "");
                if (string.IsNullOrEmpty(link)) return null;
                //text
                var txtElement = div?.SelectSingleNode(".//div[contains(@class,'s')]//span[contains(@class,'st')]");
                if (txtElement == null) return null;
                bool haveDateSpan = txtElement.InnerHtml.Contains("<span");
                var text = haveDateSpan
                    ? DeleteExtraSpanWithDate(txtElement).InnerText
                    : txtElement.InnerText;
                if (string.IsNullOrEmpty(text)) return null;
                return new SearchResult { Header = header, Link = link, ResultText = WebUtility.HtmlDecode(text)};
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при формировании результата из HTMLNode для GoogleBrowser: " + e);
            }
        }

        public HtmlNode DeleteExtraSpanWithDate(HtmlNode textNode)
        {
            var spanDate = textNode?.SelectSingleNode(".//span[contains(@class,'f')]");
            if (spanDate != null)
            {
                textNode.ChildNodes.Remove(spanDate);
            }
            return textNode;
        }
    }
}