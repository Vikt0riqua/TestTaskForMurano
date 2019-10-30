using BL.Searchers;
using DA.Models;
using HtmlAgilityPack;
using NUnit.Framework;

namespace SearchEngineBLTests.SearchersTests
{
    public class BingSearcherTests
    {
        private readonly BingSearcher _bingSearcher = new BingSearcher();
        
        [TestCase("https://www.bing.com/search?qs=ds&form=QBRE&q=")]
        public void GetAddress_Test(string expectedAddress)
        {
            var address = _bingSearcher.GetAddress();
            Assert.AreEqual(expectedAddress, address);
        }

        
        [TestCase("", "")]
        //строка со span
        [TestCase("<span><span class=\"news_dt\">26.01.2018</span>&nbsp;&#0183;&#32;<strong>Клубника</strong> (Fragaria moschata или Fragaria elatior) ― именно так с 18 века ученые начали называть землянику мускатную. У данного растение существует множество названий, к примеру: <strong>клубника</strong> садовая, либо ...</span>",
            "<span>&nbsp;&#0183;&#32;<strong>Клубника</strong> (Fragaria moschata или Fragaria elatior) ― именно так с 18 века ученые начали называть землянику мускатную. У данного растение существует множество названий, к примеру: <strong>клубника</strong> садовая, либо ...</span>")]
        //строка без span с датой
        [TestCase("<span>Раскраска <strong>клубника</strong> даст возможность малышам раскрасить одно из любимейших лакомств каждого ребёнка. Сочные и спелые ягоды в раскраске <strong>клубника</strong> …</span>",
            "<span>Раскраска <strong>клубника</strong> даст возможность малышам раскрасить одно из любимейших лакомств каждого ребёнка. Сочные и спелые ягоды в раскраске <strong>клубника</strong> …</span>")]
        public void DeleteExtraSpanWithDate_Test(string forNode, string forExpectedNode)
        {
            //arrange
            var htmlNode = HtmlNode.CreateNode(forNode);
            var expectedSearchNode = HtmlNode.CreateNode(forExpectedNode);

            //act 
            var result = _bingSearcher.DeleteExtraSpanWithDate(htmlNode);

            //assert
            Assert.AreEqual(expectedSearchNode?.InnerHtml, result?.InnerHtml);
        }

        [TestCase("", true)]
        [TestCase(Constants.LiHtmlWhitErrorForBing, true)]
        [TestCase(Constants.CorrectLiHtmlForBing, false,
            "Раскраска клубника. Распечатать картинки с ягодой.", 
            "http://vse-raskraski.ru/frukty-i-ovoshchi-raskraski/raskraska-klubnika",
            "Раскраска клубника даст возможность малышам раскрасить одно из любимейших лакомств каждого ребёнка. Сочные и спелые ягоды в раскраске клубника …")]
        public void ResultFromLiElement_Test(string forNode, bool isExpectedResultNull, string header = "", string link = "", string text = "")
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
            var result = _bingSearcher.ResultFromLiElement(liNode);

            //assert
            Assert.AreEqual(expectedSearchResult, result);
        }
    }
}