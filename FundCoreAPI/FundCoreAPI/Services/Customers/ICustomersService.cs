namespace FundCoreAPI.Services.Customers
{
    using FundCoreAPI.Models;

    public interface ICustomersService
    {
        Task CreateCustomerAsync(Customers customer);
        Task<Customers?> GetCustomerByIdAsync(string customerId);
        Task UpdateCustomerAsync(Customers customer);
        Task DeleteCustomerAsync(string customerId);
    }
}
