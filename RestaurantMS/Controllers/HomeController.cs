using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMS.Context;
using RestaurantMS.Models;
using System.Diagnostics;

namespace RestaurantMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        //public IActionResult Index()
        //{
        
        //    var featuredItems = _context.MenuItems.ToList();

        //    return View(featuredItems);
        //}
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var items = _context.MenuItems
                                .Include(m => m.Category)
                                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(m => m.Name.Contains(searchString));
                    //||
                    //m.Category.Name.Contains(searchString));
            }

            return View(await items.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
