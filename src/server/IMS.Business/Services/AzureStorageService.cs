using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace IMS.Business.Services;

public class AzureStorageService
{
    private readonly BlobContainerClient _containerClient;

    public AzureStorageService(IConfiguration configuration)
    {
        string connectionString = configuration["AzureStorage:ConnectionString"]
            ?? throw new InvalidOperationException("Azure Storage connection string is not configured.");

        string containerName = configuration["AzureStorage:ContainerName"]
            ?? throw new InvalidOperationException("Azure Storage container name is not configured.");
        _containerClient = new BlobContainerClient(connectionString, containerName);
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        BlobClient blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, true);
        return blobClient.Uri.ToString(); 
    }

    public  async Task<Stream> DownloadFileFromUrlAsync(string fileUrl)
    {
        Uri fileUri = new Uri(fileUrl);
        BlobClient blobClient = new BlobClient(fileUri);

        try
        {
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error downloading the file from Azure Blob Storage.", ex);
        }
    }

    public async Task<IEnumerable<string>> GetAllFilesAsync()
    {
        try
        {
            var blobs = _containerClient.GetBlobsAsync();

            var fileNames = new List<string>();
            await foreach (var blobItem in blobs)
            {
                fileNames.Add(blobItem.Name);
            }

            return fileNames;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error retrieving file list from Azure Blob Storage.", ex);
        }
    }



    public async Task<bool> CheckConnectionAsync()
    {
        try
        {
            await _containerClient.ExistsAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    internal string? GetFileUrl(string fileName)
    {
        BlobClient blobClient = _containerClient.GetBlobClient(fileName);
        return blobClient.Uri.ToString();
    }
}