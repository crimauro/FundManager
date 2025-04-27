namespace FundCoreAPI.Repositories.Customers
{
    using Amazon.DynamoDBv2.DataModel;
    using Amazon.DynamoDBv2.DocumentModel;
    using FundCoreAPI.Models;

    /// <summary>
    /// Repository for managing customer data in the DynamoDB "Customers" table.
    /// </summary>
    public class CustomersRepository : ICustomersRepository
    {
        private readonly IDynamoDBContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The DynamoDB context for database operations.</param>
        public CustomersRepository(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Creates a new customer in the database.
        /// </summary>
        /// <param name="customer">The customer entity to create.</param>
        public async Task CreateCustomerAsync(Customers customer)
        {
            await _dbContext.SaveAsync(customer);
        }

        /// <summary>
        /// Retrieves a customer by their identification number.
        /// </summary>
        /// <param name="customerId">The identification number of the customer.</param>
        /// <returns>The customer entity if found; otherwise, null.</returns>
        public async Task<Customers?> GetCustomerByIdAsync(string customerId)
        {
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("IdentificationNumber", ScanOperator.Equal, customerId)
                };

            var results = await _dbContext.ScanAsync<Customers>(conditions).GetRemainingAsync();
            return results.FirstOrDefault();
        }

        /// <summary>
        /// Updates an existing customer in the database.
        /// </summary>
        /// <param name="customer">The customer entity with updated information.</param>
        public async Task UpdateCustomerAsync(Customers customer)
        {
            await _dbContext.SaveAsync(customer);
        }

        /// <summary>
        /// Deletes a customer from the database by their identification number.
        /// </summary>
        /// <param name="customerId">The identification number of the customer to delete.</param>
        public async Task DeleteCustomerAsync(string customerId)
        {
            var customer = await GetCustomerByIdAsync(customerId);
            if (customer != null)
            {
                await _dbContext.DeleteAsync(customer);
            }
        }
    }
}
