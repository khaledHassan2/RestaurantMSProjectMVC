﻿using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantMS.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantMS.ViewModels
{
    public class CreatMenuItemVM
    {
        [Required, MaxLength(30, ErrorMessage = "Name must be 30 Character"),MinLength(3,ErrorMessage ="Name Must Be Mor than 3 Character")]
        public string Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }

        public int PreparationTimeMinutes { get; set; } = 10;

        public int DailyOrderCount { get; set; } = 0;
        public IFormFile? ImageFile { get; set; }
        [StringLength(250)]
        public string? ImageUrl { get; set; } = "/images/default-item.jpg"; // default fallback

        // Foreign Key
        [Required(ErrorMessage ="Category Id Is Required")]
        public int CategoryId { get; set; }
        public SelectList? Categories { get; set; } = null!;
    }
}
