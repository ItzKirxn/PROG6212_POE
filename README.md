# RaceDay - PROG6212 POE

## Student Information

**Student Name:** Kieran Pillay  
**Student Number:** ST10445189  
**Module:** PROG6212  
**Institution:** Emeris Durban North  
**Project:** RaceDay

---

## 1. Project Overview

RaceDay is a race and event management system designed to help organisers manage running, walking and cycling events.

The system allows organisers to create and manage events, categories and race routes. Participants can register for accounts, browse available events, enrol in event categories and view their enrolments and race results.

This repository contains the development work for the PROG6212 Portfolio of Evidence (POE).

The project is divided into the following parts:

- **Part 1:** System planning, database design, Entity Relationship Diagram (ERD) and API endpoint planning.
- **Part 2:** RESTful API development, database integration, authentication, role-based access control, unit testing and CI/CD.
- **Part 3:** Planned enhancements, including an MVC web application, Azure Blob Storage, weather API integration and Docker containerisation.

---

## 2. System Roles

RaceDay supports two main user roles.

### Organiser

Organisers are responsible for managing race events and participant activities. Their responsibilities include:

- Creating, editing and deleting events.
- Managing event categories.
- Managing event routes.
- Viewing enrolments for their events.
- Capturing and managing participant results.

### Participant

Participants use the system to access event information and manage their participation. Their responsibilities include:

- Creating an account and logging in.
- Browsing available events.
- Selecting categories and enrolling in events.
- Viewing their own enrolments.
- Viewing their personal race results.

Role-based access control restricts access to protected operations according to the user's role.

---

## 3. Technologies Used

The project uses the following technologies and tools:

- **C#** – Application development.
- **ASP.NET Core Web API (.NET 8)** – RESTful API development.
- **Entity Framework Core** – Object-relational mapping and database operations.
- **Microsoft SQL Server** – Relational database management.
- **SQL Server LocalDB** – Local database development, where configured.
- **xUnit** – Unit testing.
- **Swagger / OpenAPI** – API documentation and endpoint testing.
- **Git and GitHub** – Version control and source code management.
- **GitHub Actions** – Automated build and test workflow.
- **draw.io** – Entity Relationship Diagram creation.
- **Visual Studio** – Development and debugging.

---

## 4. Part 1: Planning and Database Design

Part 1 focused on planning the system, designing the database and documenting the API endpoints before implementing the RESTful API.

### 4.1 Planning Documents

The following planning documents are stored in the `docs` folder:

- `API_Endpoint_Plan.md` – Documents the planned API endpoints, HTTP methods and functionality.
- `RaceDay_Database_Script.sql` – SQL script for creating the RaceDay database structure.
- `RaceDay_ERD.drawio.png` – Entity Relationship Diagram illustrating the database tables and their relationships.

### 4.2 Database Information

**Database name:** `RaceDayDB`

The database contains the following tables:

| Table | Purpose |
|---|---|
| `User` | Stores user account and profile information. |
| `Event` | Stores race and event information. |
| `Category` | Stores categories available for events. |
| `Route` | Stores route information associated with events. |
| `Enrolment` | Records participant enrolments in event categories. |
| `Result` | Stores participant race results. |

### 4.3 Database Relationships

The database design includes the following relationships:

- User → Event
- User → Enrolment
- Event → Category
- Event → Route
- Category → Enrolment
- Enrolment → Result
- User → Result

These relationships support event management, participant registration, enrolment tracking and result management.

The exact foreign keys and relationship cardinalities are defined in the database script and ERD.

### 4.4 Running the Database Script

To create the database manually using SQL Server Management Studio (SSMS):

1. Open SQL Server Management Studio.
2. Connect to your SQL Server instance.
3. Open `docs/RaceDay_Database_Script.sql`.
4. Execute the script.
5. Refresh the Databases folder and verify that `RaceDayDB` has been created.

**Note:** If the project uses Entity Framework Core migrations, follow the Part 2 setup instructions instead of unnecessarily recreating the database.

### 4.5 Part 1 Video Presentation

**YouTube Link:** https://youtu.be/Uf77KLrUiIg

---

## 5. Part 2: RESTful API Development

Part 2 involved developing the RaceDay RESTful API using ASP.NET Core Web API, Entity Framework Core and SQL Server.

The API provides endpoints for authentication, profile management, events, categories, enrolments and results.

### 5.1 API Functionality

The implemented API functionality includes the following areas:

| Feature | Description |
|---|---|
| Authentication | Supports user registration and login. |
| Profile management | Provides functionality for user profile operations. |
| Event management | Supports event-related operations. |
| Category management | Supports event category operations. |
| Enrolments | Manages participant enrolments. |
| Results | Supports participant race result operations. |
| Role-based access | Restricts protected operations according to user roles. |
| Database integration | Uses Entity Framework Core to interact with SQL Server. |
| API documentation | Uses Swagger / OpenAPI to document and test endpoints. |

### 5.2 Authentication and Security

The API includes authentication and access control to protect user information and administrative operations.

The security implementation includes:

- User registration and login endpoints.
- Password hashing rather than storing plain-text passwords.
- Session management, where configured by the application.
- Role-based restrictions for organisers and participants.
- Protected endpoints for operations requiring an authenticated user.

Authentication and authorisation should be tested to confirm that users can access only the functionality permitted by their roles.

### 5.3 Database Integration

Entity Framework Core is used to connect the application to SQL Server and map application models to database tables.

Code-First migrations are used to create and update the database schema.

The database configuration is defined in the application's configuration files and startup code.

### 5.4 Swagger / OpenAPI

Swagger provides an interactive interface for exploring and testing the API.

Once the API is running, open:

`http://localhost:5046/swagger`

If the application starts on a different port, use the Swagger URL displayed by the application.

Swagger can be used to inspect available endpoints, submit requests and review HTTP responses.

---

## 6. Setup and Installation

Follow these instructions to run the project locally.

### 6.1 Prerequisites

Ensure that the following software is installed:

- .NET 8 SDK.
- Visual Studio with the ASP.NET and web development workload, or an equivalent .NET development environment.
- SQL Server or SQL Server LocalDB, according to the configured connection string.
- Git.
- Entity Framework Core command-line tools, if required for migrations.

### 6.2 Clone the Repository

Open a terminal and run:

```bash
git clone https://github.com/ItzKiroxn/PROG6212_POE.git
cd PROG6212_POE
```

### 6.3 Configure the Database Connection

Open the API project's configuration file, such as `appsettings.json`.

Check that the connection string points to an installed SQL Server instance.

For example, a LocalDB connection string may look like this:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=RaceDayDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

**Important:** This is an example. Keep the actual connection string name and configuration required by your project. If your project uses a different SQL Server instance, update the server name accordingly.

### 6.4 Apply Database Migrations

Ensure that the Entity Framework Core tools are installed if they are not already available:

```bash
dotnet tool install --global dotnet-ef
```

If the tool is already installed, update it when necessary rather than installing it again.

From the repository root, apply the migrations:

```bash
dotnet ef database update --project RaceDay.Api
```

If the API project has a separate startup project or the solution uses a different project structure, adjust the command to match the repository.

### 6.5 Run the API

From the repository root, run:

```bash
dotnet run --project RaceDay.Api
```

Wait for the application to start successfully.

Open the Swagger interface using the local URL displayed in the terminal. If the configured HTTP port is `5046`, use:

`http://localhost:5046/swagger`

You can now explore and test the available API endpoints.

---

## 7. Unit Testing

The project includes an xUnit test project for testing application functionality.

The test suite is intended to cover areas such as:

- User registration.
- Login and authentication.
- Event management.
- Role-based access restrictions.
- Enrolment functionality.
- Other implemented API operations.

The test project includes the test suite referred to during development as the **Jarred Loves test suite**.

### Running the Tests

From the repository root, execute:

```bash
dotnet test
```

The command builds the test projects and executes the available tests.

Review the output to confirm that the tests complete successfully. If any tests fail, investigate and resolve the errors before submitting the project.

---

## 8. Continuous Integration and Continuous Delivery (CI/CD)

The project uses GitHub Actions to automate the build and test process.

The workflow configuration is stored in:

`.github/workflows/ci.yml`

The workflow is intended to build the application and run the unit tests when the configured GitHub events occur, such as pushes or pull requests.

### 8.1 Viewing the CI/CD Workflow

1. Open the RaceDay GitHub repository.
2. Select the **Actions** tab.
3. Open the relevant workflow.
4. Select the latest workflow run.
5. Check whether the build and test jobs completed successfully.

### 8.2 CI/CD Screenshot

Save a screenshot showing the successful GitHub Actions workflow in the following folder:

`PROG6212_POE_screenshots/ci-screenshot.png`

Ensure that the screenshot is committed and pushed to the repository.

The screenshot should show the workflow run and its successful status clearly.

---

## 9. Part 2 Video Presentation

**YouTube Link:** 

<!-- Add your Part 2 YouTube video link here after recording and uploading the presentation. -->

The Part 2 video should demonstrate the main features of the RESTful API and provide evidence that the application and its tests work correctly.

### Suggested Video Demonstration

The presentation can include:

1. A brief introduction to the RaceDay project.
2. The project structure in Visual Studio.
3. The main models, controllers and database context.
4. The authentication and role-based access implementation.
5. The Swagger interface and a demonstration of selected API endpoints.
6. The database structure in SQL Server.
7. The unit tests running successfully.
8. The successful GitHub Actions workflow.
9. A short conclusion summarising the completed work.

Replace the empty YouTube link section above once the video has been uploaded.

---

## 10. Project Structure

The repository contains the planning documents, API implementation, database resources, testing resources and supporting documentation.

The main project components include:

- `docs/` – Planning documents, database script and ERD.
- `RaceDay.Api/` – ASP.NET Core Web API project.
- Test project – xUnit tests for application functionality.
- `.github/workflows/` – GitHub Actions workflow configuration.
- `PROG6212_POE_screenshots/` – Screenshots supporting the submission.
- `README.md` – Project overview, setup instructions and documentation.

**Note:** Folder and file names should match the actual structure of the repository. Update this section if your test project or screenshot folder has a different name.

---

## 11. Project Status

### Part 1: Planning and Database Design

- [x] API endpoint planning.
- [x] Database design.
- [x] SQL Server database script.
- [x] Entity Relationship Diagram.
- [x] Part 1 video presentation.

### Part 2: RESTful API Development

The following items should be marked complete once they have been implemented and verified in the repository.

- [x] ASP.NET Core Web API setup.
- [x] Entity Framework Core integration.
- [x] Database migrations.
- [x] Authentication and password hashing.
- [x] Session management and role-based access control.
- [x] API endpoints for authentication, profiles, events, categories, enrolments and results.
- [x] Swagger / OpenAPI integration.
- [x] xUnit test project.
- [x] GitHub Actions CI/CD workflow configuration.
- [ ] Successful unit test run verified.
- [ ] Successful GitHub Actions build verified.
- [ ] CI/CD screenshot committed to the repository.
- [ ] Part 2 video recorded and uploaded.
- [ ] Part 2 YouTube link added to this README.

Update the checklist to reflect the final state of your implementation and submission.

---

## 12. Future Development: Part 3

The following enhancements are planned for the next stage of the RaceDay project:

- **MVC web application:** Develop a user-friendly web interface that consumes the RESTful API.
- **Azure Blob Storage:** Support uploading and storing route files, such as GPX files.
- **Weather API integration:** Provide weather information relevant to race events.
- **Docker containerisation:** Package application components into containers to improve portability and deployment.
- **Further testing:** Expand automated testing and improve application reliability.

These enhancements can extend RaceDay into a more complete event management solution.

---

## 13. References

The following sources were consulted for technical guidance during the planning and development of the RaceDay project.

Microsoft (n.d.) *Create web APIs with ASP.NET Core*. Available at: https://learn.microsoft.com/en-us/aspnet/core/web-api/ (Accessed: 17 October 2026).

Microsoft (n.d.) *Entity Framework Core documentation*. Available at: https://learn.microsoft.com/en-us/ef/core/ (Accessed: 17 October 2026).

Microsoft (n.d.) *DbContext configuration*. Available at: https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/ (Accessed: 17 October 2026).

Microsoft (n.d.) *Applying migrations*. Available at: https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying (Accessed: 17 October 2026).

Microsoft (n.d.) *Relationships in Entity Framework Core*. Available at: https://learn.microsoft.com/en-us/ef/core/modeling/relationships (Accessed: 17 October 2026).

Microsoft (n.d.) *Session and state management in ASP.NET Core*. Available at: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state (Accessed: 17 October 2026).

Microsoft (n.d.) *Password hashing in ASP.NET Core*. Available at: https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing (Accessed: 17 October 2026).

Microsoft (n.d.) *Integration tests in ASP.NET Core*. Available at: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests (Accessed: 17 October 2026).

Microsoft (n.d.) *Unit testing in .NET*. Available at: https://learn.microsoft.com/en-us/dotnet/core/testing/ (Accessed: 17 October 2026).

Microsoft (n.d.) *Create a table (Transact-SQL)*. Available at: https://learn.microsoft.com/en-us/sql/t-sql/statements/create-table-transact-sql (Accessed: 17 October 2026).

Microsoft (n.d.) *Web API design best practices*. Available at: https://learn.microsoft.com/en-us/azure/architecture/best-practices/api-design (Accessed: 17 October 2026).

Microsoft (n.d.) *Get started with Swashbuckle and ASP.NET Core*. Available at: https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle (Accessed: 17 October 2026).

xUnit (n.d.) *xUnit.net: Unit testing tool for .NET*. Available at: https://xunit.net/ (Accessed: 17 October 2026).

GitHub (n.d.) *GitHub Actions documentation*. Available at: https://docs.github.com/en/actions (Accessed: 17 October 2026).

diagrams.net (n.d.) *diagrams.net: Diagramming application*. Available at: https://www.diagrams.net/ (Accessed: 17 October 2026).

**Reference note:** The sources above provide technical documentation for the technologies used in the project. Ensure that the references accurately reflect the material consulted and follow any additional referencing requirements in the PROG6212 assignment brief.

---

## 14. Code Attributions and AI Assistance Declaration

### Code Attributions

The RaceDay database design, ERD, API endpoint plan and application implementation form part of the project submission.

Microsoft Learn, xUnit documentation and other technical documentation were used as references for framework usage, database configuration, API development, testing and related implementation concepts.

Specific external code, templates or adapted examples should be acknowledged in the relevant source files and in a dedicated attribution document where appropriate.

### AI Assistance Declaration

AI assistance was used during development for technical explanations, implementation guidance, documentation drafting and troubleshooting.

The student is responsible for reviewing, understanding, testing and verifying the final submitted work. Any AI-generated or AI-assisted material should be disclosed in accordance with the institution's academic integrity policy and the requirements of the assignment brief.


---

## 15. Author

**Kieran Pillay**  
**Student Number:** ST10445189  
**Module:** PROG6212  
**Project:** RaceDay

---

## 16. Final Submission Checklist

Before submitting the RaceDay POE, ensure that:

- [ ] The README accurately describes the implemented application.
- [ ] All required Part 1 planning documents are committed.
- [ ] The database script and ERD are included.
- [ ] The API builds and runs successfully.
- [ ] Database migrations work with the configured database.
- [ ] The API endpoints have been tested.
- [ ] Authentication and role-based restrictions have been tested.
- [ ] The unit tests have been executed and their results checked.
- [ ] GitHub Actions has completed successfully.
- [ ] The CI/CD screenshot is included in the repository.
- [ ] References and code attributions have been reviewed.
- [ ] The Part 2 video link has been added.
- [ ] All final changes have been committed and pushed to GitHub.

