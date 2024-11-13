using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Infrastructure.Entities
{
    public class Employee
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        [Format("dd/MM/yyyy")]
        public DateOnly DateOfBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string JobTitle { get; set; }
        public string Team { get; set; }
        public string LineManager { get; set; }
        [Format("dd/MM/yyyy")]
        public DateOnly StartDate { get; set; }
        public string ProfilePicture { get; set; }
    }
}
