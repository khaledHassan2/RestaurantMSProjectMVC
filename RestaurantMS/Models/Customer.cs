using System.ComponentModel.DataAnnotations;

namespace RestaurantMS.Models
{
    public class Customer : ModelBase
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }

}
