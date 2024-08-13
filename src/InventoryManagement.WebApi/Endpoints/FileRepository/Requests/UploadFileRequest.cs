namespace InventoryManagement.WebApi.Endpoints.FileRepository.Requests;

public class UploadFileRequest
{
    public IFormFile File { get; set; } = null!;
    public string Source { get; set; } = null!;
}