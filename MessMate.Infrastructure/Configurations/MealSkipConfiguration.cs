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
    public class MealSkipConfiguration : IEntityTypeConfiguration<MealSkip>
    {
        public void Configure(EntityTypeBuilder<MealSkip> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.MealSlot)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.RefundAmount)
                .HasPrecision(10, 2);

            builder.Property(x => x.RefundStatus)
                .HasConversion<int>();

            builder.HasOne(x => x.Order)
                .WithOne(o => o.MealSkip)
                .HasForeignKey<MealSkip>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Subscription)
                .WithMany(s => s.MealSkips)
                .HasForeignKey(x => x.SubscriptionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("MealSkips");
        }
    }
}
