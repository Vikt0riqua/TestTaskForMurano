using System.Collections.Generic;
using AngleSharp;
using AngleSharp.Dom;
using SearchEngine.Models;

namespace SearchEngine.Searchers
{
    public interface ISearcher
    {
        string CreateLinkForSearch(string searchString);
        
        List<SearchResult> SearchResults(IDocument resultFromSearcher);

    }
}