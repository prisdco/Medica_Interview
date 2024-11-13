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

    public class UpdateEmployeeCommand : IRequest<ResultViewModel>
    {
        public Employee employee { get; set; }

        public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, ResultViewModel>
        {
            private readonly ILogger<UpdateEmployeeCommandHandler> _logger;
            private readonly IRepository<Employee> _repository;

            public UpdateEmployeeCommandHandler(ILogger<UpdateEmployeeCommandHandler> logger, IRepository<Employee> repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<ResultViewModel> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Start Employee Submission.");

                try
                {   // create program

                    var _Response = await _repository.UpdateData(request.employee);
                    if(_Response.Email != null && _Response.Email != string.Empty)
                        return ResultViewModel.Ok(_Response);
                    else
                        return ResultViewModel.Fail("Employee Data Failed To Submit.");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.Message);
                    return ResultViewModel.Fail("Employee Data Failed To Submit.");
                }
            }
        }
    }
}
