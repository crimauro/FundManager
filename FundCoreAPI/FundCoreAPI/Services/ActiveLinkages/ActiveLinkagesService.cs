namespace FundCoreAPI.Services.ActiveLinkages
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Repositories.ActiveLinkages;

    public class ActiveLinkagesService : IActiveLinkagesService
    {
        private readonly IActiveLinkagesRepository _activeLinkagesRepository;

        public ActiveLinkagesService(IActiveLinkagesRepository activeLinkagesRepository)
        {
            _activeLinkagesRepository = activeLinkagesRepository;
        }

        public async Task CreateLinkageAsync(ActiveLinkages linkage)
        {
            await _activeLinkagesRepository.CreateLinkageAsync(linkage);
        }

        public async Task<ActiveLinkages?> GetLinkageByIdAsync(string customerId, int fundId)
        {
            return await _activeLinkagesRepository.GetLinkageByIdAsync(customerId, fundId);
        }

        public async Task<List<ActiveLinkages>> GetAllLinkagesAsync(string customerId)
        {
            return await _activeLinkagesRepository.GetAllLinkagesAsync(customerId);
        }

        public async Task<List<ActiveLinkages>> GetLinkagesByCategoryAsync(string customerId, string category)
        {
            return await _activeLinkagesRepository.GetLinkagesByCategoryAsync(customerId, category);
        }

        public async Task DeleteLinkageAsync(string customerId, int fundId)
        {
            await _activeLinkagesRepository.DeleteLinkageAsync(customerId, fundId);
        }
    }
}
