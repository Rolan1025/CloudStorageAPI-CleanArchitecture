namespace CloudStorageAPICleanArchitecture.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GenerateJwtTokenAsync(string apiKey);
    }
}
