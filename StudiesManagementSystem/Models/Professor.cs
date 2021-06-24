using System;
using System.Collections.Generic;

#nullable disable

namespace StudiesManagementSystem.Models
{
    public partial class Professor
    {
        public Professor()
        {
            Classes = new HashSet<Class>();
        }

        public int ProfId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string AcademicDegree { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int? FosId { get; set; }

        public virtual FieldOfStudy Fos { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
    }
}
