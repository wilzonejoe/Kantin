using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Core.Interface;
using Core.Models.File;
using Microsoft.Extensions.Configuration;

namespace Core.Helpers
{
    public class FileStorageHelper : IFileStorage<BlobContainerClient>
    {
        private readonly string _connectionString;

        public FileStorageHelper(IConfiguration configuration)
        {
            var credential = new AzureStorageCredential();
            configuration.GetSection("Azure").Bind(credential);
            _connectionString = credential.StorageConnectionString;
        }

        public async Task<BlobContainerClient> Connect(string containerName = "attachments")
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);

            var container = blobServiceClient.GetBlobContainerClient(containerName);

            if (container?.Exists() ?? false)
                return container;

            var containerClientResponse = await blobServiceClient.CreateBlobContainerAsync(containerName);
            return containerClientResponse?.Value;
        }

        public async Task<UploadResult> Upload(UploadFile uploadFile)
        {
            var containerClient = await Connect();

            if (containerClient == null)
                return new UploadResult
                {
                    Success = false
                };

            var blobName = $"{uploadFile.OrganisationId}/{uploadFile.AttachmentId}{uploadFile.FileName}";
            var blobClient = containerClient.GetBlobClient(blobName);
            var uri = blobClient.Uri;
            
            var uploadResponse =  await blobClient.UploadAsync(uploadFile.Data);
            var blobContentInfo = uploadResponse?.Value;

            if (blobContentInfo == null)
            {
                return new UploadResult
                {
                    Success = false
                };
            }
            else
            {
                return new UploadResult
                {
                    Success = true,
                    Uri = uri?.ToString()
                };
            }
        }

        public async Task<DownloadResult> Download(Guid organisationId, Guid attachmentId, string fileName = null)
        {
            var containerClient = await Connect();

            if (containerClient == null)
                return new DownloadResult
                {
                    Success = false
                };

            var blobName = $"{organisationId}/{attachmentId}{fileName ?? string.Empty}";
            var blobClient = containerClient.GetBlobClient(blobName);
            var download = await blobClient.DownloadAsync();
            var downloadResult = download?.Value;

            if (downloadResult == null)
            {
                return new DownloadResult
                {
                    Success = false
                };
            }
            else
            {
                return new DownloadResult
                {
                    Success = true,
                    Data = downloadResult.Content,
                    FileName = fileName
                };
            }
        }
    }
}
