namespace FundCoreAPI.Repositories.Funds
{
    using Amazon.DynamoDBv2.DataModel;
    using Amazon.DynamoDBv2.DocumentModel;
    using FundCoreAPI.Models;

    /// <summary>
    /// Repository for managing fund entities in the DynamoDB table.
    /// </summary>
    public class FundsRepository : IFundsRepository
    {
        private readonly IDynamoDBContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The DynamoDB context for database operations.</param>
        public FundsRepository(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new fund in the database.
        /// </summary>
        /// <param name="fund">The fund entity to create.</param>
        public async Task CreateFundAsync(Fund fund)
        {
            await _dbContext.SaveAsync(fund);
        }

        /// <summary>
        /// Retrieves a fund by its unique identifier.
        /// </summary>
        /// <param name="fundId">The unique identifier of the fund.</param>
        /// <returns>The fund entity if found; otherwise, null.</returns>
        public async Task<Fund?> GetFundByIdAsync(int fundId)
        {
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("Id", ScanOperator.Equal, fundId)
                };

            var results = await _dbContext.ScanAsync<Fund>(conditions).GetRemainingAsync();
            return results.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all funds from the database.
        /// </summary>
        /// <returns>A list of all fund entities.</returns>
        public async Task<List<Fund>> GetAllFundsAsync()
        {
            return await _dbContext.ScanAsync<Fund>(new List<ScanCondition>()).GetRemainingAsync();
        }

        /// <summary>
        /// Updates an existing fund in the database.
        /// </summary>
        /// <param name="fund">The fund entity with updated information.</param>
        public async Task UpdateFundAsync(Fund fund)
        {
            await _dbContext.SaveAsync(fund);
        }

        /// <summary>
        /// Deletes a fund by its unique identifier.
        /// </summary>
        /// <param name="fundId">The unique identifier of the fund to delete.</param>
        public async Task DeleteFundAsync(int fundId)
        {
            var fund = await GetFundByIdAsync(fundId);
            if (fund != null)
            {
                await _dbContext.DeleteAsync(fund);
            }
        }
    }
}
