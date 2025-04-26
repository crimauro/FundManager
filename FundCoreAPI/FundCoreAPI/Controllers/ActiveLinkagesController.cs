namespace FundCoreAPI.Controllers
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Services.ActiveLinkages;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class ActiveLinkagesController : ControllerBase
    {
        private readonly IActiveLinkagesService _activeLinkagesService;

        public ActiveLinkagesController(IActiveLinkagesService activeLinkagesService)
        {
            _activeLinkagesService = activeLinkagesService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLinkage([FromBody] ActiveLinkages linkage)
        {
            try
            {
                await _activeLinkagesService.CreateLinkageAsync(linkage);
                return CreatedAtAction(nameof(GetLinkageById), new { customerId = linkage.PK, fundId = linkage.FundId }, linkage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the linkage.", details = ex.Message });
            }
        }

        [HttpGet("{customerId}/{fundId}")]
        public async Task<IActionResult> GetLinkageById(string customerId, int fundId)
        {
            try
            {
                var linkage = await _activeLinkagesService.GetLinkageByIdAsync(customerId, fundId);
                if (linkage == null)
                {
                    return NotFound(new { message = "Linkage not found." });
                }
                return Ok(linkage);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the linkage.", details = ex.Message });
            }
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetAllLinkages(string customerId)
        {
            try
            {
                var linkages = await _activeLinkagesService.GetAllLinkagesAsync(customerId);
                return Ok(linkages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving all linkages.", details = ex.Message });
            }
        }

        [HttpGet("{customerId}/category/{category}")]
        public async Task<IActionResult> GetLinkagesByCategory(string customerId, string category)
        {
            try
            {
                var linkages = await _activeLinkagesService.GetLinkagesByCategoryAsync(customerId, category);
                return Ok(linkages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving linkages by category.", details = ex.Message });
            }
        }

        [HttpDelete("{customerId}/{fundId}")]
        public async Task<IActionResult> DeleteLinkage(string customerId, int fundId)
        {
            try
            {
                await _activeLinkagesService.DeleteLinkageAsync(customerId, fundId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the linkage.", details = ex.Message });
            }
        }
    }
}
