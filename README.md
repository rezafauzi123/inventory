# Base Project Dotnet API Template Radya Labs Digital

Base project dotnet web api using .Net 7

# Prerequisite

1. Install [.NET 7][1] (Windows, Linux supported)
2. SqlServer express or developer must be installed first
3. Install [dotnet-ef][dotnetefinstall] tools using command "dotnet tool install --global dotnet-ef"
    4. If already installed use this to update "dotnet tool update --global dotnet-ef"

# Library

1. Bogus [bogus], for helping making fake data
2. Moq [moq], for mocking unit testing
3. Mediator [mediator2], use source generator mediator
4. Linq Dynamic Core [linqdynamiccore], use for better queryable using linq
5. FluentValidation [fluentvalidation], use for validation
6. Jil [jil], another json serializer
7. Serilog [serilog], for logging purpose

# How to add migrations

Before doing migrations, please update appsettings.json first here [appsettings] with your own local machine connection

```json
{
  "ConnectionStrings": {
    "sqlserver": "Server=localhost;Database=InventoryManagementDb;User ID=sa;Password=Password1234;TrustServerCertificate=True;",
    "postgres": ""
  }
}
```

Then proceed below :

```powershell
dotnet ef migrations add InitialCreate -p .\src\InventoryManagement.Persistence.SqlServer\ -s .\src\InventoryManagement.WebApi\
#then
dotnet ef database update -p .\src\InventoryManagement.Persistence.SqlServer\ -s .\src\InventoryManagement.WebApi\
```

# How to install

* git pull this repository
* open terminal or cmd
* execute command "dotnet new install ."

![alt text][dotnetnewinstall]

* and then check already installed using "dotnet new list"

![alt text][dotnetnewlist]

## How to create API Endpoints

When you creating an endpoint, note that default naming convention using verb and adjective, for example when you want
to create an api,
for creating user

`CreateUser.cs`

this class will inherit either `BaseEndpoint` or `BaseEndpointWithoutRequest` and it looks like this

``` csharp
public class CreateUser : BaseEndpointWithoutResponse<CreateUserRequest>
```

The implementation of `BaseEndPoint` support so many cases, with or without request or response it looks like these

``` csharp
[Route("api/[namespace]")]
public abstract class BaseEndpoint<TReq, TRes> : EndpointBaseAsync.WithRequest<TReq>.WithActionResult<TRes>
{
}

[Route("api/[namespace]")]
public abstract class BaseEndpointWithoutResponse<TReq> : EndpointBaseAsync.WithRequest<TReq>.WithActionResult
{
}

[Route("api/[namespace]")]
public abstract class BaseEndpointWithoutRequest<TRes> : EndpointBaseAsync.WithoutRequest.WithActionResult<TRes>
{
}


[Route("api/[namespace]")]
public abstract class BaseEndpoint : EndpointBaseAsync.WithoutRequest.WithActionResult
{
}
```

Naming class with suffix `Request`, meaning that class is Data Transfer Object for example

``` csharp
public class CreateUserRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Fullname { get; set; }
    public string? Role { get; set; }
    public string? EmailAddress { get; set; }
}
```

Naming class for validator add suffix `Validator`, after request class

``` csharp
public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(e => e.Username).NotNull().NotEmpty().MaximumLength(256).SetValidator(new NonUnicodeOnlyValidator());
        RuleFor(e => e.Password).NotNull().NotEmpty().MaximumLength(256);
        RuleFor(e => e.Fullname).NotNull().NotEmpty().MaximumLength(256).SetValidator(new NonUnicodeOnlyValidator());
        RuleFor(e => e.Role).NotNull().NotEmpty().MaximumLength(256);
        RuleFor(e => e.EmailAddress).NotEmpty().MaximumLength(256).EmailAddress();
    }
}
```

### How to create an Endpoint that has route value

Mostly this use case will come up when update, or get by id, and so on. Let`s say we want to create an update user API.

When you put routes value on template url like this

```csharp
[HttpPut("users/{userId}")]
```

Then in parameter request in handler method required this attribute `[FromRoute]`, then inside request model may looks
like this

```csharp
public class UpdateUserRequest
{
    public UpdateUserRequest()
    {
        UpdateUserRequestPayload = new UpdateUserRequestPayload();
    }

    [FromRoute(Name = "userId")] public Guid UserId { get; set; }
    [FromBody] public UpdateUserRequestPayload UpdateUserRequestPayload { get; set; }
}

public class UpdateUserRequestPayload
{
    public string? Fullname { get; set; }
}
```

As you can see, that there is another child request object, we add suffix `Payload`, so that validator will just created
from child object

```csharp
public class UpdateUserRequestPayloadValidator : AbstractValidator<UpdateUserRequestPayload>
{
    public UpdateUserRequestPayloadValidator()
    {
        RuleFor(e => e.Fullname).NotNull().NotEmpty();
    }
}
```

## Endpoint Namespace and Scopes

Creating an endpoint group must under folder `Endpoints`, and naming folder must *<b>Singular</b>*, example like

1. UserManagement
2. RoleManagement
3. Identity

After creating a group endpoint, there are must scopes for security reason unless special case like 
`Identity`, most common there might be 2 scopes for example in `UserManagement`.

1. `usermanagementscope` given permission to get and edit verb
2. `usermanaegmentscope.readonly` given permission only for get api

naming convention for scopes is add suffix `scope` after it's `namespace`,

If there is another use-case that required unique scope, just create one and do not forget that class must inherit
`IScope`

[1]: https://dotnet.microsoft.com/en-us/download/dotnet/7.0

[infra]: https://github.com/radyalabs/dotnet-api-template/blob/main/.github/images/infra.drawio.png "Project Infrastructure"

[bogus]: https://www.nuget.org/packages/Bogus

[moq]: https://www.nuget.org/packages/Moq

[mediator1]: https://www.nuget.org/packages/Mediator.Abstractions/2.1.1

[mediator2]: https://www.nuget.org/packages/Mediator.SourceGenerator/2.1.1

[linqdynamiccore]: https://www.nuget.org/packages/System.Linq.Dynamic.Core

[fluentvalidation]: https://www.nuget.org/packages/FluentValidation

[jil]: https://www.nuget.org/packages/Jil/2.17.0

[serilog]: https://www.nuget.org/packages/Serilog/2.12.0

[appsettings]: https://github.com/radyalabs/dotnet-api-template/blob/main/src/InventoryManagement.WebApi/appsettings.json

[dotnetnewinstall]: https://github.com/radyalabs/dotnet-api-template/blob/main/.github/images/dotnetnewinstall.PNG "cmd install"

[dotnetnewlist]: https://github.com/radyalabs/dotnet-api-template/blob/main/.github/images/dotnetnewlist.PNG "new list"

[dotnetefinstall]: https://learn.microsoft.com/en-us/ef/core/cli/dotnet#installing-the-tools

[projectfolderstructure]: https://github.com/radyalabs/dotnet-api-template/blob/main/.github/images/project-structure.png "Project Folder Infrastructure"