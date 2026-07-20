using BankWorkflow.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankWorkflow.API.Configurations;

public class RequestTypeConfiguration : IEntityTypeConfiguration<RequestType>
{
    public void Configure(EntityTypeBuilder<RequestType> builder)
    {
        builder.ToTable("RequestTypes");

        builder.HasKey(rt => rt.Id);

        builder.Property(rt => rt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(rt => rt.Description)
            .HasMaxLength(250);

        builder.Property(rt => rt.IsActive)
            .IsRequired();

        builder.Property(rt => rt.CreatedAt)
            .IsRequired();

        builder.HasIndex(rt => rt.Name)
            .IsUnique();

        builder.HasMany(rt => rt.WorkflowRequests)
            .WithOne(wr => wr.RequestType)
            .HasForeignKey(wr => wr.RequestTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}