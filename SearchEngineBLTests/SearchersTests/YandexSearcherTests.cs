using BL.Searchers;
using DA.Models;
using HtmlAgilityPack;
using NUnit.Framework;

namespace SearchEngineBLTests.SearchersTests
{
    public class YandexSearcherTests
    {
        private readonly YandexSearcher _yandexSearcher = new YandexSearcher();
        
        [TestCase("https://yandex.ru/search/?text=")]
        public void GetAddress_Test(string expectedAddress)
        {
            var address = _yandexSearcher.GetAddress();
            Assert.AreEqual(expectedAddress, address);
        }


        [TestCase("", "")]
        [TestCase(null, "")]
        [TestCase("Какой-то текст для тестирования. Скрыть", "Какой-то текст для тестирования.")]
        [TestCase("Какой-то текст для тестирования.", "Какой-то текст для тестирования.")]
        public void DeleteExtraWord_Test(string text, string expectedText)
        {
            var resultText = _yandexSearcher.DeleteExtraWord(text);
            Assert.AreEqual(expectedText, resultText);
        }


        [TestCase("", true)]
        [TestCase(Constants.LiHtmlWhitErrorForYandex, true)]
        [TestCase(Constants.CorrectLiHtmlForYandex, false,
            "Клубника – Польза и Вред Для Вашего Здоровья (+14 Фото)",
            "https://edaplus.info/produce/strawberry.html",
            "Лечебные свойства клубники. Клубника – богатейший источник таких питательных веществ как витамины А, В, С, Е и минералы (калий, фосфор, кальций, магний, кальций, железо, йод, марганец). Помимо этого, ягода содержит целый набор непитательных биологически активных составляющих (фенольные соединения), которые вместе оказывают синергетическое действие на организм, укрепляя здоровье и предотвращая развитие различных заболеваний.")]
        public void ResultFromLiElement_NullNode_Test(string forNode, bool isExpectedResultNull, string header = "", string link = "", string text = "")
        {
            //arrange
            var liNode = HtmlNode.CreateNode(forNode);
            var expectedSearchResult = isExpectedResultNull
                ? null
                : new SearchResult
                {
                    Header = header,
                    Link = link,
                    ResultText = text
                };

            //act 
            var result = _yandexSearcher.ResultFromLiElement(liNode);

            //assert
            Assert.AreEqual(expectedSearchResult, result);
        }
    }
}