using Back.DTO.Admin;

namespace Back.Services.Admin
{
    public interface IAdminService
    {
        Task<AdminStatsResponse> GetAdminStatsAsync();
        Task AddGameAsync(AddGameRequest request);
    }
}