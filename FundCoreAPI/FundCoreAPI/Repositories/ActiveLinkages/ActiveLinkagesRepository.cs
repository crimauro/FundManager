namespace FundCoreAPI.Repositories.ActiveLinkages
{
    using Amazon.DynamoDBv2.DataModel;
    using Amazon.DynamoDBv2.DocumentModel;
    using FundCoreAPI.Models;

    public class ActiveLinkagesRepository : IActiveLinkagesRepository
    {
        private readonly IDynamoDBContext _dbContext;

        public ActiveLinkagesRepository(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateLinkageAsync(ActiveLinkages linkage)
        {
            await _dbContext.SaveAsync(linkage);
        }

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


        public async Task<List<ActiveLinkages>> GetAllLinkagesAsync(string customerId)
        {
            return await _dbContext.QueryAsync<ActiveLinkages>(customerId).GetRemainingAsync();
        }

        public async Task<List<ActiveLinkages>> GetLinkagesByCategoryAsync(string customerId, string category)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("CustomerId", ScanOperator.Equal, customerId),
                new ScanCondition("Category", ScanOperator.Equal, category)
            };
            return await _dbContext.ScanAsync<ActiveLinkages>(conditions).GetRemainingAsync();
        }

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
