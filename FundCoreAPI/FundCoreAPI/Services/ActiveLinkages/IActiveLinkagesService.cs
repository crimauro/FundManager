namespace FundCoreAPI.Services.ActiveLinkages
{
    using FundCoreAPI.Models;

    /// <summary>
    /// Interface for managing active linkages in the system.
    /// </summary>
    public interface IActiveLinkagesService
    {
        /// <summary>
        /// Creates a new linkage asynchronously.
        /// </summary>
        /// <param name="linkage">The linkage to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateLinkageAsync(ActiveLinkages linkage);

        /// <summary>
        /// Retrieves a linkage by customer ID and fund ID asynchronously.
        /// </summary>
        /// <param name="customerId">The customer ID associated with the linkage.</param>
        /// <param name="fundId">The fund ID associated with the linkage.</param>
        /// <returns>A task representing the asynchronous operation, containing the linkage if found.</returns>
        Task<ActiveLinkages?> GetLinkageByIdAsync(string customerId, int fundId);

        /// <summary>
        /// Retrieves all linkages for a specific customer asynchronously.
        /// </summary>
        /// <param name="customerId">The customer ID to filter linkages.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of linkages.</returns>
        Task<List<ActiveLinkages>> GetAllLinkagesAsync(string customerId);

        /// <summary>
        /// Retrieves linkages by category for a specific customer asynchronously.
        /// </summary>
        /// <param name="customerId">The customer ID to filter linkages.</param>
        /// <param name="category">The category to filter linkages.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of linkages.</returns>
        Task<List<ActiveLinkages>> GetLinkagesByCategoryAsync(string customerId, string category);

        /// <summary>
        /// Deletes a linkage by customer ID and fund ID asynchronously.
        /// </summary>
        /// <param name="customerId">The customer ID associated with the linkage.</param>
        /// <param name="fundId">The fund ID associated with the linkage.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteLinkageAsync(string customerId, int fundId);
    }
}
