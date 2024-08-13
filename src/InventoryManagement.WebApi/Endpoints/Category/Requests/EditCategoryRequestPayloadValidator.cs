using InventoryManagement.WebApi.Validators;
using FluentValidation;

namespace InventoryManagement.WebApi.Endpoints.Category.Requests;

public class EditCategoryRequestPayloadValidator : AbstractValidator<EditCategoryRequestPayload>
{
    public EditCategoryRequestPayloadValidator()
    {
        RuleFor(e => e.Name).NotNull();

        When(e => !string.IsNullOrWhiteSpace(e.Name),
            () => { RuleFor(e => e.Name).SetValidator(new AsciiOnlyValidator()!).MaximumLength(100); });

    }
}