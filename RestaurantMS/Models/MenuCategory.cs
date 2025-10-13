using System.ComponentModel.DataAnnotations;

namespace RestaurantMS.Models
{
    public class MenuCategory:ModelBase
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
