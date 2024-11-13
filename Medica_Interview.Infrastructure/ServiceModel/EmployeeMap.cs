using Medica_Interview.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Infrastructure.ServiceModel
{
    public class EmployeeMap : CsvHelper.Configuration.ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Map(m => m.Firstname).Name("Firstname");
            Map(m => m.Lastname).Name("Lastname");
            Map(m => m.Email).Name("Email");
            Map(m => m.Telephone).Name("Telephone");
            Map(m => m.DateOfBirth).Name("Date of Birth").TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.Address1).Name("Address 1");
            Map(m => m.Address2).Name("Address 2");
            Map(m => m.Town).Name("Town");
            Map(m => m.County).Name("County");
            Map(m => m.Postcode).Name("Postcode");
            Map(m => m.JobTitle).Name("Job Title");
            Map(m => m.Team).Name("Team");
            Map(m => m.LineManager).Name("Line Manager");
            Map(m => m.StartDate).Name("Start Date").TypeConverterOption.Format("dd/MM/yyyy");
            Map(m => m.ProfilePicture).Name("Profile Picture");
        }
    }
}
