using FluentValidation;
using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.WebApi.Endpoints.Inventory.Requests;
using InventoryManagement.WebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Inventory;

public class ReduceInventory : BaseEndpointWithoutResponse<ReduceInventoryRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IInventoryService _inventoryService;
    private readonly IRng _rng;
    private readonly ISalter _salter;
    private readonly IStringLocalizer<ReduceInventory> _localizer;

    public ReduceInventory(IDbContext dbContext,
        IInventoryService InventoryService,
        IRng rng,
        ISalter salter,
        IStringLocalizer<ReduceInventory> localizer)
    {
        _dbContext = dbContext;
        _inventoryService = InventoryService;
        _rng = rng;
        _salter = salter;
        _localizer = localizer;
    }

    [HttpPost("reduce")]
    //[Authorize]
    //[RequiredScope(typeof(InventoryScope))]
    [SwaggerOperation(
        Summary = "Reduce Inventory",
        Description = "",
        OperationId = "Inventory.ReduceInventory",
        Tags = new[] { "Inventory" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(ReduceInventoryRequest request,
        CancellationToken cancellationToken = new())
    {
        try
        {
            var validator = new ReduceInventoryRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest(Error.Create(_localizer["invalid-parameter"], validationResult.Construct()));

            if (request.Qty < 0)
                return BadRequest(Error.Create(_localizer["invalid-parameter"]));

            var existingInventory = await _inventoryService.GetByBookIdAsync(request.BookId, cancellationToken);
            if (existingInventory == null)
                return BadRequest(Error.Create(_localizer["data-not-found"]));

            if (existingInventory.Stock < 0)
                return BadRequest(Error.Create(_localizer["invalid-parameter"]));

            _dbContext.AttachEntity(existingInventory);

            existingInventory.BookId = request.BookId;
            existingInventory.Stock -= request.Qty;

            var transactionHistory = new Domain.Entities.TransactionHistory
            {
                InventoryId = existingInventory.Id,
                Qty = request.Qty,
                TransactionDate = DateTime.UtcNow,
                TransactionType = (int)TransactionType.Out,
            };

            await _dbContext.InsertAsync(transactionHistory, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        catch (Exception ex)
        {
            throw new(ex.InnerException!.Message);
        }
    }
}