namespace FundCoreAPI.Repositories.Transactions
{
    using FundCoreAPI.Models;

    /// <summary>
    /// Interface for managing transaction-related operations.
    /// </summary>
    public interface ITransactionsRepository
    {
        /// <summary>
        /// Asynchronously creates a new transaction.
        /// </summary>
        /// <param name="transaction">The transaction to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateTransactionAsync(Transaction transaction);

        /// <summary>
        /// Asynchronously retrieves a transaction by its unique identifier.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction.</param>
        /// <returns>A task representing the asynchronous operation, containing the transaction if found.</returns>
        Task<Transaction?> GetTransactionByIdAsync(string transactionId);

        /// <summary>
        /// Asynchronously retrieves all transactions.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of all transactions.</returns>
        Task<List<Transaction>> GetAllTransactionsAsync();

        /// <summary>
        /// Asynchronously retrieves transactions associated with a specific fund.
        /// </summary>
        /// <param name="fundId">The identifier of the fund.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of transactions for the specified fund.</returns>
        Task<List<Transaction>> GetTransactionsByFundIdAsync(int fundId);

        /// <summary>
        /// Retrieves transactions associated with a specific Customer.
        /// </summary>
        /// <param name="customerId">The identifier of the customer.</param>
        /// <returns>A list of transactions associated with the specified customer.</returns>
        Task<List<Transaction>> GetTransactionsByCustomerIdAsync(string customerId);

        /// <summary>
        /// Asynchronously retrieves transactions associated with a specific customer.
        /// </summary>
        /// <param name="CustomerId">The identifier of the customer.</param>
        /// <returns>A task representing the asynchronous operation, containing a list of transactions for the specified customer.</returns>
        Task<List<Transaction>> GetTransactionsByCustomerIddAsync(int CustomerId);

        /// <summary>
        /// Asynchronously deletes a transaction by its unique identifier.
        /// </summary>
        /// <param name="transactionId">The unique identifier of the transaction to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteTransactionAsync(string transactionId);
    }
}
