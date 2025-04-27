namespace FundCoreAPI.Repositories.ActiveLinkages
{
    using Amazon.DynamoDBv2.DataModel;
    using Amazon.DynamoDBv2.DocumentModel;
    using FundCoreAPI.Models;

    /// <summary>
    /// Repository for managing ActiveLinkages in DynamoDB.
    /// </summary>
    public class ActiveLinkagesRepository : IActiveLinkagesRepository
    {
        private readonly IDynamoDBContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveLinkagesRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The DynamoDB context to interact with the database.</param>
        public ActiveLinkagesRepository(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new linkage in the database.
        /// </summary>
        /// <param name="linkage">The linkage to create.</param>
        public async Task CreateLinkageAsync(ActiveLinkages linkage)
        {
            await _dbContext.SaveAsync(linkage);
        }

        /// <summary>
        /// Retrieves a linkage by its customer ID and fund ID.
        /// </summary>
        /// <param name="customerId">The customer ID associated with the linkage.</param>
        /// <param name="fundId">The fund ID associated with the linkage.</param>
        /// <returns>The linkage if found; otherwise, null.</returns>
        public async Task<ActiveLinkages?> GetLinkageByIdAsync(string customerId, int fundId)
        {
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("CustomerId", ScanOperator.Equal, customerId),
                    new ScanCondition("FundId", ScanOperator.Equal, fundId)
                };

            var results = await _dbContext.ScanAsync<ActiveLinkages>(conditions).GetRemainingAsync();
            return results.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all linkages for a specific customer.
        /// </summary>
        /// <param name="customerId">The customer ID to filter linkages.</param>
        /// <returns>A list of linkages associated with the customer.</returns>
        public async Task<List<ActiveLinkages>> GetAllLinkagesAsync(string customerId)
        {
            //return await _dbContext.QueryAsync<ActiveLinkages>(customerId).GetRemainingAsync();
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("CustomerId", ScanOperator.Equal, customerId),
                };

            return await _dbContext.ScanAsync<ActiveLinkages>(conditions).GetRemainingAsync();
        }

        /// <summary>
        /// Retrieves linkages for a specific customer and category.
        /// </summary>
        /// <param name="customerId">The customer ID to filter linkages.</param>
        /// <param name="category">The category to filter linkages.</param>
        /// <returns>A list of linkages matching the customer ID and category.</returns>
        public async Task<List<ActiveLinkages>> GetLinkagesByCategoryAsync(string customerId, string category)
        {
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("CustomerId", ScanOperator.Equal, customerId),
                    new ScanCondition("Category", ScanOperator.Equal, category)
                };
            return await _dbContext.ScanAsync<ActiveLinkages>(conditions).GetRemainingAsync();
        }

        /// <summary>
        /// Deletes a linkage by its customer ID and fund ID.
        /// </summary>
        /// <param name="customerId">The customer ID associated with the linkage.</param>
        /// <param name="fundId">The fund ID associated with the linkage.</param>
        public async Task DeleteLinkageAsync(string customerId, int fundId)
        {
            var linkage = await GetLinkageByIdAsync(customerId, fundId);
            if (linkage != null)
            {
                await _dbContext.DeleteAsync(linkage);
            }
        }
    }
}
