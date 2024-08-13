using InventoryManagement.WebApi.Validators;
using FluentValidation;

namespace InventoryManagement.WebApi.Endpoints.Publisher.Requests;

public class EditPublisherRequestPayloadValidator : AbstractValidator<EditPublisherRequestPayload>
{
    public EditPublisherRequestPayloadValidator()
    {
        RuleFor(e => e.Name).NotNull();

        When(e => !string.IsNullOrWhiteSpace(e.Name),
            () => { RuleFor(e => e.Name).SetValidator(new AsciiOnlyValidator()!).MaximumLength(100); });

        }
}