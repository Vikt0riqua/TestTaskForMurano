using System.Collections.Generic;

namespace DA.Models
{
    public class ResultsViewModel
    {
        public IEnumerable<SearchResult> SearchResults { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}