using GymManagement.Domain.Common;

namespace GymManagement.Domain.Services.Members;

public interface IImageService
{
    Task<Result<string>> UploadAsync(Stream fileStream, string fileName, string folder);
    Task<Result> DeleteAsync(string publicId);
}