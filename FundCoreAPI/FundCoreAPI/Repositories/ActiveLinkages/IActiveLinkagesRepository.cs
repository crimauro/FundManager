namespace FundCoreAPI.Repositories.ActiveLinkages
{
    using FundCoreAPI.Models;

    public interface IActiveLinkagesRepository
    {
        Task CreateLinkageAsync(ActiveLinkages linkage);
        Task<ActiveLinkages?> GetLinkageByIdAsync(string customerId, int fundId);
        Task<List<ActiveLinkages>> GetAllLinkagesAsync(string customerId);
        Task<List<ActiveLinkages>> GetLinkagesByCategoryAsync(string customerId, string category);
        Task DeleteLinkageAsync(string customerId, int fundId);
    }
}
