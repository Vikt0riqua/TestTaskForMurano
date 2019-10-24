using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Searchers;
using DA.Adapters;
using DA.HttpClients;
using DA.Models;
using HtmlAgilityPack;

namespace BL.Services
{
    public class SearchService
    {
        public async Task<List<SearchResult>> SearchForResults(string searchString, Dictionary<int, ISearcher> browserList)
        {
            var (index, document) = await Search(searchString, browserList);
            var resultsList = browserList[index].SearchResults(document);
            return resultsList;
        }

        private async Task<(int index, HtmlDocument document)> Search(string searchString, Dictionary<int, ISearcher> browserList)
        {
            var tasks = browserList.Select(m => GetTuple(m, searchString)).ToArray();
            var firstTask = await Task.WhenAny(tasks);
            return firstTask.Result;
        }

        private async Task<(int index, HtmlDocument document)> GetTuple(KeyValuePair<int, ISearcher> browser, string searchString)
        {
            var uri = LinkCreator.CreateLinkForSearch(browser.Value.GetAddress(),searchString);
            var res = await HttpRequestClient.GetHttpContent(uri, HtmlDocumentAdapter.StringToHtmlDocumentConverter);// GetAsync(uri);
            if (browser.Key >= 2) await Task.Delay(500);
            return (browser.Key, res);
        }
    }
}