namespace FundCoreAPI.Controllers
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Services.Customers;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersService _customersService;

        public CustomersController(ICustomersService customersService)
        {
            _customersService = customersService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customers customer)
        {
            try
            {
                await _customersService.CreateCustomerAsync(customer);
                return CreatedAtAction(nameof(GetCustomerById), new { customerId = customer.PK }, customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById(string customerId)
        {
            try
            {
                var customer = await _customersService.GetCustomerByIdAsync(customerId);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{customerId}")]
        public async Task<IActionResult> UpdateCustomer(string customerId, [FromBody] Customers customer)
        {
            try
            {
                if (customerId != customer.PK)
                {
                    return BadRequest("Customer ID mismatch.");
                }

                await _customersService.UpdateCustomerAsync(customer);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(string customerId)
        {
            try
            {
                await _customersService.DeleteCustomerAsync(customerId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
