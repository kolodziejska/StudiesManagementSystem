using System;
using System.Collections.Generic;

#nullable disable

namespace StudiesManagementSystem.Models
{
    public partial class Student
    {
        public Student()
        {
            FosStudents = new HashSet<FosStudent>();
            Grades = new HashSet<Grade>();
        }

        public int StudentId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Email { get; set; }

        public virtual ICollection<FosStudent> FosStudents { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
