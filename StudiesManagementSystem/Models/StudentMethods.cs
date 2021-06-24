using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudiesManagementSystem.Models
{
    public partial class Student
    {
        public override string ToString()
        {
            var cultureInfo = new CultureInfo("pl-PL");
                        
            return (this.LastName + " " + this.FirstName +
                " birthdate: " + this.Birthdate.ToString("d", cultureInfo) + " Address: " + this.Address + " " + this.City +
                " Email: " + this.Email);
        }

        

    }
}
