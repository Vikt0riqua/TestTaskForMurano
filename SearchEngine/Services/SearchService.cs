using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using SearchEngine.Models;
using SearchEngine.Searchers;

namespace SearchEngine.Services
{
    public class SearchService
    {
        private readonly ResultsStorage _resultsStorage;
        private readonly IBrowsingContext _context;
        private static readonly Dictionary<int, ISearcher> BrowserList = new Dictionary<int, ISearcher>
        {
            {1, new YandexSearcher()},
            {2, new GoogleSearcher()},
            {3, new BingSearcher()},
        };


        public SearchService(ResultsStorage resultsStorage)
        {
            _resultsStorage = resultsStorage;
            var config = new Configuration().WithDefaultLoader();
            _context = BrowsingContext.New(config);
        }

        public async Task<List<SearchResult>> SearchForResults(string searchString)
        {
            var (index, document) = await Search(searchString);
            var resultsList = BrowserList[index].SearchResults(document);
            await _resultsStorage.SaveResults(resultsList);
            return resultsList;
        }

        private async Task<(int index, IDocument document)> Search(string searchString)
        {
            var tasks = BrowserList.Select(m => GetTuple(m, searchString)).ToArray();
            var firstTask = await Task.WhenAny(tasks);
            return firstTask.Result;
        }

        private async Task<(int index, IDocument document)> GetTuple(KeyValuePair<int, ISearcher> browser, string searchString)
        {
            var res = await _context.OpenAsync(browser.Value.CreateLinkForSearch(searchString));
            return (browser.Key, res);
        }
    }
}