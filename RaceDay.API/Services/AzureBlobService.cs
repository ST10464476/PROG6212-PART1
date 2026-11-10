using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace RaceDay.API.Services
{
    public class AzureBlobService
    {
        private readonly BlobContainerClient _containerClient;

        public AzureBlobService(IConfiguration configuration)
        {
            string connectionString =
                configuration["AzureStorage:ConnectionString"]
                ?? throw new InvalidOperationException(
                    "Azure Storage connection string is missing.");

            string containerName =
                configuration["AzureStorage:ContainerName"]
                ?? "raceday-images";

            var blobServiceClient =
                new BlobServiceClient(connectionString);

            _containerClient =
                blobServiceClient.GetBlobContainerClient(containerName);
        }

        public async Task<string> UploadImageAsync(
            Stream fileStream,
            string fileName,
            string contentType)
        {
            await _containerClient.CreateIfNotExistsAsync(
                PublicAccessType.None);

            string blobName =
                $"{Guid.NewGuid()}_{Path.GetFileName(fileName)}";

            BlobClient blobClient =
                _containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(
                fileStream,
                new BlobHttpHeaders
                {
                    ContentType = contentType
                });

            return blobName;
        }

        public async Task<string?> GetImageReadUrlAsync(string blobName)
        {
            BlobClient blobClient =
                _containerClient.GetBlobClient(blobName);

            if (!await blobClient.ExistsAsync())
                return null;

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerClient.Name,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(15)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }
    }
}