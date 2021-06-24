using System;
using System.Collections.Generic;

#nullable disable

namespace StudiesManagementSystem.Models
{
    public partial class Class
    {
        public Class()
        {
            Grades = new HashSet<Grade>();
        }

        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int? ProfId { get; set; }
        public int? FosId { get; set; }
        public int? SemesterId { get; set; }

        public virtual FieldOfStudy Fos { get; set; }
        public virtual Professor Prof { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
    }
}
