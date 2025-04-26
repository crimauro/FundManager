namespace FundCoreAPI.Repositories.Customers
{
    using Amazon.DynamoDBv2.DataModel;
    using Amazon.DynamoDBv2.DocumentModel;
    using FundCoreAPI.Models;

    public class CustomersRepository : ICustomersRepository
    {
        private readonly IDynamoDBContext _dbContext;

        public CustomersRepository(IDynamoDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateCustomerAsync(Customers customer)
        {
            await _dbContext.SaveAsync(customer);
        }

        public async Task<Customers?> GetCustomerByIdAsync(string customerId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition("IdentificationNumber", ScanOperator.Equal, customerId)
            };

            var results = await _dbContext.ScanAsync<Customers>(conditions).GetRemainingAsync();
            return results.FirstOrDefault();
        }

        public async Task UpdateCustomerAsync(Customers customer)
        {
            await _dbContext.SaveAsync(customer);
        }

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
