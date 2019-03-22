namespace BillsPaymentSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class CreditCard
    {
        public int CreditCardId { get; set; }

        public decimal Limit { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal MoneyOwed { get; set; }

        public decimal LimitLeft => this.Limit - this.MoneyOwed;

        public PaymentMethod PaymentMethod { get; set; }

    }
}
