using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using FitTracker.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace FitTracker.Infrastructure.Services;

public class BlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration) : IBlobStorageService
{
    private readonly string _containerName = configuration["AzureStorage:ContainerName"] ??
                                             throw new ArgumentNullException(
                                                 nameof(configuration),
                                                 "AzureStorage:ContainerName is not configured");

    /// <inheritdoc />
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var blobClient = containerClient.GetBlobClient(fileName);

        await using (var stream = file.OpenReadStream())
        {
            await blobClient.UploadAsync(stream, true);
        }

        return blobClient.Uri.AbsoluteUri;
    }

    /// <inheritdoc />
    public async Task DeleteFileAsync(string fileUrl)
    {
        var uri = new Uri(fileUrl);
        var blobName = Path.GetFileName(uri.LocalPath);

        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync();
    }
}
