namespace FundCoreAPI.Repositories.Funds
{
    using Amazon.DynamoDBv2.DataModel;
    using Amazon.DynamoDBv2.DocumentModel;
    using FundCoreAPI.Models;

    public class FundsRepository : IFundsRepository
    {
        private readonly IDynamoDBContext _dbContext;

        public FundsRepository(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateFundAsync(Fund fund)
        {
            await _dbContext.SaveAsync(fund);
        }

        public async Task<Fund?> GetFundByIdAsync(int fundId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("Id", ScanOperator.Equal, fundId)
            };

            var results = await _dbContext.ScanAsync<Fund>(conditions).GetRemainingAsync();
            return results.FirstOrDefault();
        }

        public async Task<List<Fund>> GetAllFundsAsync()
        {
            return await _dbContext.ScanAsync<Fund>(new List<ScanCondition>()).GetRemainingAsync();
        }

        public async Task UpdateFundAsync(Fund fund)
        {
            await _dbContext.SaveAsync(fund);
        }

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
