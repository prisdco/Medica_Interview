using MediatR;
using Medica_Interview.Infrastructure.Entities;
using Medica_Interview.Infrastructure.Interface;
using Medica_Interview.Infrastructure.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Application.UseCase.Queries.DatabaseSource
{
    public class DbEmployeesQuery : IRequest<Result<IEnumerable<Employee>>>
    {
        public class DbEmployeesQueryHandler : IRequestHandler<DbEmployeesQuery, Result<IEnumerable<Employee>>>
        {
            private readonly IRepository<Employee> _repository;
            public DbEmployeesQueryHandler(IRepository<Employee> repository)
            {
                _repository = repository;
            }

            public async Task<Result<IEnumerable<Employee>>> Handle(DbEmployeesQuery request, CancellationToken cancellationToken)
            {

                var employees = await _repository.GetAll();
                List<Employee> allEmployees = new List<Employee>();
                foreach (var employe in employees)
                {
                    Employee allEmployee = new();
                    allEmployee.ProfilePicture = employe.ProfilePicture;
                    allEmployee.Telephone = employe.Telephone;
                    allEmployee.Team = employe.Team;
                    allEmployee.Address1 = employe.Address1;
                    allEmployee.Address2 = employe.Address2;
                    allEmployee.DateOfBirth = employe.DateOfBirth;
                    allEmployee.Firstname = employe.Firstname;
                    allEmployee.Lastname = employe.Lastname;
                    allEmployee.Email = employe.Email;
                    allEmployee.StartDate = employe.StartDate;
                    allEmployee.County = employe.County;
                    allEmployee.JobTitle = employe.JobTitle;
                    allEmployee.LineManager = employe.LineManager;
                    allEmployee.Postcode = employe.Postcode;
                    allEmployee.Town = employe.Town;
                    allEmployees.Add(allEmployee);
                }
                var results = from x in allEmployees select x;
                return ResultViewModel.Ok(results);
            }
        }

    }
}
