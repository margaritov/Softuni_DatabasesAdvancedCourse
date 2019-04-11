namespace SoftJail.DataProcessor
{

    using Data;
    using System;

    public class Bonus
    {
        public static string ReleasePrisoner(SoftJailDbContext context, int prisonerId)
        {
            var prisoner = context.Prisoners.Find(prisonerId);

            string result = "";

            if (prisoner.ReleaseDate != null)
            {
                prisoner.ReleaseDate = DateTime.Now;
                prisoner.CellId = null;

                context.Prisoners.Update(prisoner);

                context.SaveChanges();

                result = $"Prisoner {prisoner.FullName} released";
            }
            else
            {
                result = $"Prisoner {prisoner.FullName} is sentenced to life";
            }

            return result;
        }
    }
}
