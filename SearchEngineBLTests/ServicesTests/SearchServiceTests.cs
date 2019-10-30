using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BL;
using BL.Services;
using HtmlAgilityPack;
using NUnit.Framework;

namespace SearchEngineBLTests.ServicesTests
{
    public class SearchServiceTests
    {
        private readonly SearchService _searchService = new SearchService();
        readonly List<int> _listForTask = new List<int> { 1, 2, 3 };

        private async Task<BrowserResult> CreateTask(int index)
        {
            await Task.Delay(index * 100);
            return new BrowserResult { BrowserIndex = index, ResultDocument = new HtmlDocument()};
        }

        [Test]
        public async Task Search_Test()
        {
            //arrange
            var tasks = _listForTask.Select(CreateTask);
            var expectedTask = new BrowserResult{BrowserIndex = 1, ResultDocument = new HtmlDocument()};

            //act 
            var result =await _searchService.Search(tasks);

            //assert
            Assert.AreEqual(expectedTask.BrowserIndex, result.BrowserIndex);
        }
    }
}