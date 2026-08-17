using BankWorkflow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BankWorkflow.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<User> Users => Set<User>();

    public DbSet<RequestType> RequestTypes => Set<RequestType>();

    public DbSet<WorkflowRequest> WorkflowRequests => Set<WorkflowRequest>();

    public DbSet<WorkflowStep> WorkflowSteps => Set<WorkflowStep>();

    public DbSet<WorkflowComment> WorkflowComments => Set<WorkflowComment>();

    public DbSet<WorkflowAttachment> WorkflowAttachments => Set<WorkflowAttachment>();

    public DbSet<WorkflowHistory> WorkflowHistory => Set<WorkflowHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<WorkflowComment>()
            .HasOne(c => c.WorkflowRequest)
            .WithMany(r => r.Comments)
            .HasForeignKey(c => c.WorkflowRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkflowComment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkflowAttachment>()
            .HasOne(a => a.WorkflowRequest)
            .WithMany(r => r.Attachments)
            .HasForeignKey(a => a.WorkflowRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<WorkflowAttachment>()
            .HasOne(a => a.UploadedByUser)
            .WithMany(u => u.UploadedAttachments)
            .HasForeignKey(a => a.UploadedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}