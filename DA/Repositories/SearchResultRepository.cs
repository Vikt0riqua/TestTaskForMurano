using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DA.Contexts;
using DA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DA.Repositories
{
    public class SearchResultRepository
    {
        private readonly SearchResultContext _searchResultContext;
        private readonly ILogger<SearchResultRepository> _logger;

        public SearchResultRepository(SearchResultContext context, ILogger<SearchResultRepository> logger)
        {
            _searchResultContext = context;
            _logger = logger;
        }
        
        public async Task SaveSearchResults(IEnumerable<SearchResult> results)
        {
            foreach (var searchResult in results)
            {
                try
                {
                    _searchResultContext.SearchResults.Add(searchResult);
                    await _searchResultContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    _logger.LogWarning("Попытка добавить элемент в базу данных, который уже существует.");
                }
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