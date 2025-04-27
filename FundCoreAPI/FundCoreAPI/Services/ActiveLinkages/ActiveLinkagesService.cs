namespace FundCoreAPI.Services.ActiveLinkages
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Repositories.ActiveLinkages;

    /// <summary>
    /// Service for managing active linkages.
    /// </summary>
    public class ActiveLinkagesService : IActiveLinkagesService
    {
        private readonly IActiveLinkagesRepository _activeLinkagesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveLinkagesService"/> class.
        /// </summary>
        /// <param name="activeLinkagesRepository">The repository for active linkages.</param>
        public ActiveLinkagesService(IActiveLinkagesRepository activeLinkagesRepository)
        {
            _activeLinkagesRepository = activeLinkagesRepository;
        }

        /// <summary>
        /// Creates a new linkage asynchronously.
        /// </summary>
        /// <param name="linkage">The linkage to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateLinkageAsync(ActiveLinkages linkage)
        {
            await _activeLinkagesRepository.CreateLinkageAsync(linkage);
        }

        /// <summary>
        /// Retrieves a linkage by customer ID and fund ID asynchronously.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="fundId">The fund ID.</param>
        /// <returns>A task representing the asynchronous operation, containing the linkage if found.</returns>
        public async Task<ActiveLinkages?> GetLinkageByIdAsync(string customerId, int fundId)
        {
            return await _activeLinkagesRepository.GetLinkageByIdAsync(customerId, fundId);
        }

        /// <summary>
        /// Retrieves all linkages for a specific customer asynchronously.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of linkages.</returns>
        public async Task<List<ActiveLinkages>> GetAllLinkagesAsync(string customerId)
        {
            return await _activeLinkagesRepository.GetAllLinkagesAsync(customerId);
        }

        /// <summary>
        /// Retrieves linkages by category for a specific customer asynchronously.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="category">The category of the linkage.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of linkages.</returns>
        public async Task<List<ActiveLinkages>> GetLinkagesByCategoryAsync(string customerId, string category)
        {
            return await _activeLinkagesRepository.GetLinkagesByCategoryAsync(customerId, category);
        }

        /// <summary>
        /// Deletes a linkage by customer ID and fund ID asynchronously.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="fundId">The fund ID.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteLinkageAsync(string customerId, int fundId)
        {
            await _activeLinkagesRepository.DeleteLinkageAsync(customerId, fundId);
        }
    }
}
