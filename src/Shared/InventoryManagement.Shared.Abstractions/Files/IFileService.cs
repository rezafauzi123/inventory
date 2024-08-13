namespace InventoryManagement.Shared.Abstractions.Files;

public interface IFileService
{
    Task<FileResponse> UploadAsync(FileRequest request, CancellationToken cancellationToken);

    Task<FileDownloadResponse?> DownloadAsync(string fileName, CancellationToken cancellationToken);

    Task<bool> IsFileExistAsync(string fileName, CancellationToken cancellationToken);

    public Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken);
}