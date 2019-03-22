namespace BillsPaymentSystem.Data.EntityConfigurations
{
    using BillsPaymentSystem.Models;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class CreditCardConfig : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<CreditCard> builder)
        {
            throw new NotImplementedException();
        }
    }
}
