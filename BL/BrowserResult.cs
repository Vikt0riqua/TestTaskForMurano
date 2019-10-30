using HtmlAgilityPack;

namespace BL
{
    public class BrowserResult
    {
        public int BrowserIndex { get; set; }
        public HtmlDocument ResultDocument { get; set; }
    }
}