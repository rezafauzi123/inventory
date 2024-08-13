using InventoryManagement.WebApi.Validators;
using FluentValidation;

namespace InventoryManagement.WebApi.Endpoints.Book.Requests;

public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(e => e.CategoryId).NotNull();
        RuleFor(e => e.AuthorId).NotNull();
        RuleFor(e => e.PublisherId).NotNull();
        RuleFor(e => e.Title).NotNull().NotEmpty().MaximumLength(100).SetValidator(new AsciiOnlyValidator());
        RuleFor(e => e.Year).NotNull();
        RuleFor(e => e.Pages).NotNull();
        RuleFor(e => e.PublishedDate).NotNull();
        RuleFor(e => e.Isbn).NotNull().NotEmpty().MaximumLength(20);
        RuleFor(e => e.Dimensions).NotNull();
        RuleFor(e => e.Weight).NotNull();
        RuleFor(e => e.Price).NotNull();
        RuleFor(e => e.Language).NotNull().NotEmpty();
    }
}