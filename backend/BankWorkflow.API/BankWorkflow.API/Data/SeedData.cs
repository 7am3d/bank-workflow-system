using BankWorkflow.API.Models;

namespace BankWorkflow.API.Data;

public static class SeedData
{
    public static readonly List<Department> Departments =
    [
        new() { Name = "Information Technology", Code = "IT" },
        new() { Name = "Human Resources", Code = "HR" },
        new() { Name = "Finance", Code = "FIN" },
        new() { Name = "Operations", Code = "OPS" },
        new() { Name = "Risk Management", Code = "RISK" },
        new() { Name = "Compliance", Code = "COMP" }
    ];

    public static readonly List<Role> Roles =
    [
        new() { Name = "Employee" },
        new() { Name = "Supervisor" },
        new() { Name = "Manager" },
        new() { Name = "Director" },
        new() { Name = "Admin" }
    ];

    public static readonly List<RequestType> RequestTypes =
    [
        new() { Name = "Leave Request" },
        new() { Name = "Access Request" },
        new() { Name = "Purchase Request" },
        new() { Name = "Expense Claim" },
        new() { Name = "Hardware Request" },
        new() { Name = "Software Request" }
    ];
}