using MessMate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Infrastructure.Configurations
{
    public class DeliveryConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OTP)
                .IsRequired()
                .HasMaxLength(6);

            builder.Property(x => x.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.Status)
                .HasConversion<int>();

            builder.HasOne(x => x.Order)
                .WithOne(o => o.Delivery)
                .HasForeignKey<Delivery>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Staff)
                .WithMany()
                .HasForeignKey(x => x.StaffId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Deliveries");
        }
    }
}
