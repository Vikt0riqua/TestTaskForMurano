using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DA.Models;
using HtmlAgilityPack;

namespace BL.Searchers
{
    public class BingSearcher : ISearcher
    {
        private const string Address = "https://www.bing.com/search?qs=ds&form=QBRE&q=";
        private const string XPathForResults = ".//ol//li[@class='b_algo']";

        public string GetAddress()
        {
            return Address;
        }
        public List<SearchResult> SearchResults(HtmlDocument pageDocument)
        {
            try
            {
                var liElements = pageDocument.DocumentNode.SelectNodes(XPathForResults);
                return liElements == null ? new List<SearchResult>() : liElements.Select(ResultFromLiElement).Where(m => m != null).Take(10).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при поиске элементов li на странице с результатами для Bing: " + e);
            }
        }

        /* Элементы с результатом выглядят следующим образом
         <li class="b_algo">
            <h2>
                <a href="ссылка на сайт">Заголовок</a>
            </h2>
            <div class="b_caption">
                <div></div>
                <p>Текст результата</p>
            </div>
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
                var aElement = li.SelectSingleNode(".//h2//a");
                var header = WebUtility.HtmlDecode(aElement?.InnerText);
                if (string.IsNullOrEmpty(header)) return null;
                var link = aElement?.GetAttributeValue("href", "");
                if (string.IsNullOrEmpty(link)) return null;
                var txtpElement = li.SelectSingleNode(".//div[contains(@class,'b_caption')]//p");
                if (txtpElement == null) return null;
                bool haveDateSpan = txtpElement.InnerHtml.Contains("<span");
                var text = haveDateSpan
                    ? DeleteExtraSpanWithDate(txtpElement).InnerText
                    : txtpElement.InnerText;
                if (string.IsNullOrEmpty(text)) return null;
                return new SearchResult {Header = header, Link = link, ResultText = WebUtility.HtmlDecode(text)};
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при формировании результата из HTMLNode для BingBrowser: " + e);
            }
        }

        public HtmlNode DeleteExtraSpanWithDate(HtmlNode textNode)
        {
            var spanDate = textNode?.SelectSingleNode(".//span[contains(@class,'news_dt')]");
            if (spanDate != null)
            {
                textNode.ChildNodes.Remove(spanDate);
            }
            return textNode;
        }
    }
}