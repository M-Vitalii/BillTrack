# Overview
This is an educational ASP.NET Core 8.0 REST API project built using FastEndpoints. It is designed to streamline the billing process for accounting purposes

# Technologies
- ASP.NET Core 8.0: The framework for building the web application.
- Entity Framework Core (PostgreSQL): The ORM for database interactions.
- NUnit: The testing framework.
- Docker: For containerization.
- Pub/Sub provider (AWS SQS): For message queuing.
- Fluent Validation: For validation.
- FastEndpoints: For creating endpoints quickly and efficiently.

# Database
The database is built using PostgreSQL and includes the following entities:
![ER Diagram](assets/er-diagram.png)

### Relationships Overview
- **User**: Independent entity with no direct relationships to other entities.
- **Employee**: Has many Workdays and Invoices; belongs to one Department and one Project.
- **Department**: Has many Employees.
- **Project**: Has many Employees.
- **Workday**: Belongs to one Employee.
- **Invoice**: Belongs to one Employee.