namespace FundCoreAPI.Repositories.Transactions
{
    using FundCoreAPI.Models;

    public interface ITransactionsRepository
    {
        Task CreateTransactionAsync(Transaction transaction);
        Task<Transaction?> GetTransactionByIdAsync(string transactionId);
        Task<List<Transaction>> GetAllTransactionsAsync();
        Task<List<Transaction>> GetTransactionsByFundIdAsync(int fundId);

        Task<List<Transaction>> GetTransactionsByCustomerIddAsync(int CustomerId);
        Task DeleteTransactionAsync(string transactionId);
    }
}
