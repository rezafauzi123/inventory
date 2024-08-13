using InventoryManagement.WebApi.Validators;
using FluentValidation;

namespace InventoryManagement.WebApi.Endpoints.Category.Requests;

public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(e => e.Name).NotNull().NotEmpty().MaximumLength(100).SetValidator(new AsciiOnlyValidator());
    }
}