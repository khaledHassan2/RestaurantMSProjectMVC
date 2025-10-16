using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RestaurantMS.Models
{
    public class MenuCategory:ModelBase
    {
        [Required, StringLength(100)]
        [Remote(action: "IsUniqueName", controller: "MenuCategory")]
        public string Name { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
