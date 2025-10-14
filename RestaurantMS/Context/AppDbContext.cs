namespace RestaurantMS.Context
{
    using Microsoft.EntityFrameworkCore;
    using RestaurantMS.Models;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global filter for soft delete
            modelBuilder.Entity<MenuCategory>().HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<MenuItem>().HasQueryFilter(i => !i.IsDeleted);
            modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<OrderItem>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);

            // Data Seed
            modelBuilder.Entity<MenuCategory>().HasData(
                new MenuCategory
                {
                    Id = 1,
                    Name = "Pizza",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    UpdatedAt = null,
                    CreatedBy = "System",
                    UpdatedBy = null
                },
                new MenuCategory
                {
                    Id = 2,
                    Name = "Burgers",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    UpdatedAt = null,
                    CreatedBy = "System",
                    UpdatedBy = null
                },
                new MenuCategory
                {
                    Id = 3,
                    Name = "Drinks",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    UpdatedAt = null,
                    CreatedBy = "System",
                    UpdatedBy = null
                },
                new MenuCategory
                {
                    Id = 4,
                    Name = "Desserts",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    UpdatedAt = null,
                    CreatedBy = "System",
                    UpdatedBy = null
                }
            );

            //  Menu Items
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    Id = 1,
                    Name = "Margherita Pizza",
                    Price = 85,
                    CategoryId = 1,
                    PreparationTimeMinutes = 20,
                    ImageUrl = "/images/pizza_margherita.jpg",
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    CreatedBy = "System"
                },
                new MenuItem
                {
                    Id = 2,
                    Name = "Pepperoni Pizza",
                    Price = 95,
                    CategoryId = 1,
                    PreparationTimeMinutes = 25,
                    ImageUrl = "/images/pizza_pepperoni.jpg",
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    CreatedBy = "System"
                },
                new MenuItem
                {
                    Id = 3,
                    Name = "Cheese Burger",
                    Price = 70,
                    CategoryId = 2,
                    PreparationTimeMinutes = 15,
                    ImageUrl = "/images/cheese_burger.jpg",
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    CreatedBy = "System"
                },
                new MenuItem
                {
                    Id = 4,
                    Name = "Double Beef Burger",
                    Price = 95,
                    CategoryId = 2,
                    PreparationTimeMinutes = 18,
                    ImageUrl = "/images/double_beef_burger.jpg",
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    CreatedBy = "System"
                },
                new MenuItem
                {
                    Id = 5,
                    Name = "Cola",
                    Price = 25,
                    CategoryId = 3,
                    PreparationTimeMinutes = 2,
                    ImageUrl = "/images/cola.jpg",
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    CreatedBy = "System"
                },
                new MenuItem
                {
                    Id = 6,
                    Name = "Orange Juice",
                    Price = 30,
                    CategoryId = 3,
                    PreparationTimeMinutes = 3,
                    ImageUrl = "/images/orange_juice.jpg",
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    CreatedBy = "System"
                },
                new MenuItem
                {
                    Id = 7,
                    Name = "Chocolate Cake",
                    Price = 50,
                    CategoryId = 4,
                    PreparationTimeMinutes = 10,
                    ImageUrl = "/images/choco_cake.jpg",
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    CreatedBy = "System"
                },
                new MenuItem
                {
                    Id = 8,
                    Name = "Ice Cream Cup",
                    Price = 35,
                    CategoryId = 4,
                    PreparationTimeMinutes = 5,
                    ImageUrl = "/images/ice_cream.jpg",
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    IsDeleted = false,
                    CreatedAt = new DateTime(2025, 10, 1),
                    CreatedBy = "System"
                }
            );
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<ModelBase>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = null;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
