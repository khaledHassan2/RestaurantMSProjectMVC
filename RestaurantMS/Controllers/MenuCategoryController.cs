using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMS.Context;
using RestaurantMS.Models;
using System.Threading.Tasks;

namespace RestaurantMS.Controllers
{
    public class MenuCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public MenuCategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var cat = await _context.MenuCategories.ToListAsync();
            return View(cat);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MenuCategory menu)
        {
            if (!ModelState.IsValid)
            {
            return View();
            }
            var cats =await _context.MenuCategories.ToListAsync();
            foreach (var c in cats) 
            {
                if (c.Name == menu.Name)
                {
                    return View();
                }
                
            }
                await _context.MenuCategories.AddAsync(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            MenuCategory cat =await  _context.MenuCategories.FindAsync(id);
            return View(cat);
        }
        [HttpPost]
        public async Task<IActionResult> Update(MenuCategory menu)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var cat = await _context.MenuCategories.FindAsync(menu.Id);
            cat.Name = menu.Name;
            cat.IsActive=menu.IsActive;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _context.MenuCategories
                .Include(c => c.MenuItems)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cat == null)
                return NotFound("Category not found");

            // لو فيه عناصر تابعة للفئة
            if (cat.MenuItems != null && cat.MenuItems.Any())
            {
                ViewBag.ErrorMessage = $"Cannot delete category '{cat.Name}' because it has {cat.MenuItems.Count} menu items.";
                var cats = await _context.MenuCategories
                    .Where(c => !c.IsDeleted)
                    .ToListAsync();
                return View("Index", cats);
            }

            cat.IsDeleted = true;
            await _context.SaveChangesAsync();

            ViewBag.SuccessMessage = $"Category '{cat.Name}' deleted successfully.";
            var categories = await _context.MenuCategories
                .Where(c => !c.IsDeleted)
                .ToListAsync();
            return View("Index", categories);
        }

    }
}
