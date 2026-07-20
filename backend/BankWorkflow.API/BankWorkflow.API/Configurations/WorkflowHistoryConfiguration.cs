using BankWorkflow.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankWorkflow.API.Configurations;

public class WorkflowHistoryConfiguration : IEntityTypeConfiguration<WorkflowHistory>
{
    public void Configure(EntityTypeBuilder<WorkflowHistory> builder)
    {
        builder.ToTable("WorkflowHistory");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.Action)
            .IsRequired();

        builder.Property(h => h.PreviousStatus)
            .IsRequired();

        builder.Property(h => h.NewStatus)
            .IsRequired();

        builder.Property(h => h.CreatedAt)
            .IsRequired();

        builder.HasOne(h => h.WorkflowRequest)
            .WithMany(wr => wr.HistoryEntries)
            .HasForeignKey(h => h.WorkflowRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.User)
            .WithMany(u => u.HistoryEntries)
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}