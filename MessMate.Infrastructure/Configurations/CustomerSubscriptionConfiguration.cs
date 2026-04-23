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
    public class CustomerSubscriptionConfiguration : IEntityTypeConfiguration<CustomerSubscription>
    {
        public void Configure(EntityTypeBuilder<CustomerSubscription> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(x => x.Status)
                .HasConversion<int>();

            builder.Property(x => x.PaymentMethod)
                .HasConversion<int>();

            builder.Property(x => x.PaymentStatus)
                .HasConversion<int>();

            builder.HasOne(x => x.Customer)
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Plan)
                .WithMany(p => p.Subscriptions)
                .HasForeignKey(x => x.PlanId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Mess)
                .WithMany()
                .HasForeignKey(x => x.MessId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("CustomerSubscriptions");
        }
    }
}
