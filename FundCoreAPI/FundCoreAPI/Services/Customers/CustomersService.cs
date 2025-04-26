namespace FundCoreAPI.Services.Customers
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Repositories.Customers;

    public class CustomersService : ICustomersService
    {
        private readonly ICustomersRepository _customersRepository;

        public CustomersService(ICustomersRepository customersRepository)
        {
            _customersRepository = customersRepository;
        }

        public async Task CreateCustomerAsync(Customers customer)
        {
            await _customersRepository.CreateCustomerAsync(customer);
        }

        public async Task<Customers?> GetCustomerByIdAsync(string customerId)
        {
            return await _customersRepository.GetCustomerByIdAsync(customerId);
        }

        public async Task UpdateCustomerAsync(Customers customer)
        {
            await _customersRepository.UpdateCustomerAsync(customer);
        }

        public async Task DeleteCustomerAsync(string customerId)
        {
            await _customersRepository.DeleteCustomerAsync(customerId);
        }
    }
}
