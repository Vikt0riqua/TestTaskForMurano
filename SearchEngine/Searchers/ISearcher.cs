using System.Collections.Generic;
using SearchEngine.Models;

namespace SearchEngine.Searchers
{
    public interface ISearcher
    {
        string CreateLinkForSearch(string searchString);
        
        List<SearchResult> SearchResults(string resultFromSearcher);

    }
}