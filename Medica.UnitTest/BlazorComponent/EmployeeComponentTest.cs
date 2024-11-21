using BlazorBootstrap;
using Bunit;
using Medica_Interview.Models;
using Medica_Interview.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Medica.UnitTest.BlazorComponent
{
    public class EmployeeComponentTests : TestContext
    {
        private Mock<HttpMessageHandler> httpMessageHandlerMock;


        public EmployeeComponentTests()
        {
            // Register ModalService
            Services.AddSingleton<ModalService>();

            // Setup JSInterop to handle the BlazorBootstrap JavaScript calls
            JSInterop.SetupVoid("window.blazorBootstrap.modal.initialize", _ => true);
            JSInterop.SetupVoid("window.blazorBootstrap.modal.hide", _ => true);
            JSInterop.SetupVoid("window.blazorBootstrap.modal.show", _ => true);
            JSInterop.SetupVoid("showDeleteToast", _ => true);
            JSInterop.SetupVoid("hideDeleteToast", _ => true);

            var inMemorySettings = new Dictionary<string, string>
            {
                { "SomeSettingKey", "SomeSettingValue" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Register IConfiguration
            Services.AddSingleton<IConfiguration>(configuration);
            httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            var httpClient = new HttpClient(httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:7228")
            };

            // Register the EmployeeService with the mock HttpClient
            Services.AddSingleton(new EmployeeService(httpClient));
        }

        [Fact]
        public void RendersLoadingState_WhenProcessing()
        {
            // Arrange
            var component = RenderComponent<Medica_Interview.Components.Pages.Employee>(parameters
                => parameters.Add(p => p.isProcessing, false));

            // Act & Assert
            Assert.Contains("card shadow", component.Markup);
        }

        [Fact]
        public async Task DisplaysEmployeeList_WhenProcessingIsFalse()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { ProfilePicture = "profile.jpg",
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
                Town = "Techville" }
            };

            httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.AbsolutePath == "/api/v1/Employee/all-employees"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(employees)
                });

            var component = RenderComponent<Medica_Interview.Components.Pages.Employee>();

            // Act
            await component.InvokeAsync(() => component.Find("button.btn-success").Click());

            // Assert
            component.WaitForState(() => !component.Instance.isProcessing);
            Assert.Contains("prisdco@gmail.com", component.Markup);
        }


        [Fact]
        public async Task OpensModal_OnAddNewEmployeeClick()
        {
            // Arrange
            var component = RenderComponent<Medica_Interview.Components.Pages.Employee>();
            var addButton = component.Find("button.btn-success");

            // Act
            await component.InvokeAsync(() => addButton.Click());

            // Assert
            Assert.Contains("Add New Employee", component.Markup);
        }
               

    }


}
