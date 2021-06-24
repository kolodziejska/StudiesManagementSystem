using System;
using System.Collections.Generic;

#nullable disable

namespace StudiesManagementSystem.Models
{
    public partial class Grade
    {
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public double? GradeValue { get; set; }

        public virtual Class Class { get; set; }
        public virtual Student Student { get; set; }
    }
}
