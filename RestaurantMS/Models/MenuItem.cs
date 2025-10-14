using System.ComponentModel.DataAnnotations;

namespace RestaurantMS.Models
{
    public class MenuItem:ModelBase
    {
        [Required, StringLength(150)]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        public int PreparationTimeMinutes { get; set; } = 10;

        public int DailyOrderCount { get; set; } = 0;
        public IFormFile? ImageFile { get; set; }

        [StringLength(250)]
        public string? ImageUrl { get; set; } = "/images/default-item.jpg"; // default fallback

        // Foreign Key
        public int CategoryId { get; set; }
        public MenuCategory? Category { get; set; }
    }
}
