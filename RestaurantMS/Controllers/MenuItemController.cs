using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantMS.Context;
using RestaurantMS.Models;
using RestaurantMS.ViewModels;
using System.Threading.Tasks;

namespace RestaurantMS.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly AppDbContext _context;

        public MenuItemController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var items =await _context.MenuItems.Include(m=>m.Category).ToListAsync();

            return View(items);
        }
        public async Task<IActionResult> New()
        {
            var cats=await _context.MenuCategories.ToListAsync();
            CreatMenuItemVM creatMenuItemVM = new CreatMenuItemVM();
            creatMenuItemVM.Categories = new SelectList(cats, "Id", "Name");
            return View(creatMenuItemVM);
        }
        [HttpPost]
        public async Task<IActionResult> New(CreatMenuItemVM item)
        {
            if (!ModelState.IsValid)
            {
                var cats = await _context.MenuCategories.ToListAsync();
                item.Categories = new SelectList(cats, "Id", "Name");
                return View(item);
                
            }
            MenuItem newItem = new MenuItem()
            {
                Name = item.Name,
                PreparationTimeMinutes = item.PreparationTimeMinutes,
                Price = item.Price,
                DailyOrderCount = item.DailyOrderCount,
                ImageUrl = item.ImageUrl,
                CategoryId = item.CategoryId,
            };
            await _context.MenuItems.AddAsync(newItem);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id) 
        {
            MenuItem item =await _context.MenuItems.FirstAsync(x => x.Id == id);
            if (item != null) 
            {
                item.IsDeleted = true;
                _context.Update(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            MenuItem item =await _context.MenuItems.FirstAsync(x => x.Id == id);
            if (item != null)
            {
                return View(item);
            }
            return RedirectToAction("Index");
        }

    }
}
