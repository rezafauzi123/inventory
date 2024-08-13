using InventoryManagement.WebApi.Validators;
using FluentValidation;

namespace InventoryManagement.WebApi.Endpoints.Inventory.Requests;

public class AddInventoryRequestValidator : AbstractValidator<AddInventoryRequest>
{
    public AddInventoryRequestValidator()
    {
        RuleFor(e => e.BookId).NotNull();
    }
}