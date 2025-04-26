namespace FundCoreAPI.Repositories.Transactions
{
    using Amazon.DynamoDBv2.DataModel;
    using Amazon.DynamoDBv2.DocumentModel;
    using FundCoreAPI.Models;

    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly IDynamoDBContext _dbContext;

        public TransactionsRepository(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            await _dbContext.SaveAsync(transaction);
        }

        public async Task<Transaction?> GetTransactionByIdAsync(string transactionId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("TransactionId", ScanOperator.Equal, transactionId)
            };

            var results = await _dbContext.ScanAsync<Transaction>(conditions).GetRemainingAsync();
            return results.FirstOrDefault();
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _dbContext.ScanAsync<Transaction>(new List<ScanCondition>()).GetRemainingAsync();
        }

        public async Task<List<Transaction>> GetTransactionsByFundIdAsync(int fundId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("FundId",ScanOperator.Equal, fundId)
            };
            return await _dbContext.ScanAsync<Transaction>(conditions).GetRemainingAsync();
        }

        public async Task DeleteTransactionAsync(string transactionId)
        {
            var transaction = await GetTransactionByIdAsync(transactionId);
            if (transaction != null)
            {
                await _dbContext.DeleteAsync(transaction);
            }
        }

        public async Task<List<Transaction>> GetTransactionsByCustomerIddAsync(int CustomerId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("CustomerId",ScanOperator.Equal, CustomerId)
            };
            return await _dbContext.ScanAsync<Transaction>(conditions).GetRemainingAsync();
        }
    }
}
