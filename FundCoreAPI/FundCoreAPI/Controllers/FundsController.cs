using Microsoft.AspNetCore.Mvc;

namespace FundCoreAPI.Controllers
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Services.Funds;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class FundsController : ControllerBase
    {
        private readonly IFundsService _fundsService;

        public FundsController(IFundsService fundsService)
        {
            _fundsService = fundsService;
        }

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
