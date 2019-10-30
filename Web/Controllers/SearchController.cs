using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Services;
using DA.Models;
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
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Search(string searchString, int page = 1)
        {
            ViewData["searchString"] = searchString;
            ViewData["actionString"] = "Search";
            const int pageSize = 10;
            var findResults = await _searchEngineService.Execute(searchString);
            if (findResults == null) return PartialView("SearchForResults", null);
            var pageViewModel = new PageViewModel(findResults.Count, page, pageSize);
            var viewModel = new ResultsViewModel
            {
                PageViewModel = pageViewModel,
                SearchResults = findResults.Skip((page - 1) * pageSize).Take(pageSize)
            };
            return PartialView("SearchForResults", viewModel);
        }
        public IActionResult FindResults()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Find(string searchString, int page = 1)
        {
            ViewData["searchString"] = searchString;
            ViewData["actionString"] = "Find";
            const int pageSize = 10;
            var findResults = await _searchEngineService.ExecuteLocalSearch(searchString);
            if(findResults == null) return PartialView("SearchForResults", null);
            var pageViewModel = new PageViewModel(findResults.Count, page, pageSize);
            var viewModel = new ResultsViewModel
            {
                PageViewModel = pageViewModel,
                SearchResults = findResults.Skip((page - 1) * pageSize).Take(pageSize)
            };
            return PartialView("SearchForResults", viewModel);
        }
    }
}