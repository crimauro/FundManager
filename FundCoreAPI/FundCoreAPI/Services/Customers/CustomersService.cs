namespace FundCoreAPI.Services.Customers
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Repositories.Customers;

    /// <summary>
    /// Service class for managing customer-related operations.
    /// </summary>
    public class CustomersService : ICustomersService
    {
        private readonly ICustomersRepository _customersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersService"/> class.
        /// </summary>
        /// <param name="customersRepository">The repository for customer data operations.</param>
        public CustomersService(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }

        /// <summary>
        /// Creates a new customer asynchronously.
        /// </summary>
        /// <param name="customer">The customer entity to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateCustomerAsync(Customers customer)
        {
            await _customersRepository.CreateCustomerAsync(customer);
        }

        /// <summary>
        /// Retrieves a customer by their unique identifier asynchronously.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer.</param>
        /// <returns>A task representing the asynchronous operation, containing the customer entity if found.</returns>
        public async Task<Customers?> GetCustomerByIdAsync(string customerId)
        {
            return await _customersRepository.GetCustomerByIdAsync(customerId);
        }

        /// <summary>
        /// Updates an existing customer asynchronously.
        /// </summary>
        /// <param name="customer">The customer entity with updated information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateCustomerAsync(Customers customer)
        {
            await _customersRepository.UpdateCustomerAsync(customer);
        }

        /// <summary>
        /// Deletes a customer by their unique identifier asynchronously.
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DeleteCustomerAsync(string customerId)
        {
            await _customersRepository.DeleteCustomerAsync(customerId);
        }
    }
}
