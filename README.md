# EvolentAssignment
Evolent Assignment : API code

This API project is Rest API project in .net core. It provides APIs for contact management such as 
- List contact
-Add contact
-Update Contact
-Delete Contact

The solution has three projects for API, DB repository, Unit test.

Database used is SQL server and have kept connection string on appsettings.json file.

Tried to implement security using JWT Bearer auth but its not working hence i have commented the Authorize attribute for now. You will find middleware for security implementation in API project.

I have deployed the app in cloud. URL for same is https://evolent-assignement-app.azurewebsites.net/swagger/index.html

