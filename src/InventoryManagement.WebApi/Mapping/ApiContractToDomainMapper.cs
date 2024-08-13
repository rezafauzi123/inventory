using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Extensions;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.Shared.Abstractions.Files;
using InventoryManagement.WebApi.Endpoints.Author.Requests;
using InventoryManagement.WebApi.Endpoints.Book.Requests;
using InventoryManagement.WebApi.Endpoints.Category.Requests;
using InventoryManagement.WebApi.Endpoints.FileRepository.Requests;
using InventoryManagement.WebApi.Endpoints.Publisher.Requests;
using InventoryManagement.WebApi.Endpoints.RoleManagement.Requests;
using InventoryManagement.WebApi.Endpoints.UserManagement.Requests;

namespace InventoryManagement.WebApi.Mapping;

public static class ApiContractToDomainMapper
{
    public static FileRepository ToFileRepository(this UploadFileRequest request, FileResponse fileResponse)
    {
        var fileRepository = new FileRepository
        {
            FileName = request.File.FileName,
            UniqueFileName = fileResponse.NewFileName,
            Source = request.Source,
            Size = request.File.Length,
            FileExtension = Path.GetExtension(request.File.FileName).ToUpper(),
        };

        if (FileRepositoryExtensions.ListOfFileTypeImages.Any(e => e == fileRepository.FileExtension))
            fileRepository.FileType = FileType.Images;

        if (FileRepositoryExtensions.ListOfFileTypeDocuments.Any(e => e == fileRepository.FileExtension))
            fileRepository.FileType = FileType.Document;

        return fileRepository;
    }

    public static Role ToRole(this CreateRoleRequest request)
    {
        var role = new Role
        {
            Name = request.Name!,
            Description = request.Description,
        };

        role.Code = RoleExtensions.Slug(role.RoleId, role.Name);

        if (request.Scopes.Any())
            foreach (var item in request.Scopes)
                role.RoleScopes.Add(new RoleScope
                {
                    RoleId = role.RoleId,
                    Name = item
                });

        return role;
    }

    public static User ToUser(this CreateUserRequest request, string salt, ISalter salter)
    {
        var user = new User
        {
            Username = request.Username!,
            NormalizedUsername = request.Username!.ToUpper(),
            Salt = salt,
            Password = salter.Hash(salt, request.Password!),
            LastPasswordChangeAt = DateTime.UtcNow,
            FullName = request.Fullname
        };

        user.UserRoles.Add(
            new UserRole
            {
                RoleId = request.RoleId!.Value
            });

        return user;
    }

    public static Category ToCategory(this CreateCategoryRequest request, string salt, ISalter salter)
    {
        var bookCategory = new Category
        {
            Name = request.Name!,
        };

        return bookCategory;
    }

    public static Publisher ToPublisher(this CreatePublisherRequest request, string salt, ISalter salter)
    {
        var publisher = new Publisher
        {
            Name = request.Name!,
            Phone = request.Phone!,
            Email = request.Email!,
            Address = request.Address,
        };

        return publisher;
    }

    public static Author ToAuthor(this CreateAuthorRequest request, string salt, ISalter salter)
    {
        var author = new Author
        {
            Name = request.Name!,
            Biography = request.Biography!
        };

        return author;
    }

    public static Book ToBook(this CreateBookRequest request, string salt, ISalter salter)
    {
        var book = new Book
        {
            AuthorId = request.AuthorId!.Value,
            CategoryId = request.CategoryId!.Value,
            PublisherId = request.PublisherId!.Value,
            Title = request.Title!,
            Year = request.Year!.Value,
            Pages = request.Pages!.Value,
            Description = request.Description!,
            PublishedDate = request.PublishedDate!.Value,
            Isbn = request.Isbn!,
            Dimensions = request.Dimensions!,
            Weight = request.Weight!.Value,
            Price = request.Price!.Value,
            Language = request.Language!
        };

        return book;
    }
}