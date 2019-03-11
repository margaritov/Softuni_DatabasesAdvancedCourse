namespace P01_HospitalDatabase
{
    using P01_HospitalDatabase.Data;
    using System;

    public class StartUp
    {
        public static void Main()
        {
            
            //  db.Database.EnsureCreated();
            using (var db = new HospitalContext())
            {
                ;
            }
        }
    }
}
