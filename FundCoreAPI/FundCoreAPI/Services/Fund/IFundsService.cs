namespace FundCoreAPI.Services.Funds
{
    using FundCoreAPI.Models;

    public interface IFundsService
    {
        Task CreateFundAsync(Fund fund);
        Task<Fund?> GetFundByIdAsync(int fundId);
        Task<List<Fund>> GetAllFundsAsync();
        Task UpdateFundAsync(Fund fund);
        Task DeleteFundAsync(int fundId);
    }
}
