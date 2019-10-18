using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SearchEngine.Models;

namespace SearchEngine.Services
{
    public class ResultsStorage
    {
        private readonly SearchResultContext _db;

        public ResultsStorage(SearchResultContext context)
        {
            _db = context;
        }
        public async Task SaveResults(IEnumerable<SearchResult> results)
        {
            try
            {
                foreach (var searchResult in results)
                {
                    searchResult.ResultText = searchResult.ResultText.Length > 450
                        ? searchResult.ResultText.Substring(0, 450)
                        : searchResult.ResultText;
                    _db.SearchResults.Add(searchResult);
                    await _db.SaveChangesAsync();
                }
            }
            catch
            {
                //
            }
        }

        public async Task<List<SearchResult>> FindResults(string searchString)
        {
            var resultsList = 
                await _db.SearchResults.Where(p => EF.Functions.Like(p.Header.ToLower(),"%" + searchString.ToLower() + "%") ||
                                                   EF.Functions.Like(p.ResultText.ToLower(),"%" + searchString.ToLower() + "%"))
                .ToListAsync();
            return resultsList.Take(10).ToList();
        }
    }
}