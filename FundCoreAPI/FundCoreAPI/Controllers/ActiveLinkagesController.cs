namespace FundCoreAPI.Controllers
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Services.ActiveLinkages;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for managing active linkages.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ActiveLinkagesController : ControllerBase
    {
        private readonly IActiveLinkagesService _activeLinkagesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveLinkagesController"/> class.
        /// </summary>
        /// <param name="activeLinkagesService">Service for managing active linkages.</param>
        public ActiveLinkagesController(IActiveLinkagesService activeLinkagesService)
        {
            _activeLinkagesService = activeLinkagesService;
        }

        /// <summary>
        /// Creates a new linkage.
        /// </summary>
        /// <param name="linkage">The linkage to create.</param>
        /// <returns>An action result indicating the outcome of the operation.</returns>
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

        /// <summary>
        /// Retrieves a linkage by its customer ID and fund ID.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="fundId">The fund ID.</param>
        /// <returns>The requested linkage, if found.</returns>
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

        /// <summary>
        /// Retrieves all linkages for a specific customer.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>A list of linkages associated with the customer.</returns>
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

        /// <summary>
        /// Retrieves linkages for a specific customer filtered by category.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="category">The category to filter by.</param>
        /// <returns>A list of linkages matching the specified category.</returns>
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

        /// <summary>
        /// Deletes a linkage by its customer ID and fund ID.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="fundId">The fund ID.</param>
        /// <returns>An action result indicating the outcome of the operation.</returns>
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
