# Accounting Demo App
This webapp was made for personal use. It serves to connect to a mysql database and manage a kindergarten-like structure. <br/><br/>
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/Sergio-GC/AccountingDemo/.github%2Fworkflows%2Fdeploy-to-pi.yml)
![Static Badge](https://img.shields.io/badge/License-MIT-yellow)

## Main features
- Manage list of kids (create, list, update and delete kids from the database)
- Manage list of working days (create, list, update and delete)
- Manage list of prices (create, list, update and delete)
- Calculate total amount for a given period of time
- Display near future working days
- Update siblings relationship between kids
- WebApp translated to English, French and Spanish

## Screenshots
- Translations
![Translations](/Assets/translations.gif)

- Creation of a new working day
![Working day creation](/Assets/createWD.gif)

- Creation of a new kid
![Kid creation](/Assets/kidCreation.gif)

- Management of sibling relationships
![Siblings management](/Assets/siblingsManagement.gif)

- Generation of invoices
![Facturation](/Assets/facturationPreview.gif)

## Architecture
This repository contains several projects (Visual Studio).
- REST API
- WebApp
- EF Application

### Tech Stack
- C# (aspnet core 8.0)
- Entity Framework Core
- MySql
- Docker (Docker compose)
- Github Actions
- JavaScript (no framework used, WebApp views created in .cshtml files, js added to manage adding new fields)

The WebApp follows the MVC architecture, even though the models come directly from the DTO, which explains the lack of models inside the project.

## Github workflow
This repository uses Github actions to set up a CI/CD pipeline that builds, tests and deploys the project whenever a new push is done in the `master` branch. The API as well as the WebApp are run in a docker container inside a raspberry pi.

## Setup for local use
In order to make the project work it needs an `appsettings.json` file located in the root of the EFAccounting project. The contents should be the following:
```
{
  "ConnectionStrings": {
    "DefaultDb": "Server=myServer;Database=myDb1;Trusted_Connection=True;"
  }
}
```

For the WebApp to work properly, the REST API must be running. By default, it runs in port `5000`

## Next steps
- Clean user interface
- Add more unit tests
- Android App
- Improve error handling
- Logging
