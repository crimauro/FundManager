namespace FundCoreAPI.Services.Customers
{
    using FundCoreAPI.Models;

    /// <summary>
    /// Interface for customer service operations.
    /// </summary>
    public interface ICustomersService
    {
        /// <summary>
        /// Asynchronously creates a new customer.
        /// </summary>
        /// <param name="customer">The customer entity to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateCustomerAsync(Customers customer);

        /// <summary>
        /// Asynchronously retrieves a customer by their ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve.</param>
        /// <returns>A task representing the asynchronous operation, containing the customer entity if found.</returns>
        Task<Customers?> GetCustomerByIdAsync(string customerId);

        /// <summary>
        /// Asynchronously updates an existing customer.
        /// </summary>
        /// <param name="customer">The customer entity with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateCustomerAsync(Customers customer);

        /// <summary>
        /// Asynchronously deletes a customer by their ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteCustomerAsync(string customerId);
    }
}
