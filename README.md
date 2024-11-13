# Medica_Interview
This is blazor server and API .net 8 project.
Note: Ensure the Assests/dataset is present at the API library 
Note: Do not run the database endpoint as it is not connected to any database

# Things that could be done differently.
If I have more time, I would further decouple the DatabaseSource extension by adding a new library, which will be separate from the current flow. This will prove the effectiveness of the SOLID principle application.
An update UI form would have been designed on the blazor to test the update endpoint.
The blazor is a bit not decoupled to my liking and this is due to timeframe. Some of the codes should be in a different library for re-usability purpose. Some components like toastr would have been well designed and made to be re-usable.
The validation of the UI form is a lazy man's Job. Normally, I should have ensure each field is validated and error is printed individually.
The API calling could have been well arrange using a dynamic pattern. The current process will cause a repetition if another service is to be created that requires API

# How the solution functionality could be extended or improved with more time.
The API is currently able to switch between different datasource but the current approach can be improved to handle scaling and performance if given more time.
The GetEndPoint can be made dynamic regardless of the datasource and I can ensure the API have just a single endpoint to dynamically serves all requests dealing with fetching information.
The blazor UI do not have a global error management/monitors unlike its API. This is an important aspect but time constraint.
The API and Blazor do not have security implementation and this can be done if given more time.
The database source can be extended to create the database if it does not exist and populate any default entry once the endpoint is called.

# Decision Process
The current architecture approach is called compnent based. Most solutions in the project ought to have their library eg Business logic, third party integration, internal integration etc for re-usability purposes
Using Mediator design for the API is due to the application of SOLID principle.
I created a generic IRepository which is responsible for managing datasource and object type. It only cares about the IDataSourceReader while the implementation is kept hidden. 

# Payload Test
{
  "employee": {
    "Firstname": "Oluwafemi",
    "Lastname": "Olajire",
    "Email": "oluwafemiolajire@medica.co.uk",
    "Telephone": "7894000111",
    "DateOfBirth": "1990-12-20",
    "Address1": "1 Derby Street",
    "Address2": "",
    "Town": "Derbyshire",
    "County": "Big County",
    "Postcode": "TN12 3AB",
    "JobTitle": "Fullstack Developer",
    "Team": "Technology",
    "LineManager": "Klye Owen",
    "StartDate": "2023-10-31",
    "ProfilePicture": "profile3.png"
  }
}
