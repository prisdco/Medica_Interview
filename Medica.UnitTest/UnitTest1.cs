using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;
using System.Net;
using FluentAssertions;
using System.Text.Json;
using Medica_Interview.Infrastructure.Entities;

namespace Medica.UnitTest
{
    [Collection("ApplicationClientFactory")]
    public class UnitTest1 : IClassFixture<TestingWebAppFactory<Program>>
    {

        private HttpClient _httpClient;
        public UnitTest1(TestingWebAppFactory<Program> webApplicationFactory)
        {
            _httpClient = webApplicationFactory.CreateClient();
        }
        public static IEnumerable<object[]> TestData()
        {
            yield return new object[] { "Simon", "Trelawny", "simontrelawny@medica.co.uk", "7456236458", "05/08/1984", "First floor flat",
                "12 Middle Street", "Smalltown", "Small County", "ST12 5LH", "Software Engineer", "Software Development",
                "Ann Hitch", "12/07/2012", "profile3.png"
            };
            yield return new object[] { "Oluwafemi", "Olajire", "oluwafemiolajire@medica.co.uk", "7456236458", "05/08/1984", "First floor flat",
                "12 Middle Street", "Derby", "Small County", "ST12 5LH", "Software Engineer", "Tech",
                "Ann Hitch", "12/07/2012", "profile1.png"};
            yield return new object[] {"Jake", "Wales", "jakewales@medica.co.uk", "7456236458", "05/08/1984", "First floor flat",
                "12 Middle Street", "London", "London", "ST12 5LH", "Analyst Engineer", "Tech",
                "Ann Hitch", "12/07/2012", "profile2.png" };
        }

        [Fact]
        public async Task GetAllEmployees_ShouldReturnSuccessResponse()
        {
            // Arrange
            var requestUri = "https://localhost:7228/api/v1/Employee/all-employees";

            // Act
            var response = await _httpClient.GetAsync(requestUri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Parse the response content
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonSerializer.Deserialize<Employee>(responseContent);

            jsonResponse.Should().NotBeNull();
        }
    }
}