using System;
using System.Collections.Generic;

#nullable disable

namespace StudiesManagementSystem.Models
{
    public partial class Semester
    {
        public Semester()
        {
            Classes = new HashSet<Class>();
            FosStudents = new HashSet<FosStudent>();
        }

        public int SemesterId { get; set; }
        public string SemesterName { get; set; }

        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<FosStudent> FosStudents { get; set; }
    }
}
