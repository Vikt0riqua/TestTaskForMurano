using System.Collections.Generic;
using System.Threading.Tasks;
using DA.Contexts;
using DA.Models;
using DA.Repositories;

namespace BL.Services
{
    public class ResultsStorageService
    {
        private readonly SearchResultRepository _searchResultRepository;

        public ResultsStorageService(SearchResultRepository searchResultRepository)
        {
            _searchResultRepository = searchResultRepository;
        }
        
        public async Task SaveResults(IEnumerable<SearchResult> results)
        {
            await _searchResultRepository.SaveSearchResults(results);
        }

        public async Task<List<SearchResult>> FindResults(string searchString)
        {
            return await _searchResultRepository.FindResultsFromPastSearches(searchString);
        }
    }
}