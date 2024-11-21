using Medica_Interview.Models;

namespace Medica_Interview.Service
{
    public class EmployeeService
    {
        private readonly HttpClient _httpClient;

        public EmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            //var employees = new List<Employee>
            //{
            //    new Employee { ProfilePicture = "profile.jpg",
            //    Telephone = "1234567890",
            //    Team = "Development",
            //    Address1 = "123 Main St",
            //    Address2 = "Suite 100",
            //    DateOfBirth = DateOnly.FromDateTime(DateTime.Parse("1990-01-01")),
            //    Firstname = "John",
            //    Lastname = "Doe",
            //    Email = "prisdco@gmail.com",
            //    StartDate = DateOnly.FromDateTime(DateTime.Parse("2020-01-01")),
            //    County = "County A",
            //    JobTitle = "Software Engineer",
            //    LineManager = "Manager A",
            //    Postcode = "12345",
            //    Town = "Techville" }
            //};
            return await _httpClient.GetFromJsonAsync<IEnumerable<Employee>>("https://localhost:7228/api/v1/Employee/all-employees");
            //return employees;
        }

        public async Task<Employee> AddEmployeesAsync(EmployeeDTO employee)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7228/api/v1/Employee/save-employee", employee);

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Return the newly created employee from the response
                return await response.Content.ReadFromJsonAsync<Employee>();
            }
            else
            {
                // Handle the error response (e.g., log the error or throw an exception)
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to save employee: {errorMessage}");
            }
        }

        public async Task<bool> DeleteEmployeesAsync(EmployeeDTO employee)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, "https://localhost:7228/api/v1/Employee/delete-employee")
            {
                Content = JsonContent.Create(employee)
            };

            // Send the request
            var response = await _httpClient.SendAsync(request);

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                // Handle the error response (e.g., log the error or throw an exception)
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to delete employee: {errorMessage}");
            }
        }
    }
}
