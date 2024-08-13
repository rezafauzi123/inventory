using InventoryManagement.WebApi.Validators;
using FluentValidation;

namespace InventoryManagement.WebApi.Endpoints.Author.Requests;

public class EditAuthorRequestPayloadValidator : AbstractValidator<EditAuthorRequestPayload>
{
    public EditAuthorRequestPayloadValidator()
    {
        RuleFor(e => e.Name).NotNull();

        When(e => !string.IsNullOrWhiteSpace(e.Name),
            () => { RuleFor(e => e.Name).SetValidator(new AsciiOnlyValidator()!).MaximumLength(100); });

        }
}