using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Searchers;
using DA.Adapters;
using DA.HttpClients;
using DA.Models;

namespace BL.Services
{
    public class SearchService
    {
        public async Task<List<SearchResult>> SearchForResults(string searchString, Dictionary<int, ISearcher> browserList)
        {
            var tasks = CreateTasksForSearchers(searchString, browserList);
            var resultFromBrowser = await Search(tasks);
            var resultsList = browserList[resultFromBrowser.BrowserIndex].SearchResults(resultFromBrowser.ResultDocument);
            return resultsList;
        }

        private IEnumerable<Task<BrowserResult>> CreateTasksForSearchers(string searchString, Dictionary<int, ISearcher> browserList)
        {
            return browserList.Select(m => GetTaskForSearcher(m, searchString));
        }

        public async Task<BrowserResult> Search(IEnumerable<Task<BrowserResult>> tasks)
        {
            var firstTask = await Task.WhenAny(tasks.ToArray());
            return firstTask.Result;
        }

        private async Task<BrowserResult> GetTaskForSearcher(KeyValuePair<int, ISearcher> searcher, string searchString)
        {
            var uri = LinkCreator.CreateLinkForSearch(searcher.Value.GetAddress(),searchString);
            var res = await HttpRequestClient.GetHttpContent(uri, HtmlDocumentAdapter.StringToHtmlDocumentConverter);
            if (searcher.Key != 1) await Task.Delay(200);
            return new BrowserResult{BrowserIndex = searcher.Key, ResultDocument = res};
        }
    }
}