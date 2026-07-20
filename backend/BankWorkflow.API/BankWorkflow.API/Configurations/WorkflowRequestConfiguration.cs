using BankWorkflow.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankWorkflow.API.Configurations;

public class WorkflowRequestConfiguration : IEntityTypeConfiguration<WorkflowRequest>
{
    public void Configure(EntityTypeBuilder<WorkflowRequest> builder)
    {
        builder.ToTable("WorkflowRequests");

        builder.HasKey(wr => wr.Id);

        builder.Property(wr => wr.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(wr => wr.Description)
            .HasMaxLength(2000);

        builder.Property(wr => wr.Status)
            .IsRequired();

        builder.Property(wr => wr.Priority)
            .IsRequired();

        builder.Property(wr => wr.CurrentStep)
            .IsRequired();

        builder.Property(wr => wr.CreatedAt)
            .IsRequired();

        builder.HasOne(wr => wr.CreatedByUser)
            .WithMany(u => u.CreatedRequests)
            .HasForeignKey(wr => wr.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(wr => wr.RequestType)
            .WithMany(rt => rt.WorkflowRequests)
            .HasForeignKey(wr => wr.RequestTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(wr => wr.WorkflowSteps)
            .WithOne(ws => ws.WorkflowRequest)
            .HasForeignKey(ws => ws.WorkflowRequestId);

        builder.HasMany(wr => wr.Comments)
            .WithOne(c => c.WorkflowRequest)
            .HasForeignKey(c => c.WorkflowRequestId);

        builder.HasMany(wr => wr.HistoryEntries)
            .WithOne(h => h.WorkflowRequest)
            .HasForeignKey(h => h.WorkflowRequestId);
    }
}