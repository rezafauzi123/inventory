using InventoryManagement.WebApi.Validators;
using FluentValidation;

namespace InventoryManagement.WebApi.Endpoints.Inventory.Requests;

public class ReduceInventoryRequestValidator : AbstractValidator<ReduceInventoryRequest>
{
    public ReduceInventoryRequestValidator()
    {
        RuleFor(e => e.BookId).NotNull();
    }
}