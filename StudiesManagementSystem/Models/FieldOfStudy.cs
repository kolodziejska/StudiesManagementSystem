using System;
using System.Collections.Generic;

#nullable disable

namespace StudiesManagementSystem.Models
{
    public partial class FieldOfStudy
    {
        public FieldOfStudy()
        {
            Classes = new HashSet<Class>();
            FosStudents = new HashSet<FosStudent>();
            Professors = new HashSet<Professor>();
        }

        public int FosId { get; set; }
        public string FosName { get; set; }
        public int? FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<FosStudent> FosStudents { get; set; }
        public virtual ICollection<Professor> Professors { get; set; }
    }
}
