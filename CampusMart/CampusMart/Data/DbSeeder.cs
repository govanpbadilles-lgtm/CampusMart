using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CampusMart.Models.Entities;
using CampusMart.Data;

namespace CampusMart.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var db = serviceProvider.GetRequiredService<AppDbContext>();

            // 1. Ensure Roles exist (Admin + User only, no Seller)
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. Seed Admin Accounts
            var admins = new[]
            {
                new { Id = "23769862", Pass = "Marecigan@0912", Email = "admin1@campusmart.local", Name = "Head Administrator" },
                new { Id = "23769863", Pass = "Marecigan@0913", Email = "admin2@campusmart.local", Name = "Secondary Admin" }
            };

            foreach (var adm in admins)
            {
                var adminUser = await userManager.FindByNameAsync(adm.Id);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adm.Id,
                        Email = adm.Email,
                        FullName = adm.Name,
                        StudentId = adm.Id,
                        EmailConfirmed = true,
                        DateTime = DateTime.UtcNow,
                        Department = "Administration",
                        YearLevel = "N/A"
                    };

                    var result = await userManager.CreateAsync(adminUser, adm.Pass);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                        await userManager.AddToRoleAsync(adminUser, "User");
                    }
                }
                else
                {
                    if (!await userManager.IsInRoleAsync(adminUser, "Admin")) await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (!await userManager.IsInRoleAsync(adminUser, "User")) await userManager.AddToRoleAsync(adminUser, "User");
                }
            }

            // 3. Seed a Default Student User
            string studentId = "2021-0001";
            var studentUser = await userManager.FindByNameAsync(studentId);
            if (studentUser == null)
            {
                studentUser = new ApplicationUser
                {
                    UserName = studentId,
                    Email = "student@campusmart.local",
                    FullName = "Sample Student",
                    StudentId = studentId,
                    EmailConfirmed = true,
                    DateTime = DateTime.UtcNow,
                    Department = "Computer Science",
                    YearLevel = "3rd Year"
                };

                var result = await userManager.CreateAsync(studentUser, "Student123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(studentUser, "User");
                }
            }

            // 4. Seed Floors
            if (!await db.Floors.AnyAsync())
            {
                db.Floors.AddRange(
                    new Floor { Name = "Ground Floor Lobby", FloorNumber = 1, Capacity = 10, Building = "Main Building", Description = "Main entrance area with food and supply stalls" },
                    new Floor { Name = "Second Floor Hallway", FloorNumber = 2, Capacity = 8, Building = "Main Building", Description = "Academic supplies and tech accessories" }
                );
                await db.SaveChangesAsync();
            }

            // 5. Seed Sample Stalls
            if (!await db.Stalls.AnyAsync())
            {
                var firstFloor = await db.Floors.FirstOrDefaultAsync(f => f.FloorNumber == 1);
                var secondFloor = await db.Floors.FirstOrDefaultAsync(f => f.FloorNumber == 2);
                if (firstFloor != null)
                {
                    db.Stalls.AddRange(
                        new Stall 
                        { 
                            Name = "The Book Nook", 
                            StallNumber = "G-01", 
                            OwnerName = "Admin", 
                            Category = "Books", 
                            Description = "Official campus bookstore satellite stall.",
                            FloorId = firstFloor.Id,
                            OpenTime = "08:00",
                            CloseTime = "17:00"
                        },
                        new Stall 
                        { 
                            Name = "Tech Stop", 
                            StallNumber = "G-02", 
                            OwnerName = "Admin", 
                            Category = "Electronics", 
                            Description = "Quick repairs and accessories.",
                            FloorId = firstFloor.Id,
                            OpenTime = "09:00",
                            CloseTime = "18:00"
                        },
                        new Stall
                        {
                            Name = "Campus Bites",
                            StallNumber = "G-03",
                            OwnerName = "Admin",
                            Category = "Food",
                            Description = "Snacks, drinks, and ready meals for students.",
                            FloorId = firstFloor.Id,
                            OpenTime = "07:00",
                            CloseTime = "19:00"
                        }
                    );
                    await db.SaveChangesAsync();
                }

                if (secondFloor != null)
                {
                    db.Stalls.AddRange(
                        new Stall
                        {
                            Name = "Stationery Hub",
                            StallNumber = "2-01",
                            OwnerName = "Admin",
                            Category = "Supplies",
                            Description = "Pens, notebooks, and school essentials.",
                            FloorId = secondFloor.Id,
                            OpenTime = "08:00",
                            CloseTime = "17:00"
                        }
                    );
                    await db.SaveChangesAsync();
                }
            }

            // 6. Seed Sample Products (belong to stalls, managed by admin)
            if (!await db.Products.AnyAsync())
            {
                var bookStall = await db.Stalls.FirstOrDefaultAsync(s => s.StallNumber == "G-01");
                var techStall = await db.Stalls.FirstOrDefaultAsync(s => s.StallNumber == "G-02");
                var foodStall = await db.Stalls.FirstOrDefaultAsync(s => s.StallNumber == "G-03");
                var supplyStall = await db.Stalls.FirstOrDefaultAsync(s => s.StallNumber == "2-01");

                var textbookCat = await db.Categories.FirstOrDefaultAsync(c => c.Name.Contains("Textbooks"));
                var electronicsCat = await db.Categories.FirstOrDefaultAsync(c => c.Name.Contains("Electronics"));
                var foodCat = await db.Categories.FirstOrDefaultAsync(c => c.Name.Contains("Food"));
                var stationeryCat = await db.Categories.FirstOrDefaultAsync(c => c.Name.Contains("Stationery"));

                var products = new List<Product>();

                if (bookStall != null && textbookCat != null)
                {
                    products.Add(new Product
                    {
                        Name = "Introduction to Algorithms 4th Ed",
                        Price = 1500,
                        Stock = 5,
                        Description = "Comprehensive textbook for CS students covering data structures and algorithms.",
                        CategoryId = textbookCat.Id,
                        StallId = bookStall.Id
                    });
                    products.Add(new Product
                    {
                        Name = "Physics for Engineers",
                        Price = 980,
                        Stock = 3,
                        Description = "Engineering physics textbook with practice problems.",
                        CategoryId = textbookCat.Id,
                        StallId = bookStall.Id
                    });
                }

                if (techStall != null && electronicsCat != null)
                {
                    products.Add(new Product
                    {
                        Name = "Scientific Calculator Casio fx-991ES",
                        Price = 850,
                        Stock = 10,
                        Description = "Standard engineering calculator for exams.",
                        CategoryId = electronicsCat.Id,
                        StallId = techStall.Id
                    });
                    products.Add(new Product
                    {
                        Name = "USB-C Hub Adapter",
                        Price = 450,
                        Stock = 15,
                        Description = "Multi-port USB hub for laptops.",
                        CategoryId = electronicsCat.Id,
                        StallId = techStall.Id
                    });
                }

                if (foodStall != null && foodCat != null)
                {
                    products.Add(new Product
                    {
                        Name = "Campus Meal Combo",
                        Price = 85,
                        Stock = 50,
                        Description = "Rice + viand + drink combo meal.",
                        CategoryId = foodCat.Id,
                        StallId = foodStall.Id
                    });
                }

                if (supplyStall != null && stationeryCat != null)
                {
                    products.Add(new Product
                    {
                        Name = "Engineering Notebook A4",
                        Price = 120,
                        Stock = 30,
                        Description = "Ruled engineering notebook, 200 pages.",
                        CategoryId = stationeryCat.Id,
                        StallId = supplyStall.Id
                    });
                }

                if (products.Any())
                {
                    db.Products.AddRange(products);
                    await db.SaveChangesAsync();
                }
            }

            // 7. Seed a Sample Order
            var headAdmin = await userManager.FindByNameAsync("23769862");
            if (headAdmin != null && !await db.Orders.AnyAsync(o => o.UserId == headAdmin.Id))
            {
                var product = await db.Products.FirstOrDefaultAsync();
                if (product != null)
                {
                    var order = new Order
                    {
                        UserId = headAdmin.Id,
                        Status = "Confirmed",
                        TotalAmount = product.Price,
                        ShippingAddress = "Meet Up — Main Lobby",
                        PaymentMethod = "Cash",
                        CreatedAt = DateTime.UtcNow.AddDays(-1),
                        OrderItems = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                ProductId = product.Id,
                                Quantity = 1,
                                Price = product.Price
                            }
                        }
                    };
                    db.Orders.Add(order);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
