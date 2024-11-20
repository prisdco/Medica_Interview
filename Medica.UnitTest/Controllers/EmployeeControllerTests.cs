using FluentAssertions;
using MediatR;
using Medica_API.Controllers;
using Medica_Interview.Application.UseCase.Commands;
using Medica_Interview.Application.UseCase.Queries.CsvFileData;
using Medica_Interview.Infrastructure.Entities;
using Medica_Interview.Infrastructure.ServiceModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Medica.UnitTest.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly Mock<ILogger<EmployeeController>> _loggerMock;
        private readonly EmployeeController _controller;

        Employee employee = new Employee
        {
            ProfilePicture = "profile.jpg",
            Telephone = "1234567890",
            Team = "Development",
            Address1 = "123 Main St",
            Address2 = "Suite 100",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Parse("1990-01-01")),
            Firstname = "John",
            Lastname = "Doe",
            Email = "prisdco@gmail.com",
            StartDate = DateOnly.FromDateTime(DateTime.Parse("2020-01-01")),
            County = "County A",
            JobTitle = "Software Engineer",
            LineManager = "Manager A",
            Postcode = "12345",
            Town = "Techville"
        };

        Employee badEmployee = new Employee
        {
            ProfilePicture = "profile.jpg",
            Telephone = "1234567890",
            Team = "Development",
            Address1 = "123 Main St",
            Address2 = "Suite 100",
            DateOfBirth = DateOnly.FromDateTime(DateTime.Parse("1990-01-01")),
            Firstname = "John",
            Lastname = "Doe",
            Email = "invalid-email", // Simulating failure with invalid email
            StartDate = DateOnly.FromDateTime(DateTime.Parse("2020-01-01")),
            County = "County A",
            JobTitle = "Software Engineer",
            LineManager = "Manager A",
            Postcode = "12345",
            Town = "Techville"
        };

        public EmployeeControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _configMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<EmployeeController>>();
            _controller = new EmployeeController(_mediatorMock.Object, _loggerMock.Object, _configMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOkResult_WhenEmployeesRetrievedSuccessfully()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee
                {
                    ProfilePicture = "profile1.jpg",
                    Telephone = "1234567890",
                    Team = "Team A",
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    DateOfBirth = DateOnly.FromDateTime(DateTime.Parse("1990-01-01")),
                    Firstname = "John",
                    Lastname = "Doe",
                    Email = "john.doe@example.com",
                    StartDate = DateOnly.FromDateTime(DateTime.Parse("2020-01-01")),
                    County = "County A",
                    JobTitle = "Developer",
                    LineManager = "Manager A",
                    Postcode = "12345",
                    Town = "Town A"
                },
                new Employee
                {
                    ProfilePicture = "profile2.jpg",
                    Telephone = "0987654321",
                    Team = "Team B",
                    Address1 = "Address B1",
                    Address2 = "Address B2",
                    DateOfBirth = DateOnly.FromDateTime(DateTime.Parse("1985-01-01")),
                    Firstname = "Jane",
                    Lastname = "Smith",
                    Email = "jane.smith@example.com",
                    StartDate = DateOnly.FromDateTime(DateTime.Parse("2019-01-01")),
                    County = "County B",
                    JobTitle = "Designer",
                    LineManager = "Manager B",
                    Postcode = "54321",
                    Town = "Town B"
                }
            };

            var mockResult = ResultViewModel.Ok(employees.AsEnumerable());

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AllEmployeesQuery>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsAssignableFrom<IEnumerable<Employee>>(okResult.Value);
            Assert.Equal(2, response.Count());
            Assert.Equal("John", response.First().Firstname);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnFail_WhenRetrievalNull()
        {
            // Arrange
            var mockErrorMessage = "Unable to fetch employee";
            var mockResult = ResultViewModel.Fail<IEnumerable<Employee>>(mockErrorMessage);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AllEmployeesQuery>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsAssignableFrom<ResultViewModel>(badRequestResult.Value);
            Assert.False(response.IsSuccess);
            Assert.Equal(mockErrorMessage, response.Error);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnInternalServerError_WhenUnhandledExceptionOccurs()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<AllEmployeesQuery>(), default))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.GetAllAsync());
            Assert.Equal("Unexpected error", exception.Message);
        }

        [Fact]
        public async Task CreateEmployeeAsync_ShouldReturnCreatedEmployee_WhenSuccessful()
        {

            var command = new CreateEmployeeCommand { employee = employee };
            var mockResult = ResultViewModel.Ok(employee);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateEmployeeCommand>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.CreateEmployeeAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var returnedEmployee = Assert.IsType<Result<Employee>>(result);
            Assert.Equal(employee.Email, returnedEmployee.Value.Email);
            Assert.Equal(employee.Firstname, returnedEmployee.Value.Firstname);
            Assert.Equal(employee.Lastname, returnedEmployee.Value.Lastname);
        }

        [Fact]
        public async Task CreateEmployeeAsync_ShouldReturnFailure_WhenEmployeeCreationFails()
        {
            var command = new CreateEmployeeCommand { employee = badEmployee };
            var mockResult = ResultViewModel.Fail("Employee Data Failed To Submit.");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateEmployeeCommand>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.CreateEmployeeAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Employee Data Failed To Submit.", result.Error);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldReturnSuccessResult_WhenEmployeeIsUpdatedSuccessfully()
        {

            var command = new UpdateEmployeeCommand { employee = employee };
            var mockResult = ResultViewModel.Ok(employee);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateEmployeeCommand>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.UpdateEmployeeAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            var returnedEmployee = Assert.IsType<Result<Employee>>(result);
            Assert.Equal(employee.Email, returnedEmployee.Value.Email);
            Assert.Equal(employee.Firstname, returnedEmployee.Value.Firstname);
            Assert.Equal(employee.Lastname, returnedEmployee.Value.Lastname);

        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldReturnBadRequest_WhenEmployeeUpdateFails()
        {
            // Arrange
            var command = new UpdateEmployeeCommand
            {
                employee = badEmployee
            };

            var mockResult = ResultViewModel.Fail("Employee not found");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateEmployeeCommand>(), default))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.UpdateEmployeeAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Employee not found", result.Error);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldReturnInternalServerError_WhenUnhandledExceptionOccurs()
        {
            // Arrange
            var command = new UpdateEmployeeCommand
            {
                employee = badEmployee
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateEmployeeCommand>(), default))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _controller.UpdateEmployeeAsync(command));
            Assert.Equal("Unexpected error", exception.Message);
        }


    }
}
