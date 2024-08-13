using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Files;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Endpoints.FileRepository.Requests;
using InventoryManagement.WebApi.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.FileRepository;

public class UploadFile : BaseEndpoint<UploadFileRequest, UploadFileResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IFileService _fileService;

    public UploadFile(IDbContext dbContext, IFileService fileService)
    {
        _dbContext = dbContext;
        _fileService = fileService;
    }

    [HttpPost]
    [Authorize]
    [SwaggerOperation(
        Summary = "File repository upload",
        Description = "",
        OperationId = "FileRepository.Upload",
        Tags = new[] { "FileRepository" })
    ]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public override async Task<ActionResult<UploadFileResponse>> HandleAsync([FromForm] UploadFileRequest request,
        CancellationToken cancellationToken = new())
    {
        var fileResponse = await _fileService.UploadAsync(
            new FileRequest(request.File.FileName, request.File.OpenReadStream()),
            cancellationToken);

        //default value of fileRepository FileStoreAt is filesystem,
        //then by default, if the implementation is change, you must update this and add
        //FileStoreAt into something that is related to the current implementations
        //such as FileStoreAt.AzureBlob next to Source = request.Source
        var fileRepository = request.ToFileRepository(fileResponse);
        //fileRepository.FileStoreAt = FileStoreAt.AzureBlob;

        await _dbContext.InsertAsync(fileRepository, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UploadFileResponse { FileId = fileRepository.FileRepositoryId };
    }
}