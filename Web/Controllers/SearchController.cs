using System.Threading.Tasks;
using BL.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchEngineService _searchEngineService;

        public SearchController(SearchEngineService searchEngineService)
        {
            _searchEngineService = searchEngineService;
        }

        public IActionResult Index()
        {
            ViewData["actionUrl"] = Url.Action("Search", "Search");
            return View();
        }
        
        [HttpGet]
        public async Task<ActionResult> Search(string searchString)
        {
            var results = await _searchEngineService.Execute(searchString);
            return PartialView("SearchForResults", results);
        }
        public IActionResult FindResults()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<ActionResult> Find(string searchString)
        {
            var results = await _searchEngineService.ExecuteLocalSearch(searchString);
            return PartialView("SearchForResults", results);
        }
        
        
    }
}