using System.Collections.Generic;
using DA.Models;
using HtmlAgilityPack;

namespace BL.Searchers
{
    public interface ISearcher
    {
        string GetAddress();
        List<SearchResult> SearchResults(HtmlDocument resultFromSearcher);
    }
}