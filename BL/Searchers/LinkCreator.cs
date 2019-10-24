using System.Web;

namespace BL.Searchers
{
    public static class LinkCreator
    {
        public static string CreateLinkForSearch(string address, string searchString)
        {
            return address + HttpUtility.UrlEncode(searchString.Trim());
        }
    }
}