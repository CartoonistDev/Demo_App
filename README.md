# Demo_App
This project is a .NET microservice application that demonstrates the use of Hangfire for background job processing, role-based authorization using .Net Identity Framework, rate limiting, optimize queries and LINQ. It includes scenarios for sending emails and generating reports asynchronously, ensuring jobs are retried in case of failures.

## Features
1. **User Authentication and Authorization (ASP.NET Core)**: Implements user registration, login, and role-based access control.
2. **Database Query Optimization (Entity Framework Core)**: Optimizes data retrieval using efficient queries.
3. **Web API Rate Limiting**: Implements rate limiting to control API request rates.
4. **Background Job Processing (Hangfire)**: Schedules and processes background jobs such as sending emails and generating reports.
5. **Microservices Communication (REST)**: The microservice uses a REST client to communicate between services.

### Getting Started
#### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQLite](https://www.sqlite.org/download.html)
- Hangfire
- FluentValidation
- Carter for minimal API
- Mapster
- MediaR
- Entity Framework
- RestSharp

### Setting Up the Project
- Clone the repository:
  ```sh
    git clone https://github.com/CartoonistDev/Demo_App.git
    cd Demo_App

- Install dependencies:
  ```sh
    dotnet restore

- Database Setup
This project uses an in-app SQLite database to store user information. To create the database and apply the initial migrations, run the following command in the terminal or Package Manager Console:
  ```sh
    dotnet ef migrations add InitialCreate
    dotnet ef database update

- Configuration
Ensure your appsettings.json file has the correct connection string for SQLite:
  ```json
  {
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

- Running the Application
To run the application, use the following command:

  ```sh
  dotnet run


- Navigate to `http://localhost:5083/hangfire` to access the Hangfire dashboard and monitor your background jobs.
- Navigate to `http://localhost:5083/swagger` to access the product swagger documentation.
- Navigate to `http://localhost:5227/swagger` to access the auth swagger documentation.


### Contributing
Contributions are welcome! Please feel free to submit a pull request or open an issue to discuss any changes.

### License
This project is licensed under the MIT License.
