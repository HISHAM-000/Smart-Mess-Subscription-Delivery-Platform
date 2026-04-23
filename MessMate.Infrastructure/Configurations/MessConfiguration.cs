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
    public class MessConfiguration : IEntityTypeConfiguration<Mess>
    {
        public void Configure(EntityTypeBuilder<Mess> builder)
        {
            builder.HasKey(m => m.Id);

            builder.HasOne(m => m.Owner)
                .WithMany()
                .HasForeignKey(m => m.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
