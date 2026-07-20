using BankWorkflow.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankWorkflow.API.Configurations;

public class WorkflowStepConfiguration : IEntityTypeConfiguration<WorkflowStep>
{
    public void Configure(EntityTypeBuilder<WorkflowStep> builder)
    {
        builder.ToTable("WorkflowSteps");

        builder.HasKey(ws => ws.Id);

        builder.Property(ws => ws.StepNumber)
            .IsRequired();

        builder.Property(ws => ws.Status)
            .IsRequired();

        builder.Property(ws => ws.ApprovedAt);

        builder.HasOne(ws => ws.WorkflowRequest)
            .WithMany(wr => wr.WorkflowSteps)
            .HasForeignKey(ws => ws.WorkflowRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ws => ws.Role)
            .WithMany()
            .HasForeignKey(ws => ws.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ws => ws.ApproverUser)
            .WithMany(u => u.AssignedWorkflowSteps)
            .HasForeignKey(ws => ws.ApproverUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}