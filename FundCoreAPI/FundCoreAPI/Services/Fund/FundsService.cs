namespace FundCoreAPI.Services.Funds
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Repositories.Funds;

    /// <summary>
    /// Service class for managing fund operations.
    /// </summary>
    public class FundsService : IFundsService
    {
        private readonly IFundsRepository _fundsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundsService"/> class.
        /// </summary>
        /// <param name="fundsRepository">The repository for fund data access.</param>
        public FundsService(IFundsRepository fundsRepository)
        {
            _fundsRepository = fundsRepository;
        }

        /// <summary>
        /// Creates a new fund asynchronously.
        /// </summary>
        /// <param name="fund">The fund to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateFundAsync(Fund fund)
        {
            await _fundsRepository.CreateFundAsync(fund);
        }

        /// <summary>
        /// Retrieves a fund by its identifier asynchronously.
        /// </summary>
        /// <param name="fundId">The identifier of the fund.</param>
        /// <returns>A task representing the asynchronous operation, containing the fund if found.</returns>
        public async Task<Fund?> GetFundByIdAsync(int fundId)
        {
            return await _fundsRepository.GetFundByIdAsync(fundId);
        }

        /// <summary>
        /// Retrieves all funds asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all funds.</returns>
        public async Task<List<Fund>> GetAllFundsAsync()
        {
            return await _fundsRepository.GetAllFundsAsync();
        }

        /// <summary>
        /// Updates an existing fund asynchronously.
        /// </summary>
        /// <param name="fund">The fund to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateFundAsync(Fund fund)
        {
            await _fundsRepository.UpdateFundAsync(fund);
        }

        /// <summary>
        /// Deletes a fund by its identifier asynchronously.
        /// </summary>
        /// <param name="fundId">The identifier of the fund to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteFundAsync(int fundId)
        {
            await _fundsRepository.DeleteFundAsync(fundId);
        }
    }
}
