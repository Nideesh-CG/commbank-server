# Coding and Testing Results Report

This task document outlines the code modifications and test executions for the CommBank Server application.

## 🛠️ Code Modifications Summary

Here are the key changes implemented in the repository:

### 1. Framework Migration to .NET 10.0
Both the main application and test projects were upgraded from `.NET 6.0` to `.NET 10.0` to support modern C# language features and libraries:
* **File:** [CommBank.csproj](file:///d:/fullstack/SoftwareEngineering/commbank-server/CommBank-Server/CommBank.csproj)
* **File:** [CommBank.Tests.csproj](file:///d:/fullstack/SoftwareEngineering/commbank-server/CommBank.Tests/CommBank.Tests.csproj)

### 2. Goal Model Extension
Added the `Icon` property to support graphic icons for user saving goals.
* **File:** [Goal.cs](file:///d:/fullstack/SoftwareEngineering/commbank-server/CommBank-Server/Models/Goal.cs)
```csharp
public class Goal
{
    // ... existing properties ...
    public string? Icon { get; set; }
}
```

### 3. Database Connection and Configuration
* Hardcoded Atlas Connection Strings for development environments in [Program.cs](file:///d:/fullstack/SoftwareEngineering/commbank-server/CommBank-Server/Program.cs).
* Configured proper Mongo settings in [Secrets.json](file:///d:/fullstack/SoftwareEngineering/commbank-server/CommBank-Server/Secrets.json) and [appsettings.json](file:///d:/fullstack/SoftwareEngineering/commbank-server/CommBank-Server/appsettings.json).
* Commented out `UseHttpsRedirection()` in [Program.cs](file:///d:/fullstack/SoftwareEngineering/commbank-server/CommBank-Server/Program.cs) to avoid local port redirection mismatches.

### 4. Added Controller Unit Tests
* Implemented the `GetForUser` test cases inside [GoalControllerTests.cs](file:///d:/fullstack/SoftwareEngineering/commbank-server/CommBank.Tests/GoalControllerTests.cs) to verify routing endpoints fetching goals by specific users:
```csharp
[Fact]
public async void GetForUser()
{
    // Arrange
    var goals = collections.GetGoals();
    var users = collections.GetUsers();
    
    var testUser = users[0];
    foreach (var goal in goals)
    {
        goal.UserId = testUser.Id;
    }
    
    IGoalsService goalsService = new FakeGoalsService(goals, goals[0]);
    IUsersService usersService = new FakeUsersService(users, testUser);
    GoalController controller = new(goalsService, usersService);

    var httpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
    controller.ControllerContext.HttpContext = httpContext;

    // Act
    var response = await controller.GetForUser(testUser.Id!);

    // Assert
    Assert.NotNull(response);
    var returnedGoals = response;
    Assert.True(returnedGoals!.Any(), "The endpoint should return at least one goal for this user.");
    foreach (Goal goal in returnedGoals)
    {
        Assert.NotNull(goal);
        Assert.Equal(testUser.Id, goal.UserId);
    }
}
```

---

## 🧪 Testing Results

All unit tests compiled successfully and passed using `dotnet test`.

```text
Passed!  - Failed:     0, Passed:    11, Skipped:     0, Total:    11, Duration: 46 ms - CommBank.Tests.dll (net10.0)
```

### Breakdown of Passed Tests (11 Total)

| Test Suite | Test Method | Status |
| :--- | :--- | :--- |
| **AccountControllerTests** | `GetAll` | ✅ Passed |
| | `Get` | ✅ Passed |
| **GoalControllerTests** | `GetAll` | ✅ Passed |
| | `Get` | ✅ Passed |
| | `GetForUser` | ✅ Passed |
| **TagControllerTests** | `GetAll` | ✅ Passed |
| | `Get` | ✅ Passed |
| **TransactionControllerTests** | `GetAll` | ✅ Passed |
| | `Get` | ✅ Passed |
| **UserControllerTests** | `GetAll` | ✅ Passed |
| | `Get` | ✅ Passed |
