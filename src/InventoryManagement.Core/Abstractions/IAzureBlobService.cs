using InventoryManagement.Core.Models;

namespace InventoryManagement.Core.Abstractions;

/// <summary>
/// Represents a service for interacting with Azure Blob Storage.
/// </summary>
public interface IAzureBlobService
{
    /// <summary>
    /// Uploads a stream to the Azure Blob Storage asynchronously.
    /// </summary>
    /// <param name="stream">The stream to be uploaded.</param>
    /// <param name="containerName">The name of the container in Azure Blob Storage.</param>
    /// <param name="fileName">The name of the file in Azure Blob Storage.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the upload operation.</param>
    /// <returns>A task that represents the asynchronous upload operation with the response of type AzureBlobUploadResponse.</returns>
    Task<AzureBlobUploadResponse> UploadAsync(Stream stream, string containerName, string fileName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Generate a URI for accessing a specific file in an Azure Blob Storage container asynchronously.
    /// </summary>
    /// <param name="containerName">The name of the container in Azure Blob Storage.</param>
    /// <param name="filename">The name of the file in the container.</param>
    /// <param name="cancellationToken">Optional cancellation token to cancel the asynchronous operation.</param>
    /// <returns>An asynchronous task that returns an instance of AzureBlobUriResponse, which contains the generated URI for the file.</returns>
    Task<AzureBlobUriResponse> GenerateUriAsync(string containerName, string filename,
        CancellationToken cancellationToken = default);
}