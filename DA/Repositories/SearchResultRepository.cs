using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DA.Contexts;
using DA.Models;
using Microsoft.EntityFrameworkCore;

namespace DA.Repositories
{
    public class SearchResultRepository
    {
        private readonly SearchResultContext _searchResultContext;

        public SearchResultRepository(SearchResultContext context)
        {
            _searchResultContext = context;
        }
        
        public async Task SaveSearchResults(IEnumerable<SearchResult> results)
        {
            foreach (var searchResult in results)
            {
                _searchResultContext.SearchResults.Add(searchResult);
                await _searchResultContext.SaveChangesAsync();
            }
        }
        
        public async Task<List<SearchResult>> FindResultsFromPastSearches(string searchString)
        {
            var resultsList = 
                await _searchResultContext.SearchResults.Where(p => EF.Functions.Like(p.Header.ToLower(),"%" + searchString.ToLower() + "%") ||
                                                                    EF.Functions.Like(p.ResultText.ToLower(),"%" + searchString.ToLower() + "%"))
                    .ToListAsync();
            return resultsList.ToList();
        }
    }
}