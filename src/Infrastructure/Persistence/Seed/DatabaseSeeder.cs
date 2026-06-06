using Bogus;
using danialNewsNetX.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace danialNewsNetX.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Seed Roles
        string[] roleNames = { "SuperAdmin", "Admin", "Moderator", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // Seed Super Admin
        if (await userManager.FindByEmailAsync("superadmin@danialnet.com") == null)
        {
            var superAdmin = new AppUser
            {
                UserName = "superadmin",
                Email = "superadmin@danialnet.com",
                EmailConfirmed = true,
                IsVerified = true
            };
            var result = await userManager.CreateAsync(superAdmin, "SuperAdmin@123456");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
            }
        }

        if (await context.Posts.AnyAsync()) return;

        var users = new List<AppUser>();
        var faker = new Faker("fa");

        for (int i = 0; i < 25; i++)
        {
            var user = new AppUser
            {
                UserName = faker.Internet.UserName(),
                Email = faker.Internet.Email(),
                Bio = faker.Lorem.Sentence(),
                IsVerified = i % 5 == 0,
                HexThemeColor = faker.Commerce.Color()
            };

            var result = await userManager.CreateAsync(user, "Password123!");
            if (result.Succeeded)
            {
                users.Add(user);
            }
        }

        var posts = new List<Post>();
        foreach (var user in users)
        {
            for (int j = 0; j < 5; j++)
            {
                var paragraph = faker.Lorem.Paragraph();
                var content = paragraph.Length > 280 ? paragraph.Substring(0, 280) : paragraph;
                var post = new Post
                {
                    Id = Guid.NewGuid(),
                    Content = content,
                    AuthorId = user.Id,
                    CreatedAt = DateTime.UtcNow.AddHours(-faker.Random.Int(1, 100))
                };
                posts.Add(post);
            }
        }

        context.Posts.AddRange(posts);
        await context.SaveChangesAsync();

        // Add some likes
        foreach (var post in posts.Take(50))
        {
            var like = new Like
            {
                PostId = post.Id,
                UserId = users[faker.Random.Int(0, users.Count - 1)].Id
            };
            if (!context.Likes.Any(l => l.PostId == like.PostId && l.UserId == like.UserId))
            {
                context.Likes.Add(like);
            }
        }

        // Seed System Features
        if (!await context.SystemFeatures.AnyAsync())
        {
            context.SystemFeatures.AddRange(new List<SystemFeature>
            {
                new SystemFeature { Id = Guid.NewGuid(), Key = "StoryUploadsEnabled", IsEnabled = true, Description = "Toggle story uploads globally" },
                new SystemFeature { Id = Guid.NewGuid(), Key = "MaintenanceMode", IsEnabled = false, Description = "Enable system-wide maintenance mode" },
                new SystemFeature { Id = Guid.NewGuid(), Key = "NewRegistrationsEnabled", IsEnabled = true, Description = "Toggle new user registrations" }
            });
        }

        await context.SaveChangesAsync();
    }
}
