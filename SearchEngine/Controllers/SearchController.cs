using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchEngine.Services;

namespace SearchEngine.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService _service;
        private readonly ResultsStorage _resultsStorage;

        public SearchController(ResultsStorage resultsStorage, SearchService service)
        {
            _resultsStorage = resultsStorage;
            _service = service;
        }
        public IActionResult Index()
        {
            ViewData["actionUrl"] = Url.Action("Search", "Search");
            return View();
        }
        
        [HttpGet]
        public async Task<ActionResult> Search(string searchString)
        {
            var results = await _service.SearchForResults(searchString);
            return PartialView("SearchForResults", results);
        }
        public IActionResult FindResults()
        {
            ViewData["actionUrl"] = Url.Action("Find", "Search");
            return View();
        }
        
        [HttpGet]
        public async Task<ActionResult> Find(string searchString)
        {
            var results = await _resultsStorage.FindResults(searchString);
            ViewData["searchString"] = searchString;
            return PartialView("SearchForResults", results);
        }
        
        
    }
}