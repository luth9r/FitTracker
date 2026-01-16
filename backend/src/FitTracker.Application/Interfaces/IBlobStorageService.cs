using Microsoft.AspNetCore.Http;

namespace FitTracker.Application.Interfaces;

/// <summary>
///     Provides an abstraction for interacting with a blob storage service.
/// </summary>
public interface IBlobStorageService
{
    /// <summary>
    ///     Uploads a file to the blob storage asynchronously and returns the URL of the uploaded file.
    /// </summary>
    /// <param name="file">The file to be uploaded.</param>
    /// <returns>The task result contains the URL of the uploaded file.</returns>
    Task<string> UploadFileAsync(IFormFile file);

    /// <summary>
    ///     Deletes a file from the blob storage asynchronously.
    /// </summary>
    /// <param name="fileUrl">The URL of the file to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteFileAsync(string fileUrl);
}
