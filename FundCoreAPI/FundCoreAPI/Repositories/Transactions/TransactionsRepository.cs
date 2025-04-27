namespace FundCoreAPI.Repositories.Transactions
{
    using Amazon.DynamoDBv2.DataModel;
    using Amazon.DynamoDBv2.DocumentModel;
    using FundCoreAPI.Models;

    /// <summary>
    /// Repository for managing transaction data in DynamoDB.
    /// </summary>
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly IDynamoDBContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The DynamoDB context for database operations.</param>
        public TransactionsRepository(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new transaction in the database.
        /// </summary>
        /// <param name="transaction">The transaction to create.</param>
        public async Task CreateTransactionAsync(Transaction transaction)
        {
            await _dbContext.SaveAsync(transaction);
        }

        /// <summary>
        /// Retrieves a transaction by its unique identifier.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
        /// <returns>The transaction if found; otherwise, null.</returns>
        public async Task<Transaction?> GetTransactionByIdAsync(string transactionId)
        {
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("TransactionId", ScanOperator.Equal, transactionId)
                };

            var results = await _dbContext.ScanAsync<Transaction>(conditions).GetRemainingAsync();
            return results.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves all transactions from the database.
        /// </summary>
        /// <returns>A list of all transactions.</returns>
        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _dbContext.ScanAsync<Transaction>(new List<ScanCondition>()).GetRemainingAsync();
        }

        /// <summary>
        /// Retrieves transactions associated with a specific fund.
        /// </summary>
        /// <param name="fundId">The identifier of the fund.</param>
        /// <returns>A list of transactions associated with the specified fund.</returns>
        public async Task<List<Transaction>> GetTransactionsByFundIdAsync(int fundId)
        {
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("FundId", ScanOperator.Equal, fundId)
                };
            return await _dbContext.ScanAsync<Transaction>(conditions).GetRemainingAsync();
        }

        /// <summary>
        /// Deletes a transaction by its unique identifier.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction to delete.</param>
        public async Task DeleteTransactionAsync(string transactionId)
        {
            var transaction = await GetTransactionByIdAsync(transactionId);
            if (transaction != null)
            {
                await _dbContext.DeleteAsync(transaction);
            }
        }

        /// <summary>
        /// Retrieves transactions associated with a specific customer.
        /// </summary>
        /// <param name="CustomerId">The identifier of the customer.</param>
        /// <returns>A list of transactions associated with the specified customer.</returns>
        public async Task<List<Transaction>> GetTransactionsByCustomerIddAsync(int CustomerId)
        {
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("CustomerId", ScanOperator.Equal, CustomerId)
                };
            return await _dbContext.ScanAsync<Transaction>(conditions).GetRemainingAsync();
        }
    }
}
