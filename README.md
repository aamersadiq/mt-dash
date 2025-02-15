# My Bank App

This repository containts a banking dashboard solution, integrating front end, back end, and database components.

Once the application is up and running, you can:

- Log in using username/password of account-holder/account-holder@123
- View the dashboard to see account balances and transactions
- Transfer funds between accounts

## .Net Core 8 Web API (back-end folder)

API to login, view accounts and their transactions.

### Prerequesites

- .Net Core 8
- Visual Studio for editing files
- SQL Server Express Edition
- SQL Server Management Studio
- Update DefaultConnection connection string in appSetting.json to point to SQL Server instance

### Available Scripts

From the repository root folder, you can run using PowerShell or Bash (replace folder seperator \ with /):

#### Runs the unit tests

`dotnet test back-end\BankDash.UnitTests`

#### Creates the database and tables along with seed data.

`dotnet tool install --global dotnet-ef --version 8.\*`

`dotnet ef database update --project back-end\BankDash.Db\BankDash.Db.csproj --startup-project back-end\BankDash.Api/BankDash.Api.csproj`

#### Runs the API. Swagger url: https://localhost:7008/swagger/index.html

`dotnet run --project back-end\BankDash.Api/BankDash.Api.csproj`

## React app (front-end folder)

Web app for login, view accounts and their transactions.
This project was bootstrapped with [Create React App](https://github.com/facebook/create-react-app).

### Prerequesites

- NodeJs version 20 or higher
- Visual Studio Code for editing files

### Available Scripts

#### In the front-end directory, you can run:

`cd front-end`

`npm install`

`npm start`

Runs the app in the development mode.

- Open [http://localhost:3000](http://localhost:3000) to view it in your browser.
- Log in using username/password of account-holder/account-holder@123

#### Run the tests

`npm test`
