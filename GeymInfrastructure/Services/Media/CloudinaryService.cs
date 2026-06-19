using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GymManagement.Domain.Common;
using GymManagement.Domain.Services.Members;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GymManagement.Infrastructure.Services.Media;

internal sealed class CloudinaryService : IImageService
{
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; 

    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

    private readonly Cloudinary _cloudinary;
    private readonly ILogger<CloudinaryService> _logger;

    public CloudinaryService(IConfiguration configuration, ILogger<CloudinaryService> logger)
    {
        _logger = logger;

        var cloudName = configuration["Cloudinary:CloudName"]
            ?? throw new ArgumentNullException(nameof(configuration), "Cloudinary:CloudName is not configured.");
        var apiKey = configuration["Cloudinary:ApiKey"]
            ?? throw new ArgumentNullException(nameof(configuration), "Cloudinary:ApiKey is not configured.");
        var apiSecret = configuration["Cloudinary:ApiSecret"]
            ?? throw new ArgumentNullException(nameof(configuration), "Cloudinary:ApiSecret is not configured.");

        var account = new Account(cloudName, apiKey, apiSecret);
        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string folder)
    {
        if (fileStream is null || fileStream.Length == 0)
        {
            return Result<string>.Failure("Please select a file to upload.", "Photo");
        }

        if (fileStream.Length > MaxFileSizeBytes)
        {
            return Result<string>.Failure("File size must not exceed 5 MB.", "Photo");
        }

        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
        {
            return Result<string>.Failure(
                "Only JPG, JPEG, PNG, and WEBP files are allowed.", "Photo");
        }

        try
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = folder,
                Transformation = new Transformation()
                    .Width(400)
                    .Height(400)
                    .Crop("fill")
                    .Gravity("face")
                    .Quality("auto")
                    .FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError(
                    "Cloudinary upload failed for file {FileName}. Status: {Status}, Error: {Error}",
                    fileName, uploadResult.StatusCode, uploadResult.Error?.Message);

                return Result<string>.Failure(
                    "An error occurred while uploading the image. Please try again.", "Photo");
            }

            _logger.LogInformation(
                "Image uploaded successfully. PublicId: {PublicId}", uploadResult.PublicId);

            return Result<string>.Success(uploadResult.SecureUrl.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error uploading image {FileName}", fileName);

            return Result<string>.Failure(
                "An error occurred while uploading the image. Please try again.", "Photo");
        }
    }

    public async Task<Result> DeleteAsync(string publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId))
        {
            return Result.Failure("Image identifier is required.", "Photo");
        }

        try
        {
            var deleteParams = new DeletionParams(publicId);
            var deleteResult = await _cloudinary.DestroyAsync(deleteParams);

            if (deleteResult.Result != "ok")
            {
                _logger.LogError(
                    "Cloudinary delete failed for PublicId {PublicId}. Result: {Result}",
                    publicId, deleteResult.Result);

                return Result.Failure(
                    "An error occurred while deleting the image. Please try again.", "Photo");
            }

            _logger.LogInformation("Image deleted successfully. PublicId: {PublicId}", publicId);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting image with PublicId {PublicId}", publicId);

            return Result.Failure(
                "An error occurred while deleting the image. Please try again.", "Photo");
        }
    }
}
