namespace BillsPaymentSystem.Data.EntityConfigurations
{
    using BillsPaymentSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using System;
    using System.Collections.Generic;
    using System.Text;



    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.Property(f => f.FirstName)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            builder.Property(f => f.LastName)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            builder.Property(f => f.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Password)
                .HasMaxLength(25)
                .IsRequired();

        }
    }
}
