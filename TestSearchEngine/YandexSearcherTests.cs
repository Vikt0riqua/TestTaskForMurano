using NUnit.Framework;
using SearchEngine.Searchers;

namespace TestSearchEngine
{
    public class YandexSearcherTests
    {
        YandexSearcher _yandexSearcher = new YandexSearcher();
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestForCreateLink()
        {
            Assert.Pass();
        }
        
        [TestCase("find brain", "https://yandex.ru/search/?text=find%20brain")]
        public void TestForCreateLink(string searchQuery, string expectedLink)
        {
            var link = _yandexSearcher.CreateLinkForSearch(searchQuery);
            Assert.AreEqual(expectedLink, link);
        }
    }
}