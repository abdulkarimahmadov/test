using DiscussionForum.Domain.Entities;
using DiscussionForum.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DiscussionForum.Infrastructure.Data;

/// <summary>
/// Seeds initial data for the database
/// </summary>
public static class DbSeeder
{
    public static async Task SeedDataAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        // Seed roles
        await SeedRolesAsync(roleManager);

        // Seed admin user
        var adminUser = await SeedAdminUserAsync(userManager);

        // Seed categories
        if (!await context.Categories.AnyAsync())
        {
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "General Discussion",
                    Description = "General topics and off-topic discussions",
                    Icon = "💬",
                    DisplayOrder = 1,
                    CreatedDate = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Technology",
                    Description = "Discussions about programming, software, and technology",
                    Icon = "💻",
                    DisplayOrder = 2,
                    CreatedDate = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Gaming",
                    Description = "Video games, esports, and gaming culture",
                    Icon = "🎮",
                    DisplayOrder = 3,
                    CreatedDate = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Sports",
                    Description = "Sports news, discussions, and events",
                    Icon = "⚽",
                    DisplayOrder = 4,
                    CreatedDate = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Entertainment",
                    Description = "Movies, TV shows, music, and entertainment",
                    Icon = "🎬",
                    DisplayOrder = 5,
                    CreatedDate = DateTime.UtcNow
                }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            // Seed sample topics
            await SeedSampleTopicsAsync(context, adminUser.Id, categories);
        }
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Admin", "Moderator", "Member" };
        
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }

    private static async Task<ApplicationUser> SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
    {
        var adminEmail = "admin@forum.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                DisplayName = "Administrator",
                EmailConfirmed = true,
                Role = UserRole.Admin,
                JoinedDate = DateTime.UtcNow,
                ReputationPoints = 1000,
                IsBanned = false
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        return adminUser;
    }

    private static async Task SeedSampleTopicsAsync(
        ApplicationDbContext context,
        string adminUserId,
        List<Category> categories)
    {
        var sampleTopics = new List<Topic>
        {
            new Topic
            {
                Title = "Welcome to the Discussion Forum!",
                Content = "<p>Welcome to our brand new discussion forum! This is a place where you can discuss various topics with other community members.</p><p>Please be respectful and follow the community guidelines.</p>",
                CategoryId = categories[0].Id,
                UserId = adminUserId,
                CreatedDate = DateTime.UtcNow,
                LastActivityDate = DateTime.UtcNow,
                IsPinned = true,
                Status = TopicStatus.Pinned
            },
            new Topic
            {
                Title = "What programming language should I learn in 2024?",
                Content = "<p>I'm new to programming and wondering which language I should start with. I'm interested in web development and possibly mobile apps in the future.</p><p>Any recommendations?</p>",
                CategoryId = categories[1].Id,
                UserId = adminUserId,
                CreatedDate = DateTime.UtcNow.AddHours(-5),
                LastActivityDate = DateTime.UtcNow.AddHours(-2)
            },
            new Topic
            {
                Title = "Best indie games of 2024",
                Content = "<p>What are your favorite indie games that came out this year? I'm looking for some hidden gems to play!</p>",
                CategoryId = categories[2].Id,
                UserId = adminUserId,
                CreatedDate = DateTime.UtcNow.AddHours(-3),
                LastActivityDate = DateTime.UtcNow.AddHours(-1)
            }
        };

        await context.Topics.AddRangeAsync(sampleTopics);
        await context.SaveChangesAsync();

        // Add some sample posts
        var samplePosts = new List<Post>
        {
            new Post
            {
                Content = "<p>Thank you for joining our community! Feel free to introduce yourself and start participating in discussions.</p>",
                TopicId = sampleTopics[0].Id,
                UserId = adminUserId,
                CreatedDate = DateTime.UtcNow
            },
            new Post
            {
                Content = "<p>I'd recommend starting with Python or JavaScript. Python is great for beginners and has a clean syntax. JavaScript is essential for web development.</p>",
                TopicId = sampleTopics[1].Id,
                UserId = adminUserId,
                CreatedDate = DateTime.UtcNow.AddHours(-2)
            },
            new Post
            {
                Content = "<p>Check out Hollow Knight if you haven't already! It's an amazing metroidvania with beautiful art and challenging gameplay.</p>",
                TopicId = sampleTopics[2].Id,
                UserId = adminUserId,
                CreatedDate = DateTime.UtcNow.AddHours(-1)
            }
        };

        await context.Posts.AddRangeAsync(samplePosts);
        await context.SaveChangesAsync();
    }
}
