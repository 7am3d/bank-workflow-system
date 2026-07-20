using BankWorkflow.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankWorkflow.API.Configurations;

public class WorkflowCommentConfiguration : IEntityTypeConfiguration<WorkflowComment>
{
    public void Configure(EntityTypeBuilder<WorkflowComment> builder)
    {
        builder.ToTable("WorkflowComments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Comment)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.HasOne(c => c.WorkflowRequest)
            .WithMany(wr => wr.Comments)
            .HasForeignKey(c => c.WorkflowRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}