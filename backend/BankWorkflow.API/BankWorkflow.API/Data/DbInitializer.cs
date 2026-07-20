using BankWorkflow.API.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace BankWorkflow.API.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Departments.AnyAsync())
            await context.Departments.AddRangeAsync(SeedData.Departments);

        if (!await context.Roles.AnyAsync())
            await context.Roles.AddRangeAsync(SeedData.Roles);

        if (!await context.RequestTypes.AnyAsync())
            await context.RequestTypes.AddRangeAsync(SeedData.RequestTypes);

        await context.SaveChangesAsync();

        // Seed administrator user
        if (!await context.Users.AnyAsync())
        {
            var adminRole = await context.Roles
                .FirstAsync(r => r.Name == "Admin");

            var itDepartment = await context.Departments
                .FirstAsync(d => d.Code == "IT");

            var adminUser = new User
            {
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@bankworkflow.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                IsActive = true,
                RoleId = adminRole.Id,
                DepartmentId = itDepartment.Id
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }
    }
}