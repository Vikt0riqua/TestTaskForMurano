using BL.Searchers;
using NUnit.Framework;

namespace SearchEngineBLTests.SearchersTests
{
    public class LinkCreatorTest
    {
        [TestCase("https://yandex.ru/search/?text=", "Moscow subway", "https://yandex.ru/search/?text=Moscow+subway")]
        [TestCase("https://www.google.ru/search?q=", "Московское метро", "https://www.google.ru/search?q=%d0%9c%d0%be%d1%81%d0%ba%d0%be%d0%b2%d1%81%d0%ba%d0%be%d0%b5+%d0%bc%d0%b5%d1%82%d1%80%d0%be")]
        public void CreateLinkForSearch_Test(string address, string searchQuery, string expectedLink)
        {
            var link = LinkCreator.CreateLinkForSearch(address, searchQuery);
            Assert.AreEqual(expectedLink, link);
        }
    }
}