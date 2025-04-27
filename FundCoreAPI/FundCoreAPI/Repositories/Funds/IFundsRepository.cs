namespace FundCoreAPI.Repositories.Funds
{
    using FundCoreAPI.Models;

    /// <summary>
    /// Interface for managing fund operations in the repository.
    /// </summary>
    public interface IFundsRepository
    {
        /// <summary>
        /// Asynchronously creates a new fund in the repository.
        /// </summary>
        /// <param name="fund">The fund entity to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateFundAsync(Fund fund);

        /// <summary>
        /// Asynchronously retrieves a fund by its unique identifier.
        /// </summary>
        /// <param name="fundId">The unique identifier of the fund.</param>
        /// <returns>A task representing the asynchronous operation, containing the fund if found, or null otherwise.</returns>
        Task<Fund?> GetFundByIdAsync(int fundId);

        /// <summary>
        /// Asynchronously retrieves all funds from the repository.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all funds.</returns>
        Task<List<Fund>> GetAllFundsAsync();

        /// <summary>
        /// Asynchronously updates an existing fund in the repository.
        /// </summary>
        /// <param name="fund">The fund entity with updated information.</param>
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
