# WebAPI Project

## Overview
This project provides a RESTful API for managing game resources, including authentication, inventory and scoring.

## API Endpoints

| Endpoint                   | Method | Description           | Authentication Required |
|----------------------------|--------|-----------------------|-------------------------|
| `/api/game/login`          | GET    | Login existing user   | No                      |
| `/api/game/register`       | GET    | Register new user     | No                      |
| `/api/game/user`           | GET    | Get user              | Yes                     |
| `/api/game/items`          | POST   | Show user items       | Yes                     |
| `/api/game/item-random`    | POST   | Generate random item  | No                      |
| `/api/game/item-add`       | GET    | Add new item          | Yes                     |
| `/api/game/leaderboard`    | GET    | Get leaderboard       | No                      |
| `/api/game/score`          | POST   | Record user score     | Yes                     |

**Total APIs:** 8  
**Authentication Required:** 4

## Database
- **Tables:** Users, Inventories

## Configuration

Before running the project, configure the following in `appsettings.json`:

1. **Frontend Origin Config**
    - Change the frontend.url to your client url

2. **JWT**
    - Set jwt key, issuer, audience to your preference. You can use the default value if you want. 

3. **Dependency**
    - Ensure following packages are installed:
    ```
    dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0 
    dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.10
    ```
    
## Getting Started

1. Start the server:  
    ```
    dotnet run
    ```

## To Run Test
1. Ensure below packages are installed:
    ```
    dotnet add package Microsoft.NET.Test.Sdk 
    dotnet add package Moq 
    dotnet add package xunit 
    dotnet add package xunit.runner.visualstudio
    ```
2. Start running test:  
    ```
    dotnet test
    ```
    
