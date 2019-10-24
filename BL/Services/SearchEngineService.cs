using System.Collections.Generic;
using System.Threading.Tasks;
using BL.Searchers;
using DA.Models;

namespace BL.Services
{
    public class SearchEngineService
    {
        private readonly SearchService _searchService;
        private readonly ResultsStorageService _resultsStorage;

        public SearchEngineService(SearchService searchService, ResultsStorageService resultsStorage)
        {
            _searchService = searchService;
            _resultsStorage = resultsStorage;
        }

        public async Task<List<SearchResult>> Execute(string searchString)
        {
            var browserList = BrowserListStorage.BrowserList;
            var searchResults =await _searchService.SearchForResults(searchString, browserList);
            await _resultsStorage.SaveResults(searchResults);
            return searchResults;
        }
        
        public async Task<List<SearchResult>> ExecuteLocalSearch(string searchString)
        {
            return await _resultsStorage.FindResults(searchString);
        }
    }
}