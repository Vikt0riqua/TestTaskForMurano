using System.Collections.Generic;
using BL.Searchers;

namespace BL
{
    public class BrowserListStorage
    {
        public static readonly Dictionary<int, ISearcher> BrowserList = new Dictionary<int, ISearcher>
        {
            {1, new YandexSearcher()},
            {2, new GoogleSearcher()},
            {3, new BingSearcher()},
        };
    }
}