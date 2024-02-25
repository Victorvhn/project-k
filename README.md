# Project K - Financial Management System

This documentation is a work in progress.

## Overview

Project K is a comprehensive financial management system designed to help individuals and businesses effectively manage their finances. This repository contains the source code for Project K, providing a powerful and customizable solution for various financial needs.

## Pre-requisites

Before you begin with Project K, make sure you have the following prerequisites installed and configured:

- Node.js: The APP is built using `Node.js`. Download and install it from [Node.js](https://nodejs.org/).
- Package Manager (pnpm): Ensure you have `pnpm` installed. Install it using `npm` with `npm install -g @pnpm/exe`.
- Database (postgres): The API relies on a database for data storage. Install and set up a compatible database. Download and install it from [Postgres](https://www.postgresql.org/download/).
- OAuth keys: Create keys at [GitHub Developer Settings](https://github.com/settings/developers) and [Google Cloud Console](https://console.cloud.google.com/apis/credentials).
- .NET Core: The API is built using .NET Core 8. Download and install it from [.NET Core](https://dotnet.microsoft.com/download).
- Seq (optional): Configure Seq for a custom dashboard for logs. Download it from [Seq](https://datalust.co/download).

## Features

- **Expense Tracking:** Easily record and categorize your expenses to gain insights into your spending habits.
- **Budget Management:** Set and track budgets to ensure better financial planning and control.
- **Income Management:** Log and monitor your sources of income for a complete financial overview.

## Getting Started

To get started with Project K's API, follow these steps:

1. **Clone the Repository:**
```bash
git clone https://github.com/Victorvhn/project-k.git
```

### API

1. **Configure appsettings.json**
   
Configure the appsettings.json file (Adapters/Driving/Apis/ProjectK.Api), including Database, Seq, Authentication keys, etc.

Make sure to review and customize the configuration files according to your environment.

2. **Install dependencies**
```bash
cd project-k/api && dotnet restore Adapters/Driving/Apis/ProjectK.Api/ProjectK.Api.csproj
```

3. **Start the API**
```bash
dotnet run --project Adapters/Driving/Apis/ProjectK.Api/ProjectK.Api.csproj
```

### APP

1. **Configure .env**
   
Configure the .env file, including API Url, Authentication provider keys, Authentication [Docs](https://authjs.dev/getting-started/providers/oauth-tutorial), etc.

Make sure to review and customize the configuration files according to your environment.

2. **Install dependencies**
```bash
cd project-k/app && pnpm install
```

3. **Start the APP**
```bash
pnpm dev
```

## Links
[Staging Swagger](https://staging-api.project-k.tech/swagger/index.html)

[Staging APP](https://staging-app.project-k.tech)


[Production APP](https://www.project-k.tech)
