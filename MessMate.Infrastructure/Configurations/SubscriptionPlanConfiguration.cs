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
    public class SubscriptionPlanConfiguration : IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Price)
                .HasPrecision(10, 2);

            builder.Property(x => x.PlanType)
                .HasConversion<int>();

            builder.HasOne(x => x.Mess)
                .WithMany(m => m.SubscriptionPlans)
                .HasForeignKey(x => x.MessId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("SubscriptionPlans");
        }
    }
}
