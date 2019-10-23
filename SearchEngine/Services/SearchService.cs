using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SearchEngine.Models;
using SearchEngine.Searchers;

namespace SearchEngine.Services
{
    public class SearchService
    {
        private readonly ResultsStorage _resultsStorage;
        private static readonly Dictionary<int, ISearcher> BrowserList = new Dictionary<int, ISearcher>
        {
            {1, new YandexSearcher()},
            {2, new GoogleSearcher()},
            {3, new BingSearcher()},
        };

        private static readonly HttpClient Client = new HttpClient();


        public SearchService(ResultsStorage resultsStorage)
        {
            _resultsStorage = resultsStorage;
        }

        public async Task<List<SearchResult>> SearchForResults(string searchString)
        {
            var (index, document) = await Search(searchString);
            var resultsList = BrowserList[index].SearchResults(document);
            await _resultsStorage.SaveResults(resultsList);
            return resultsList;
        }

        private async Task<(int index, string document)> Search(string searchString)
        {
            var tasks = BrowserList.Select(m => GetTuple(m, searchString)).ToArray();
            //var tasks = BrowserList.Select(m => get_http(m.Value.CreateLinkForSearch(searchString))).ToList();
            var firstTask = await Task.WhenAny(tasks);
            return firstTask.Result;
        }

        private async Task<(int index, string document)> GetTuple(KeyValuePair<int, ISearcher> browser, string searchString)
        {
            var res = await GetAsync(browser.Value.CreateLinkForSearch(searchString));
            if (browser.Key == 2) await Task.Delay(500);
            return (browser.Key, res);
        }
        
        public async Task<string> GetAsync(string uri)
        {
            string resultHtml;
            using (HttpResponseMessage response = await Client.GetAsync(uri))
            using (HttpContent content = response.Content)
            {
                resultHtml = await content.ReadAsStringAsync();
            }
            return resultHtml;
        }
    }
}