using Microsoft.AspNetCore.Mvc;

namespace FundCoreAPI.Controllers
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Services.Funds;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for managing fund-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FundsController : ControllerBase
    {
        private readonly IFundsService _fundsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FundsController"/> class.
        /// </summary>
        /// <param name="fundsService">Service for handling fund operations.</param>
        public FundsController(IFundsService fundsService)
        {
            _fundsService = fundsService;
        }

        /// <summary>
        /// Creates a new fund.
        /// </summary>
        /// <param name="fund">The fund to create.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateFund([FromBody] Fund fund)
        {
            try
            {
                await _fundsService.CreateFundAsync(fund);
                return CreatedAtAction(nameof(GetFundById), new { fundId = fund.Id }, fund);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating fund: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a fund by its ID.
        /// </summary>
        /// <param name="fundId">The ID of the fund to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the fund or a not found result.</returns>
        [HttpGet("{fundId}")]
        public async Task<IActionResult> GetFundById(int fundId)
        {
            try
            {
                var fund = await _fundsService.GetFundByIdAsync(fundId);
                if (fund == null)
                {
                    return NotFound();
                }
                return Ok(fund);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving fund: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all funds.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> containing the list of funds.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllFunds()
        {
            try
            {
                var funds = await _fundsService.GetAllFundsAsync();
                return Ok(funds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving funds: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing fund.
        /// </summary>
        /// <param name="fundId">The ID of the fund to update.</param>
        /// <param name="fund">The updated fund data.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPut("{fundId}")]
        public async Task<IActionResult> UpdateFund(string fundId, [FromBody] Fund fund)
        {
            try
            {
                if (fundId != fund.Id.ToString())
                {
                    return BadRequest("Fund ID mismatch.");
                }

                await _fundsService.UpdateFundAsync(fund);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating fund: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a fund by its ID.
        /// </summary>
        /// <param name="fundId">The ID of the fund to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpDelete("{fundId}")]
        public async Task<IActionResult> DeleteFund(int fundId)
        {
            try
            {
                await _fundsService.DeleteFundAsync(fundId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting fund: {ex.Message}");
            }
        }
    }
}
