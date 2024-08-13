using InventoryManagement.WebApi.Validators;
using FluentValidation;

namespace InventoryManagement.WebApi.Endpoints.Author.Requests;

public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
{
    public CreateAuthorRequestValidator()
    {
        RuleFor(e => e.Name).NotNull().NotEmpty().MaximumLength(100).SetValidator(new AsciiOnlyValidator());
        RuleFor(e => e.Biography).NotNull().NotEmpty().MaximumLength(512).SetValidator(new AsciiOnlyValidator());
    }
}