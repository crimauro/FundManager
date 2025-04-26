namespace FundCoreAPI.Repositories.Funds
{
    using FundCoreAPI.Models;

    public interface IFundsRepository
    {
        Task CreateFundAsync(Fund fund);
        Task<Fund?> GetFundByIdAsync(int fundId);
        Task<List<Fund>> GetAllFundsAsync();
        Task UpdateFundAsync(Fund fund);
        Task DeleteFundAsync(int fundId);
    }
}
