using HtmlAgilityPack;

namespace DA.Adapters
{
    public static class HtmlDocumentAdapter
    {
        public static HtmlDocument StringToHtmlDocumentConverter(string htmlContent)
        {
            var pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(htmlContent);
            return pageDocument;
        }
    }
}