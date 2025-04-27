namespace FundCoreAPI.Controllers
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Services.Transactions;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Controller for managing transactions.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsController"/> class.
        /// </summary>
        /// <param name="transactionsService">Service for handling transaction operations.</param>
        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        /// <summary>
        /// Creates a new transaction.
        /// </summary>
        /// <param name="transaction">The transaction to create.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
        {
            try
            {
                await _transactionsService.CreateTransactionAsync(transaction);
                return CreatedAtAction(nameof(GetTransactionById), new { transactionId = transaction.TransactionId }, transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the transaction.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves a transaction by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to retrieve.</param>
        /// <returns>The transaction if found, or a not found response.</returns>
        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransactionById(string transactionId)
        {
            try
            {
                var transaction = await _transactionsService.GetTransactionByIdAsync(transactionId);
                if (transaction == null)
                {
                    return NotFound(new { Message = "Transaction not found." });
                }
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the transaction.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves all transactions.
        /// </summary>
        /// <returns>A list of all transactions.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = await _transactionsService.GetAllTransactionsAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving transactions.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves transactions by fund ID.
        /// </summary>
        /// <param name="fundId">The ID of the fund to filter transactions by.</param>
        /// <returns>A list of transactions associated with the specified fund ID.</returns>
        [HttpGet("fund/{fundId}")]
        public async Task<IActionResult> GetTransactionsByFundId(int fundId)
        {
            try
            {
                var transactions = await _transactionsService.GetTransactionsByFundIdAsync(fundId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving transactions by fund ID.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Retrieves transactions by customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer to filter transactions by.</param>
        /// <returns>A list of transactions associated with the specified customer ID.</returns>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetTransactionsByCustomerId(string customerId)
        {
            try
            {
                var transactions = await _transactionsService.GetTransactionsByCustomerIdAsync(customerId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving transactions by customer ID.", Details = ex.Message });
            }
        }

        /// <summary>
        /// Deletes a transaction by its ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to delete.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(string transactionId)
        {
            try
            {
                await _transactionsService.DeleteTransactionAsync(transactionId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the transaction.", Details = ex.Message });
            }
        }
    }
}
