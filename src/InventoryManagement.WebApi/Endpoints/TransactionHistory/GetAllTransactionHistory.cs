using InventoryManagement.Domain.Enums;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Endpoints.TransactionHistory.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace InventoryManagement.WebApi.Endpoints.TransactionHistory;

public class GetAllTransactionHistory : BaseEndpoint<GetAllTransactionHistoryRequest, List<TransactionHistoryResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllTransactionHistory(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    //[Authorize]
    //[RequiredScope(typeof(TransactionHistoryScope), typeof(TransactionHistoryScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get TransactionHistory",
        Description = "",
        OperationId = "TransactionHistory.GetAllTransactionHistory",
        Tags = new[] { "TransactionHistory" })
    ]
    [ProducesResponseType(typeof(List<TransactionHistoryResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<List<TransactionHistoryResponse>>> HandleAsync([FromQuery] GetAllTransactionHistoryRequest request,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Domain.Entities.TransactionHistory>()
            .AsQueryable();

        //if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
        //{
        //    queryable = queryable.Where(e => EF.Functions.Like(e.Book!.Title, $"%{request.Search}%"));
        //}

        var data = await queryable
            .Select(e => new TransactionHistoryResponse
            {
                Id = e.Id,
                InventoryId = e.InventoryId,
                TransactionDate = e.TransactionDate,
                TransactionType = e.TransactionType == (int)TransactionType.In? TransactionType.In.ToString(): TransactionType.Out.ToString(),
                Qty = e.Qty,
            })
            .ToListAsync(cancellationToken);

        return data;
    }
}