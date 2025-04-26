namespace FundCoreAPI.Repositories.Customers
{
    using FundCoreAPI.Models;

    public interface ICustomersRepository
    {
        Task CreateCustomerAsync(Customers customer);
        Task<Customers?> GetCustomerByIdAsync(string customerId);
        Task UpdateCustomerAsync(Customers customer);
        Task DeleteCustomerAsync(string customerId);
    }
}
