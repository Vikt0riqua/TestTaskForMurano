using BL.Searchers;
using DA.Models;
using HtmlAgilityPack;
using NUnit.Framework;

namespace SearchEngineBLTests.SearchersTests
{
    public class GoogleSearcherTests
    {
        private readonly GoogleSearcher _googleSearcher = new GoogleSearcher();
        
        [TestCase("https://www.google.ru/search?newwindow=1&source=hp&q=")]
        public void GetAddress_Test(string expectedAddress)
        {
            var address = _googleSearcher.GetAddress();
            Assert.AreEqual(expectedAddress, address);
        }

        [TestCase("", "")]
        [TestCase("<span><em>Клубни́ка</em> (от др.-рус. клуб «клубок, шарообразное тело») — название растений и их плодов (ягод, многоорешков) некоторых видов рода Земляника&nbsp;...<span/>",
            "<span><em>Клубни́ка</em> (от др.-рус. клуб «клубок, шарообразное тело») — название растений и их плодов (ягод, многоорешков) некоторых видов рода Земляника&nbsp;...<span/>")]
        [TestCase("<span><span class=\"f\">26 янв. 2018 г. - </span><em>Клубника</em> (Fragaria moschata или Fragaria elatior) ― именно так с 18 века ученые начали называть землянику мускатную. У данного&nbsp;...</span>",
            "<span><em>Клубника</em> (Fragaria moschata или Fragaria elatior) ― именно так с 18 века ученые начали называть землянику мускатную. У данного&nbsp;...</span>")]
        public void DeleteExtraSpanWithDate_Test(string forNode, string forExpectedNode)
        {
            //arrange
            HtmlNode htmlNode = HtmlNode.CreateNode(forNode);
            HtmlNode expectedSearchNode = HtmlNode.CreateNode(forExpectedNode);

            //act 
            var result = _googleSearcher.DeleteExtraSpanWithDate(htmlNode);

            //assert
            Assert.AreEqual(expectedSearchNode?.InnerHtml, result?.InnerHtml);
        }

        [TestCase("", true)]
        [TestCase(Constants.DivHtmlWhitErrorForGoogle, true)]
        [TestCase(Constants.CorrectDivHtmlForGoogle, false,
            "Клубника — Википедия",
            "https://ru.wikipedia.org/wiki/%D0%9A%D0%BB%D1%83%D0%B1%D0%BD%D0%B8%D0%BA%D0%B0",
            "Клубни́ка (от др.-рус. клуб «клубок, шарообразное тело») — название растений и их плодов (ягод, многоорешков) некоторых видов рода Земляника ...")]
        public void ResultFromLiElement_NullNode_Test(string forNode, bool isExpectedResultNull, string header = "", string link = "", string text = "")
        {
            //arrange
            var divNode = HtmlNode.CreateNode(forNode);
            var expectedSearchResult = isExpectedResultNull
                ? null
                : new SearchResult
                {
                    Header = header,
                    Link = link,
                    ResultText = text
                };

            //act 
            var result = _googleSearcher.ResultFromElement(divNode);

            //assert
            Assert.AreEqual(expectedSearchResult, result);
        }
    }
}