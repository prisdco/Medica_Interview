using MediatR;
using Medica_Interview.Infrastructure.Entities;
using Medica_Interview.Infrastructure.Interface;
using Medica_Interview.Infrastructure.ServiceModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medica_Interview.Application.UseCase.Commands
{

    public class DeleteEmployeeCommand : IRequest<ResultViewModel>
    {
        public Employee employee { get; set; }
        
        public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, ResultViewModel>
        {
            private readonly ILogger<DeleteEmployeeCommandHandler> _logger;
            private readonly IRepository<Employee> _repository;


            public DeleteEmployeeCommandHandler(ILogger<DeleteEmployeeCommandHandler> logger, IRepository<Employee> repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<ResultViewModel> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Start Program Questionaire Modification.");

                try
                {   // create program

                    var _Response = await _repository.DeleteData(request.employee);
                    if(_Response)
                        return ResultViewModel.Ok("Employee Deleted Successfully.");
                    else
                        return ResultViewModel.Fail("Employee Failed to Delete.");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    return ResultViewModel.Fail("Employee Encountered App Failure.");
                }
            }
        }
    }
}
