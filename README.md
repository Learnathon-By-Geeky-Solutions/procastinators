<div align="center">

![.NET 8](https://img.shields.io/badge/.NET%208-6C2E9B?style=flat-square)
![EF Core](https://img.shields.io/badge/EF%20Core-4D26CE?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=flat-square)
![Azure](https://img.shields.io/badge/Azure-0072C6?style=flat-square)
![Next.js](https://img.shields.io/badge/Next.js-black?style=flat-square&logo=next.js)
![shadcn/ui](https://img.shields.io/badge/shadcn/ui-black?style=flat-square&logo=shadcnui)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Learnathon-By-Geeky-Solutions_procastinators&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Learnathon-By-Geeky-Solutions_procastinators)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Learnathon-By-Geeky-Solutions_procastinators&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=Learnathon-By-Geeky-Solutions_procastinators)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Learnathon-By-Geeky-Solutions_procastinators&metric=coverage)](https://sonarcloud.io/summary/new_code?id=Learnathon-By-Geeky-Solutions_procastinators)

# Finance Tracker

###### Simplify Finances, Ease Collaboration

</div>

Finance Tracker is a comprehensive financial management application designed to help users track personal finances and manage collaborative financial activities. It's objectives are:

-   Provide an organized way to track income and expenses.
-   Offer visibility into financial habits through analytics.
-   Enable collaborative financial activities (e.g., borrowing and lending money).

<div align="center">

<br>

[![wiki](https://img.shields.io/badge/wiki-view-grey?labelColor=black&style=for-the-badge&logo=github)](https://github.com/Learnathon-By-Geeky-Solutions/procastinators/wiki)
[![SRS DOC](https://img.shields.io/badge/SRS%20DOC-view-grey?labelColor=007ACC&style=for-the-badge&logo=googledocs&logoColor=white)](https://github.com/Learnathon-By-Geeky-Solutions/procastinators/blob/main/docs/SRS.pdf)
[![API DOC](https://img.shields.io/badge/API%20DOC-view-grey?labelColor=85EA2D&style=for-the-badge&logo=swagger&logoColor=black)](https://fintrack-api-dev-hrb9cae8fef8facy.eastasia-01.azurewebsites.net/swagger/index.html)
[![live demo](https://img.shields.io/badge/live%20demo-view-grey?labelColor=8A05FF&style=for-the-badge&logo=render)](https://procastinators.onrender.com)

</div>

## Team

### Information

<div align="center">

<table>
  <tr >
    <th colspan="3"><center>ProCastinators</center> </th>
  </tr>
  <tr>
    <th>Role</th>
    <th>Name</th>
    <th>GitHub </th>
  </tr>
  <tr>
    <td>Team Leader</td>
    <td>Md. Sakib Hossain</td>
    <td> <a href="https://github.com/sakibhossain323" target="_blank"> sakibhossain323 </a></td>
  
  </tr>
  <tr>
    <td>Member</td>
    <td>Shat-El Shahriar Khan</td>
    <td> <a href="https://github.com/Ashik150" target="_blank">Ashik150 </a></td>
  </tr>
  <tr>
    <td>Member</td>
    <td>Md. Redwan Bhuiyan Rafio</td>
    <td> <a href="https://github.com/rafio1020" target="_blank">rafio1020 </a></td>
  </tr>
  <tr>
    <td>Mentor</td>
    <td>Mahbubur Rahman</td>
    <td> <a href="https://github.com/mahbub23" target="_blank"> mahbub23</a></td>
  </tr>
</table>

</div>

### Resources

<div align="center">

[![Kanban Board](https://img.shields.io/badge/Kanban%20Board-view-grey?labelColor=007ACC&style=for-the-badge&logo=github&logoColor=white)](https://github.com/orgs/Learnathon-By-Geeky-Solutions/projects/85)
[![Diagrams](https://img.shields.io/badge/Diagrams-view-grey?labelColor=E88305&style=for-the-badge&logo=lucid&logoColor=white)](https://lucid.app/lucidchart/a38c61ea-8c46-4299-a165-344b6b8e7c43/edit?viewport_loc=-1171%2C-1231%2C7008%2C3117%2C0_0&invitationId=inv_3c679402-33c7-4b2b-bf60-358c0073d401)

</div>

## Project Description

### Key Features

-   **Wallet Management**  
    Maintain multiple wallets, track balances, and transfer funds among them.
-   **Category Management**  
    Add, edit or delete categories to track and classify your income and expenses
-   **Income & Expense Tracking**  
    Add, edit or delete transactions (amount, additional note, date/time), link them to with relevant categories and wallets
-   **Analytics & Reports**  
    View category-wise breakdowns and trend charts to understand your income and spending over time.
-   **Loan Management**
    Keep track of borrowing and lending money for non-registered users as well as interactively keep track of loans by leveraging features like loan request, approval, installments etc

### ERD Diagram

<p align="center">
  <img src="https://github.com/user-attachments/assets/09ac0b91-8601-4b71-a4cd-d20e821d2a13" alt="ERD Diagram 1" width="600px" />
  <br><br>
  <img src="https://github.com/user-attachments/assets/d224482d-95d3-406a-ad76-4dd91477f6b1" alt="ERD Diagram 2" width="600px" />
</p>


### Architectural Overview

-   **Deployment Model:** Monolith Architecture
-   **Code Organiztion:** Clean Architecture
-   **Design Patterns:** Mediator, CQRS, Repository


### Repository Structure

```text
FinanceTracker.sln
backend/
├── src
│   ├── FinanceTracker.Api/
│   │   └── Controllers/
│   ├── FinanceTracker.Application/
│   │   └── [any-entity]/
│   │       ├── Commands/
│   │       ├── Queries/
│   │       └── Dtos/
│   ├── FinanceTracker.Domain/
│   │   ├── Constants/
│   │   ├── Entities/
│   │   └── Exceptions/
│   └── FinanceTracker.Infrastructure/
│       ├── Persistence/
│       ├── Migrations/
│       └── Repositories/
└── tests/
    ├── FinanceTracker.Api.Tests/
    └── FinanceTracker.Application.Tests/
frontend/
└── finance-tracker/
```

## Getting Started

### Prerequisites

-   [Visual Studio 2022](https://visualstudio.microsoft.com/) (with **ASP.NET and web development** workload)
-   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
-   [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
-   [Node.js](https://nodejs.org/) (v18.18+)

### Clone the Repository

-   Open up the terminal and run the following commands:

```bash
git clone https://github.com/Learnathon-By-Geeky-Solutions/procastinators
```

### Open Solution in Visual Studio

-   Run the following commands to open directly from terminal:

```bash
cd procastinators
start FinanceTracker.sln
```

### Update Database Connection String

-   Open `appsettings.Development.json` from **FinanceTracker.Api**.
-   Update the connection string according to your database:

```json
"ConnectionStrings": {
  "DefaultConnection": // place your connection string here
}
```

### Run the API

-   Set **FinanceTracker.Api** as the startup project.
-   Press F5 to run the API (alternatively, click **▶ Start** button in the top toolbar)

### Setting Up Frontend

-   Go back to the terminal, navigate to the `frontend/finance-tracker` directory and install dependencies.

```bash
cd frontend/finance-tracker
npm i
```

### Generate Auth Secret:

-   Run the following command:

```bash
npx auth secret
```

> This will generate a `.env.local` file with `AUTH_SECRET` environment variable in it.

### Set Environment variables `.env.local`.

-   Set `AUTH_TRUST_HOST` to true
-   Set `BACKEND_BASE_URL` to the URL your API running (along with port number)

### Launch Application

-   Run the following command to run the Next.js app:

```bash
npm run dev
```

### Configure CORS (if required)

-   Next.js by default runs on `localhost:3000`. but if the default port is changed, update the CORS configuration of **FinanceTracker.Api** from `appsettings.Development.json`

```json
"Cors": {
  "AllowedOrigin": // place url here
}
```

### Fix Fetch Error (if required)

-   If you encounter fetch error due to self signed certificates, a simple work around for local development is using url with `http` in stead of `https`(if API is running on https, as https redirection is not enabled for development environment) for the `BACKEND_BASE_URL` in `env.local`

## Development Guidelines

![Gitflow](https://nvie.com/img/git-model@2x.png)

-   **Branching Model:** [Gitflow](https://nvie.com/posts/a-successful-git-branching-model/)

-   **Commit Message:** [Conventional Commit Message](https://www.conventionalcommits.org/en/v1.0.0/)

-   **Detailed Guidelines:** [@convetions.md](https://github.com/Learnathon-By-Geeky-Solutions/procastinators/blob/main/docs/conventions.md)

</center>

## Further Reading
-   [Github Wiki](https://github.com/Learnathon-By-Geeky-Solutions/procastinators/wiki/)
