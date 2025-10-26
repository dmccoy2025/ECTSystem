# Contributing to ECTSystem

Thank you for your interest in contributing to the Electronic Case Tracking System (ECTSystem)! This document provides guidelines and standards for contributing to the project.

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Getting Started](#getting-started)
- [Development Workflow](#development-workflow)
- [Coding Standards](#coding-standards)
- [Documentation Requirements](#documentation-requirements)
- [Testing Requirements](#testing-requirements)
- [Pull Request Process](#pull-request-process)
- [Commit Message Guidelines](#commit-message-guidelines)

## Code of Conduct

This project adheres to professional standards of conduct. All contributors are expected to:

- Be respectful and inclusive in all interactions
- Focus on constructive feedback
- Prioritize the best interests of the project and its users
- Maintain confidentiality of sensitive information

## Getting Started

### Prerequisites

Before contributing, ensure you have:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or Developer Edition)
- [Git](https://git-scm.com/)
- Basic understanding of .NET, Blazor, gRPC, and Entity Framework Core

### Setup

1. Fork the repository on GitHub
2. Clone your fork locally:
   ```bash
   git clone https://github.com/YOUR_USERNAME/ECTSystem.git
   cd ECTSystem
   ```
3. Add the upstream repository:
   ```bash
   git remote add upstream https://github.com/dmccoy2025/ECTSystem.git
   ```
4. Build the solution:
   ```bash
   dotnet build ElectronicCaseTracking.sln
   ```
5. Run tests to verify setup:
   ```bash
   dotnet test
   ```

## Development Workflow

### Branching Strategy

We follow a feature branch workflow:

1. **main** - Production-ready code, protected branch
2. **feature/your-feature-name** - New features
3. **fix/your-fix-name** - Bug fixes
4. **docs/your-docs-update** - Documentation updates
5. **refactor/your-refactor-name** - Code refactoring

### Creating a Branch

```bash
# Update your local main branch
git checkout main
git pull upstream main

# Create a new feature branch
git checkout -b feature/add-new-workflow
```

### Staying Up-to-Date

Regularly sync your fork with the upstream repository:

```bash
git checkout main
git pull upstream main
git push origin main
```

## Coding Standards

### C# Coding Conventions

Follow Microsoft's [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions) and these project-specific rules:

#### File-Scoped Namespaces
**REQUIRED**: Use file-scoped namespaces throughout the solution:

```csharp
namespace AF.ECT.Server.Services;

public class MyService
{
    // Implementation
}
```

#### Naming Conventions

- **Projects**: Prefix with `AF.ECT.*`
- **Async Methods**: Suffix with `Async`
- **Streaming Methods**: Suffix with `Stream` (e.g., `GetUsersOnlineStream`)
- **Private Fields**: Prefix with underscore `_fieldName`
- **Interfaces**: Prefix with `I` (e.g., `IDataService`)

#### Code Styling

- **Inline Variables**: Inline temporary variables wherever possible
- **Lambda Expressions**: Use block body for clarity
- **Regions**: Use regions to organize code (`#region Fields`, `#region Methods`)
- **Static Methods**: Prefer static methods and extension methods for utility functions

#### SOLID Principles

Follow SOLID principles for all code:

- **S**ingle Responsibility: Each class should have one clear purpose
- **O**pen/Closed: Open for extension, closed for modification
- **L**iskov Substitution: Derived classes must be substitutable
- **I**nterface Segregation: Many specific interfaces over one general
- **D**ependency Inversion: Depend on abstractions, not concretions

### Architecture Patterns

#### Dependency Injection
**REQUIRED**: Use dependency injection everywhere. Configure services in `Program.cs`:

```csharp
services.AddScoped<IDataService, DataService>();
```

#### Configuration Validation
**REQUIRED**: Use strongly-typed options classes with data annotations:

```csharp
public class DatabaseOptions
{
    [Required]
    [Range(1, 10)]
    public int MaxRetryCount { get; set; } = 3;
}
```

Validate on startup:
```csharp
services.AddOptions<DatabaseOptions>()
    .BindConfiguration("DatabaseOptions")
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

#### gRPC Patterns

- **Unary Calls**: Single request/response operations
- **Server Streaming**: Large datasets (suffix method with `Stream`)
- **Resilience**: Use Polly policies for retries and circuit breakers
- **Audit Logging**: Implement correlation IDs for all operations

## Documentation Requirements

### XML Documentation Comments
**REQUIRED**: All public methods, classes, properties, and fields must have XML documentation:

```csharp
/// <summary>
/// Retrieves user information by ID.
/// </summary>
/// <param name="userId">The unique identifier of the user.</param>
/// <returns>A task representing the asynchronous operation, containing the user details.</returns>
/// <exception cref="ArgumentException">Thrown when userId is less than or equal to zero.</exception>
/// <exception cref="NotFoundException">Thrown when the user is not found.</exception>
public async Task<UserResponse> GetUserByIdAsync(int userId)
{
    // Implementation
}
```

### Required Documentation Elements

- `<summary>` - Brief description of the element
- `<param>` - Description of each parameter
- `<returns>` - Description of return value
- `<exception>` - Document all exceptions that may be thrown
- `<remarks>` - Additional context (optional but recommended)

### Code Comments

- Add inline comments for complex logic
- Explain **why**, not **what** (code should be self-documenting for "what")
- Use `//` for single-line comments
- Use `/* */` for multi-line explanations

### README Updates

If your change affects:
- Setup/installation
- API usage
- Configuration
- Testing procedures

Update the relevant section in `README.md`.

## Testing Requirements

### Test Coverage
**REQUIRED**: All new code must include tests:

- **Unit Tests**: For business logic, services, and utilities
- **Integration Tests**: For gRPC endpoints and database operations
- **Test Coverage Target**: Minimum 70% coverage for new code

### Test Organization

Tests are located in `AF.ECT.Tests/`:
```
AF.ECT.Tests/
├── Unit/               # Unit tests
├── Integration/        # Integration tests
├── Infrastructure/     # Test base classes and helpers
└── Data/              # Test data classes
```

### Writing Tests

Use xUnit with the following patterns:

```csharp
/// <summary>
/// Tests for the DataService class.
/// </summary>
public class DataServiceTests : DataServiceTestBase
{
    /// <summary>
    /// Verifies that GetUserByIdAsync returns correct user data.
    /// </summary>
    [Fact]
    public async Task GetUserByIdAsync_ValidId_ReturnsUser()
    {
        // Arrange
        var userId = 1;
        
        // Act
        var result = await _dataService.GetUserByIdAsync(userId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.UserId);
    }
}
```

### Running Tests

Before submitting a PR, ensure all tests pass:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test /p:CollectCoverage=true
```

### Build Verification
**CRITICAL**: Always verify the solution builds successfully before committing:

```bash
dotnet build ElectronicCaseTracking.sln
```

## Pull Request Process

### Before Submitting

1. ✅ Code follows style guidelines
2. ✅ All new code has XML documentation
3. ✅ Tests are written and passing
4. ✅ Solution builds successfully (exit code 0)
5. ✅ No new compiler warnings
6. ✅ Code has been self-reviewed
7. ✅ Related documentation updated

### Creating a Pull Request

1. Push your branch to your fork:
   ```bash
   git push origin feature/your-feature-name
   ```

2. Go to the [ECTSystem repository](https://github.com/dmccoy2025/ECTSystem)

3. Click "New Pull Request"

4. Select your branch and provide:
   - **Title**: Clear, concise description (e.g., "Add user profile management")
   - **Description**: 
     - What changes were made
     - Why the changes were necessary
     - How to test the changes
     - Related issues (if any)

### Pull Request Template

```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Unit tests added/updated
- [ ] Integration tests added/updated
- [ ] All tests passing
- [ ] Manual testing completed

## Checklist
- [ ] Code follows style guidelines
- [ ] XML documentation added
- [ ] Self-reviewed code
- [ ] Comments added for complex logic
- [ ] Documentation updated
- [ ] No new warnings
- [ ] Build successful
```

### Review Process

1. At least one approving review required
2. All CI checks must pass
3. Resolve all review comments
4. Maintain a clean commit history (squash if needed)

## Commit Message Guidelines

### Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Type

- **feat**: New feature
- **fix**: Bug fix
- **docs**: Documentation only
- **style**: Code style changes (formatting, no logic change)
- **refactor**: Code refactoring
- **test**: Adding or updating tests
- **chore**: Maintenance tasks

### Examples

```bash
# Good commit messages
feat(workflow): add signature validation to workflow processing
fix(grpc): resolve connection timeout in WorkflowClient
docs(api): update gRPC endpoint documentation
refactor(data): extract database retry logic to separate service
test(user): add unit tests for user registration

# Bad commit messages
Update files
Fixed bug
WIP
asdf
```

### Commit Best Practices

- Use imperative mood ("add feature" not "added feature")
- First line should be 50 characters or less
- Body should wrap at 72 characters
- Reference issues/PRs in footer (e.g., "Closes #123")

## Additional Resources

- [.NET Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- [Microsoft Best Patterns and Practices](https://docs.microsoft.com/en-us/azure/architecture/)
- [gRPC Best Practices](https://grpc.io/docs/guides/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)

## Questions?

If you have questions about contributing:

1. Check existing issues and discussions
2. Review the [README.md](README.md)
3. Check the [Wiki documentation](AF.ECT.Wiki/WikiPages/)
4. Open a new issue with the "question" label

## License

By contributing to ECTSystem, you agree that your contributions will be licensed under the project's MIT License.

---

Thank you for contributing to ECTSystem! 🚀
