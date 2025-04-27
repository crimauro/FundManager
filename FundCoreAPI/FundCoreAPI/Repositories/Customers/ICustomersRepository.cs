namespace FundCoreAPI.Repositories.Customers
{
    using FundCoreAPI.Models;

    /// <summary>
    /// Interface for managing customer repository operations.
    /// </summary>
    public interface ICustomersRepository
    {
        /// <summary>
        /// Creates a new customer asynchronously.
        /// </summary>
        /// <param name="customer">The customer entity to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateCustomerAsync(Customers customer);

        /// <summary>
        /// Retrieves a customer by their unique identifier asynchronously.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>A task representing the asynchronous operation, containing the customer entity if found.</returns>
        Task<Customers?> GetCustomerByIdAsync(string customerId);

        /// <summary>
        /// Updates an existing customer asynchronously.
        /// </summary>
        /// <param name="customer">The customer entity with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateCustomerAsync(Customers customer);

        /// <summary>
        /// Deletes a customer by their unique identifier asynchronously.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteCustomerAsync(string customerId);
    }
}
