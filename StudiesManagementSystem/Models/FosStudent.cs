using System;
using System.Collections.Generic;

#nullable disable

namespace StudiesManagementSystem.Models
{
    public partial class FosStudent
    {
        public int StudentStatus { get; set; }
        public int FosId { get; set; }
        public int SemesterId { get; set; }
        public int StudentId { get; set; }

        public virtual FieldOfStudy Fos { get; set; }
        public virtual Semester Semester { get; set; }
        public virtual Student Student { get; set; }

    }
}
