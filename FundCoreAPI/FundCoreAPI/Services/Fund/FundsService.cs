namespace FundCoreAPI.Services.Funds
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Repositories.Funds;

    public class FundsService : IFundsService
    {
        private readonly IFundsRepository _fundsRepository;

        public FundsService(IFundsRepository fundsRepository)
        {
            _fundsRepository = fundsRepository;
        }

        public async Task CreateFundAsync(Fund fund)
        {
            await _fundsRepository.CreateFundAsync(fund);
        }

        public async Task<Fund?> GetFundByIdAsync(int fundId)
        {
            return await _fundsRepository.GetFundByIdAsync(fundId);
        }

        public async Task<List<Fund>> GetAllFundsAsync()
        {
            return await _fundsRepository.GetAllFundsAsync();
        }

        public async Task UpdateFundAsync(Fund fund)
        {
            await _fundsRepository.UpdateFundAsync(fund);
        }

        public async Task DeleteFundAsync(int fundId)
        {
            await _fundsRepository.DeleteFundAsync(fundId);
        }
    }
}
