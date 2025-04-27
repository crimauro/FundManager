namespace FundCoreAPI.Controllers
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Services.Customers;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for managing customer-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersService _customersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="customersService">Service for handling customer operations.</param>
        public CustomersController(ICustomersService customersService)
        {
            _customersService = customersService;
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customer">The customer to create.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
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

        /// <summary>
        /// Retrieves a customer by their ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to retrieve.</param>
        /// <returns>An <see cref="IActionResult"/> containing the customer or a not found result.</returns>
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

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer to update.</param>
        /// <param name="customer">The updated customer data.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
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

        /// <summary>
        /// Deletes a customer by their ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to delete.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
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
