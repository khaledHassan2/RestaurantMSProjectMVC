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

            string? imagePath = null;

            if (item.ImageFile != null && item.ImageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.ImageFile.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await item.ImageFile.CopyToAsync(stream);
                }
                imagePath = "/images/" + fileName;
            }

            MenuItem newItem = new MenuItem()
            {
                Name = item.Name,
                PreparationTimeMinutes = item.PreparationTimeMinutes,
                Price = item.Price,
                DailyOrderCount = item.DailyOrderCount,
                ImageUrl = imagePath,
                CategoryId = item.CategoryId,
            };

            await _context.MenuItems.AddAsync(newItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            var cats = await _context.MenuCategories.ToListAsync();
            EditMenuItemVM itemVM = new()
            {
                Id=item.Id,
                Name = item.Name,
                PreparationTimeMinutes = item.PreparationTimeMinutes,
                Price = item.Price,
                DailyOrderCount = item.DailyOrderCount,
                ImageUrl = item.ImageUrl,
                CategoryId = item.CategoryId,
                Categories = new SelectList(cats, "Id", "Name")

            };
            
            return View(itemVM);
           
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditMenuItemVM item)
        {
            if (!ModelState.IsValid)
            {
                var cats = await _context.MenuCategories.ToListAsync();
                item.Categories = new SelectList(cats, "Id", "Name");
                return View(item);
            }

            var oldItem = await _context.MenuItems.FindAsync(item.Id);
            if (oldItem == null)
                return NotFound();

          
            oldItem.Name = item.Name;
            oldItem.Price = item.Price;
            oldItem.PreparationTimeMinutes = item.PreparationTimeMinutes;
            oldItem.DailyOrderCount = item.DailyOrderCount;
            oldItem.CategoryId = item.CategoryId;

            // لو المستخدم رفع صورة جديدة
            if (item.ImageFile != null && item.ImageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(item.ImageFile.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await item.ImageFile.CopyToAsync(stream);
                }

                // حذف الصورة القديمة لو موجودة
                if (!string.IsNullOrEmpty(oldItem.ImageUrl))
                {
                    string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldItem.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // حفظ المسار الجديد
                oldItem.ImageUrl = "/images/" + fileName;
            }

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
