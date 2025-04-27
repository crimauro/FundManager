namespace FundCoreAPI.Services.Funds
{
    using FundCoreAPI.Models;

    /// <summary>
    /// Interface for managing fund operations.
    /// </summary>
    public interface IFundsService
    {
        /// <summary>
        /// Asynchronously creates a new fund.
        /// </summary>
        /// <param name="fund">The fund to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateFundAsync(Fund fund);

        /// <summary>
        /// Asynchronously retrieves a fund by its unique identifier.
        /// </summary>
        /// <param name="fundId">The unique identifier of the fund.</param>
        /// <returns>A task representing the asynchronous operation, containing the fund if found, or null otherwise.</returns>
        Task<Fund?> GetFundByIdAsync(int fundId);

        /// <summary>
        /// Asynchronously retrieves all funds.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all funds.</returns>
        Task<List<Fund>> GetAllFundsAsync();

        /// <summary>
        /// Asynchronously updates an existing fund.
        /// </summary>
        /// <param name="fund">The fund to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateFundAsync(Fund fund);

        /// <summary>
        /// Asynchronously deletes a fund by its unique identifier.
        /// </summary>
        /// <param name="fundId">The unique identifier of the fund to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteFundAsync(int fundId);
    }
}
