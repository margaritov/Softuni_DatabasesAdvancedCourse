

namespace BillsPaymentSystem.App
{
    using BillsPaymentSystem.Data;
    using System;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (BillsPaymentSystemContext context = new BillsPaymentSystemContext())
            {
                DbInitializer.Seed(context);

            }

        }
    }
}
