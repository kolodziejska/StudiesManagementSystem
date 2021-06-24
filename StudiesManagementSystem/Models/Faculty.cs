using System;
using System.Collections.Generic;

#nullable disable

namespace StudiesManagementSystem.Models
{
    public partial class Faculty
    {
        public Faculty()
        {
            FieldOfStudies = new HashSet<FieldOfStudy>();
        }

        public int FacultyId { get; set; }
        public string FacultyName { get; set; }

        public virtual ICollection<FieldOfStudy> FieldOfStudies { get; set; }
    }
}
