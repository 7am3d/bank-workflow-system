using BankWorkflow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BankWorkflow.API.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        // Seed Departments
        if (!await context.Departments.AnyAsync())
            await context.Departments.AddRangeAsync(SeedData.Departments);

        // Seed Roles
        if (!await context.Roles.AnyAsync())
            await context.Roles.AddRangeAsync(SeedData.Roles);

        // Seed Request Types
        foreach (var seedRequestType in SeedData.RequestTypes)
        {
            var exists = await context.RequestTypes
                .AnyAsync(r => r.Name == seedRequestType.Name);

            if (!exists)
            {
                await context.RequestTypes.AddAsync(seedRequestType);
            }
        }

        await context.SaveChangesAsync();

        // Seed Admin user
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
                CreatedAt = DateTime.UtcNow,
                RoleId = adminRole.Id,
                DepartmentId = itDepartment.Id
            };

            await context.Users.AddAsync(adminUser);
            await context.SaveChangesAsync();
        }

        // Seed dynamic workflow definitions
        await SeedWorkflowDefinitionsAsync(context);

        // Seed test Employee users
        await SeedEmployeeUsersAsync(context);
    }

    private static async Task SeedEmployeeUsersAsync(AppDbContext context)
    {
        var employeeRole = await context.Roles
            .FirstAsync(r => r.Name == "Employee");

        var operationsDepartment = await context.Departments
            .FirstAsync(d => d.Code == "OPS");

        if (!await context.Users.AnyAsync(u => u.Email == "sarah@bankworkflow.com"))
        {
            var sarah = new User
            {
                FirstName = "Sarah",
                LastName = "Employee",
                Email = "sarah@bankworkflow.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employee@123"),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                RoleId = employeeRole.Id,
                DepartmentId = operationsDepartment.Id
            };

            await context.Users.AddAsync(sarah);
        }

        if (!await context.Users.AnyAsync(u => u.Email == "michael@bankworkflow.com"))
        {
            var michael = new User
            {
                FirstName = "Michael",
                LastName = "Employee",
                Email = "michael@bankworkflow.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employee@123"),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                RoleId = employeeRole.Id,
                DepartmentId = operationsDepartment.Id
            };

            await context.Users.AddAsync(michael);
        }

        await context.SaveChangesAsync();
    }

    private static async Task SeedWorkflowDefinitionsAsync(AppDbContext context)
    {
        // Don't create them again if they already exist.

        var supervisorRole = await context.Roles
            .FirstAsync(r => r.Name == "Supervisor");

        var managerRole = await context.Roles
            .FirstAsync(r => r.Name == "Manager");

        var directorRole = await context.Roles
            .FirstAsync(r => r.Name == "Director");

        var leaveRequest = await context.RequestTypes
            .FirstAsync(r => r.Name == "Leave Request");

        var accessRequest = await context.RequestTypes
            .FirstAsync(r => r.Name == "Access Request");

        var purchaseRequest = await context.RequestTypes
            .FirstAsync(r => r.Name == "Purchase Request");

        var expenseClaim = await context.RequestTypes
            .FirstAsync(r => r.Name == "Expense Claim");

        var hardwareRequest = await context.RequestTypes
            .FirstAsync(r => r.Name == "Hardware Request");

        var softwareRequest = await context.RequestTypes
            .FirstAsync(r => r.Name == "Software Request");

        var kycVerification = await context.RequestTypes
            .FirstAsync(r => r.Name == "KYC Verification");

        var accountOpening = await context.RequestTypes
            .FirstAsync(r => r.Name == "Account Opening");

        var workflows = new List<WorkflowDefinition>
        {
            // Leave Request
            new WorkflowDefinition
            {
                Name = "Leave Request Workflow",
                Description = "Leave requests require supervisor approval.",
                RequestTypeId = leaveRequest.Id,
                IsActive = true,
                Steps =
                {
                    new WorkflowStepDefinition
                    {
                        Sequence = 1,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = supervisorRole.Id,
                        IsRequired = true
                    }
                }
            },

            // Access Request
            new WorkflowDefinition
            {
                Name = "Access Request Workflow",
                Description = "Access requests require supervisor and manager approval.",
                RequestTypeId = accessRequest.Id,
                IsActive = true,
                Steps =
                {
                    new WorkflowStepDefinition
                    {
                        Sequence = 1,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = supervisorRole.Id,
                        IsRequired = true
                    },
                    new WorkflowStepDefinition
                    {
                        Sequence = 2,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = managerRole.Id,
                        IsRequired = true
                    }
                }
            },

            // Purchase Request
            new WorkflowDefinition
            {
                Name = "Purchase Request Workflow",
                Description = "Purchase requests require supervisor, manager, and director approval.",
                RequestTypeId = purchaseRequest.Id,
                IsActive = true,
                Steps =
                {
                    new WorkflowStepDefinition
                    {
                        Sequence = 1,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = supervisorRole.Id,
                        IsRequired = true
                    },
                    new WorkflowStepDefinition
                    {
                        Sequence = 2,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = managerRole.Id,
                        IsRequired = true
                    },
                    new WorkflowStepDefinition
                    {
                        Sequence = 3,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = directorRole.Id,
                        IsRequired = true
                    }
                }
            },

            // Expense Claim
            new WorkflowDefinition
            {
                Name = "Expense Claim Workflow",
                Description = "Expense claims require supervisor and manager approval.",
                RequestTypeId = expenseClaim.Id,
                IsActive = true,
                Steps =
                {
                    new WorkflowStepDefinition
                    {
                        Sequence = 1,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = supervisorRole.Id,
                        IsRequired = true
                    },
                    new WorkflowStepDefinition
                    {
                        Sequence = 2,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = managerRole.Id,
                        IsRequired = true
                    }
                }
            },

            // Hardware Request
            new WorkflowDefinition
            {
                Name = "Hardware Request Workflow",
                Description = "Hardware requests require supervisor and manager approval.",
                RequestTypeId = hardwareRequest.Id,
                IsActive = true,
                Steps =
                {
                    new WorkflowStepDefinition
                    {
                        Sequence = 1,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = supervisorRole.Id,
                        IsRequired = true
                    },
                    new WorkflowStepDefinition
                    {
                        Sequence = 2,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = managerRole.Id,
                        IsRequired = true
                    }
                }
            },

            // Software Request
            new WorkflowDefinition
            {
                Name = "Software Request Workflow",
                Description = "Software requests require supervisor and manager approval.",
                RequestTypeId = softwareRequest.Id,
                IsActive = true,
                Steps =
                {
                    new WorkflowStepDefinition
                    {
                        Sequence = 1,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = supervisorRole.Id,
                        IsRequired = true
                    },
                    new WorkflowStepDefinition
                    {
                        Sequence = 2,
                        ApproverType = Common.WorkflowApproverType.Role,
                        RoleId = managerRole.Id,
                        IsRequired = true
                    }
                }
            }
        };

        await AddWorkflowIfMissingAsync(
            context,
            workflows[0]);

        await AddWorkflowIfMissingAsync(
            context,
            workflows[1]);

        await AddWorkflowIfMissingAsync(
            context,
            workflows[2]);

        await AddWorkflowIfMissingAsync(
            context,
            workflows[3]);

        await AddWorkflowIfMissingAsync(
            context,
            workflows[4]);

        await AddWorkflowIfMissingAsync(
            context,
            workflows[5]);

        var kycWorkflow = await context.WorkflowDefinitions
    .Include(w => w.Steps)
    .FirstOrDefaultAsync(w => w.RequestTypeId == kycVerification.Id);

        if (kycWorkflow is null)
        {
            kycWorkflow = new WorkflowDefinition
            {
                Name = "KYC Verification Workflow",
                Description = "KYC verification requires an independent employee checker.",
                RequestTypeId = kycVerification.Id,
                IsActive = true,
                Steps =
        {
            new WorkflowStepDefinition
            {
                Sequence = 1,
                ApproverType = Common.WorkflowApproverType.Employee,
                IsRequired = true
            }
        }
            };

            await context.WorkflowDefinitions.AddAsync(kycWorkflow);
        }
        else
        {
            var step = kycWorkflow.Steps
                .FirstOrDefault(s => s.Sequence == 1);

            if (step is not null)
            {
                step.ApproverType = Common.WorkflowApproverType.Employee;
                step.RoleId = null;
            }
        }

        var accountOpeningWorkflow = await context.WorkflowDefinitions
    .Include(w => w.Steps)
    .FirstOrDefaultAsync(w => w.RequestTypeId == accountOpening.Id);

        if (accountOpeningWorkflow is null)
        {
            accountOpeningWorkflow = new WorkflowDefinition
            {
                Name = "Account Opening Workflow",
                Description = "Account opening requires an independent employee checker.",
                RequestTypeId = accountOpening.Id,
                IsActive = true,
                Steps =
        {
            new WorkflowStepDefinition
            {
                Sequence = 1,
                ApproverType = Common.WorkflowApproverType.Employee,
                IsRequired = true
            }
        }
            };

            await context.WorkflowDefinitions.AddAsync(accountOpeningWorkflow);
        }
        else
        {
            var step = accountOpeningWorkflow.Steps
                .FirstOrDefault(s => s.Sequence == 1);

            if (step is not null)
            {
                step.ApproverType = Common.WorkflowApproverType.Employee;
                step.RoleId = null;
            }
        }

        await context.SaveChangesAsync();
    }
    private static async Task AddWorkflowIfMissingAsync(
    AppDbContext context,
    WorkflowDefinition workflow)
    {
        var exists = await context.WorkflowDefinitions
            .AnyAsync(w => w.RequestTypeId == workflow.RequestTypeId);

        if (!exists)
        {
            await context.WorkflowDefinitions.AddAsync(workflow);
        }
    }
}