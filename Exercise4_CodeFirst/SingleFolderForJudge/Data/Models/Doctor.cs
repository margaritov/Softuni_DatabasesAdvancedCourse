namespace P01_HospitalDatabase.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Doctor
    {
        public int DoctorId { get; set; }

        public string Name { get; set; }

        public string Specialty { get; set; }

        public ICollection<Visitation> Visitations { get; set; }

        public string Email { get; set; }
    }
}
