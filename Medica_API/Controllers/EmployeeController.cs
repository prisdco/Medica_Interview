using Asp.Versioning;
using MediatR;
using Medica_Interview.Application.UseCase.Commands;
using Medica_Interview.Application.UseCase.Queries.CsvFileData;
using Medica_Interview.Application.UseCase.Queries.DatabaseSource;
using Medica_Interview.Infrastructure.Entities;
using Medica_Interview.Infrastructure.ServiceModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Medica_API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class EmployeeController : BaseController
    {
        public EmployeeController(IMediator mediator, ILogger<EmployeeController> logger, IConfiguration config)
        : base(mediator, logger, config)
        {
        }
        /// <summary>
        /// This endpoint is used to retrieve a list of all employee profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///  GET /fetch-all 
        ///  {
        ///  }
        /// </remarks>
        ///
        [AllowAnonymous]
        [HttpGet("all-employees")]
        [ProducesResponseType(typeof(Result<IEnumerable<Employee>>), 200)]
        [ProducesResponseType(typeof(Result<string>), 400)]
        [ProducesResponseType(typeof(Result<string>), 500)]

        public async Task<IActionResult> GetAllAsync()
        {
            Logger.LogInformation($"Received request to retrieve all employees");
            var response = await this.Mediator.Send(new AllEmployeesQuery());

            if (!response.IsSuccess)
            {
                Logger.LogWarning("Failed to retrieve employees: {Message}", response.Error);
                return BadRequest(ResultViewModel.Fail(response.Error)); // Ensure this contains the failure message.
            }

            Logger.LogInformation($"Finished retriveing employees");
            Logger.LogInformation("====================================================================================");
            return Ok(response.Value);
        }

        /// <summary>
        /// This endpoint is used to retrieve a list of all employee from database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///  GET /fetch-all 
        ///  {
        ///  }
        /// </remarks>
        ///
        [AllowAnonymous]
        [HttpGet("employees-from-database")]
        [ProducesResponseType(typeof(ResultViewModel), 200)]
        [ProducesResponseType(typeof(Result<string>), 400)]
        [ProducesResponseType(typeof(Result<string>), 500)]

        public async Task<IActionResult> GetDatabaseEmployeeAsync()
        {
            Logger.LogInformation($"Received request to retrieve all employees");
            var response = await this.Mediator.Send(new DbEmployeesQuery());

            Logger.LogInformation($"Finished retriveing employees");
            Logger.LogInformation("====================================================================================");
            return Ok(response.Value);
        }

        /// <summary>
        /// this endpoint is used to Create Employee
        /// </summary>
        /// <returns></returns>
        [HttpPost("save-employee")]
        [ProducesResponseType(typeof(ResultViewModel), 200)]
        [ProducesResponseType(typeof(Result<string>), 400)]
        [ProducesResponseType(typeof(Result<string>), 500)]
        public async Task<ResultViewModel> CreateEmployeeAsync([FromBody] CreateEmployeeCommand model)
        {
            Logger.LogInformation($"Received request to create employee application");
            var result = await this.Mediator.Send(model);
            Logger.LogInformation($"Finished creating employee application");
            Logger.LogInformation("====================================================================================");
            return result;
        }

        /// <summary>
        /// this endpoint is used to Update Employee
        /// </summary>
        /// <returns></returns>
        [HttpPut("update-employee")]
        [ProducesResponseType(typeof(ResultViewModel), 200)]
        [ProducesResponseType(typeof(Result<string>), 400)]
        [ProducesResponseType(typeof(Result<string>), 500)]
        public async Task<ResultViewModel> UpdateEmployeeAsync([FromBody] UpdateEmployeeCommand model)
        {
            Logger.LogInformation($"Received request to update employee application");
            var result = await this.Mediator.Send(model);
            Logger.LogInformation($"Finished updating employee application");
            Logger.LogInformation("====================================================================================");
            return result;
        }

        /// <summary>
        /// this endpoint is used to delete Employee
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete-employee")]
        [ProducesResponseType(typeof(ResultViewModel), 200)]
        [ProducesResponseType(typeof(Result<string>), 400)]
        [ProducesResponseType(typeof(Result<string>), 500)]
        public async Task<ResultViewModel> DeleteEmployeeAsync([FromBody] DeleteEmployeeCommand model)
        {
            Logger.LogInformation($"Received request to delete employee application");
            var result = await this.Mediator.Send(model);
            Logger.LogInformation($"Finished deleting employee application");
            Logger.LogInformation("====================================================================================");
            return result;
        }
    }
}
