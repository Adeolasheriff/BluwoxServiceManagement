using BluwoxServiceManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BluwoxServiceManagement.Infrastructure.Data.Configurations;

public class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
{
    public void Configure(EntityTypeBuilder<ServiceCategory> builder)
    {
        builder.ToTable("ServiceCategories");

        builder.HasKey(sc => new { sc.ServiceId, sc.CategoryId });

        builder.HasOne(sc => sc.Service)
            .WithMany(s => s.ServiceCategories)
            .HasForeignKey(sc => sc.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sc => sc.Category)
            .WithMany(c => c.ServiceCategories)
            .HasForeignKey(sc => sc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(sc => sc.AssignedDate)
            .IsRequired();
    }
}