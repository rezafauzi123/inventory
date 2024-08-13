using InventoryManagement.WebApi.Validators;
using FluentValidation;

namespace InventoryManagement.WebApi.Endpoints.Publisher.Requests;

public class CreatePublisherRequestValidator : AbstractValidator<CreatePublisherRequest>
{
    public CreatePublisherRequestValidator()
    {
        RuleFor(e => e.Name).NotNull().NotEmpty().MaximumLength(100).SetValidator(new AsciiOnlyValidator());
        RuleFor(e => e.Phone).NotNull().NotEmpty().MaximumLength(20).SetValidator(new AsciiOnlyValidator());
        RuleFor(e => e.Email).NotNull().NotEmpty().MaximumLength(50).EmailAddress();
        RuleFor(e => e.Address).MaximumLength(512);
    }
}