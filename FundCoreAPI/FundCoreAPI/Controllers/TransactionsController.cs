namespace FundCoreAPI.Controllers
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Services.Transactions;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

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
