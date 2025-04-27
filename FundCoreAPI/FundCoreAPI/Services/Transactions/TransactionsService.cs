namespace FundCoreAPI.Services.Transactions
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Repositories.ActiveLinkages;
    using FundCoreAPI.Repositories.Customers;
    using FundCoreAPI.Repositories.Funds;
    using FundCoreAPI.Repositories.Transactions;
    using FundCoreAPI.Services.Notifications;

    /// <summary>
    /// Service responsible for handling transaction-related operations.
    /// </summary>
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IFundsRepository _fundsRepository;
        private readonly ICustomersRepository _customersRepository;
        private readonly INotificationService _notificationService;
        private readonly IActiveLinkagesRepository _activeLinkagesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsService"/> class with all dependencies.
        /// </summary>
        /// <param name="transactionsRepository">Repository for transaction operations.</param>
        /// <param name="fundsRepository">Repository for fund operations.</param>
        /// <param name="customersRepository">Repository for customer operations.</param>
        /// <param name="notificationService">Service for sending notifications.</param>
        /// <param name="activeLinkagesRepository">Repository for active linkages operations.</param>
        public TransactionsService(
            ITransactionsRepository transactionsRepository,
            IFundsRepository fundsRepository,
            ICustomersRepository customersRepository,
            INotificationService notificationService,
            IActiveLinkagesRepository activeLinkagesRepository)
        {
            _transactionsRepository = transactionsRepository;
            _fundsRepository = fundsRepository;
            _customersRepository = customersRepository;
            _notificationService = notificationService;
            _activeLinkagesRepository = activeLinkagesRepository;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsService"/> class with only the transactions repository.
        /// </summary>
        /// <param name="transactionsRepository">Repository for transaction operations.</param>
        public TransactionsService(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        /// <summary>
        /// Creates a simple transaction asynchronously.
        /// </summary>
        /// <param name="transaction">The transaction to be created.</param>
        public async Task CreateTransactionSimpleAsync(Transaction transaction)
        {
            await _transactionsRepository.CreateTransactionAsync(transaction);
        }

        /// <summary>
        /// Creates a transaction asynchronously with validations and additional operations.
        /// </summary>
        /// <param name="transaction">The transaction to be created.</param>
        /// <exception cref="InvalidOperationException">Thrown when validation fails.</exception>
        public async Task CreateTransactionAsync(Transaction transaction)
        {
            // Verify if the FundId exists in the Funds table
            var fund = await _fundsRepository.GetFundByIdAsync(transaction.FundId);
            if (fund == null)
            {
                throw new InvalidOperationException($"The fund with ID {transaction.FundId} does not exist.");
            }

            transaction.FundName = fund.Name;

            // Verify if the CustomerId exists in the Customers table
            var customer = await _customersRepository.GetCustomerByIdAsync(transaction.CustomerId);
            if (customer == null)
            {
                throw new InvalidOperationException($"The customer with ID {transaction.CustomerId} does not exist.");
            }

            // Verify if the Amount is greater than or equal to the MinimumAmount of the fund
            if (transaction.Amount < fund.MinimumAmount)
            {
                throw new InvalidOperationException($"The amount must be greater than or equal to the minimum required for the fund {fund.Name}.");
            }

            if (transaction.OperationType == "OPENING" && customer.AvailableBalance < transaction.Amount)
            {
                throw new InvalidOperationException($"Insufficient balance to link to the fund {fund.Name}.");
            }

            // Interact with the ActiveLinkages table
            if (transaction.OperationType == "OPENING")
            {
                var exists = await _activeLinkagesRepository.GetLinkageByIdAsync(transaction.CustomerId, transaction.FundId);
                if (exists != null)
                {
                    throw new InvalidOperationException($"The fund {fund.Name} already has an opening for the customer {customer.Name}.");
                }

                // Register the linkage
                await _activeLinkagesRepository.CreateLinkageAsync(new ActiveLinkages
                {
                    FundId = transaction.FundId,
                    FundName = fund.Name,
                    CustomerId = transaction.CustomerId,
                    LinkedAmount = transaction.Amount,
                    LinkageDate = DateTime.UtcNow,
                    Category = fund.Category
                });

                // Update the customer's balance
                customer.AvailableBalance -= transaction.Amount;
            }
            else if (transaction.OperationType == "CLOSURE")
            {
                var exists = await _activeLinkagesRepository.GetLinkageByIdAsync(transaction.CustomerId, transaction.FundId);
                if (exists == null)
                {
                    throw new InvalidOperationException($"The fund {fund.Name} does not have an opening for the customer {customer.Name}.");
                }

                // Remove the linkage
                await _activeLinkagesRepository.DeleteLinkageAsync(transaction.CustomerId, transaction.FundId);

                // Update the customer's balance
                customer.AvailableBalance += exists.LinkedAmount;
            }

            await _customersRepository.UpdateCustomerAsync(customer);

            // Save the transaction
            await _transactionsRepository.CreateTransactionAsync(transaction);

            // Send notification
            if (transaction.NotificationType == "EMAIL")
            {
                await _notificationService.SendEmailAsync(
                    subject: "Transaction Notification",
                    message: $"A transaction of type {transaction.OperationType} for an amount of {transaction.Amount} has been made in the fund {fund.Name}.",
                    email: customer.Email
                );
            }
            else if (transaction.NotificationType == "SMS")
            {
                await _notificationService.SendSmsAsync(
                    message: $"A transaction of type {transaction.OperationType} for an amount of {transaction.Amount} has been made in the fund {fund.Name}.",
                    phoneNumber: customer.Phone
                );
            }
        }

        /// <summary>
        /// Retrieves a transaction by its ID asynchronously.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to retrieve.</param>
        /// <returns>The transaction if found; otherwise, null.</returns>
        public async Task<Transaction?> GetTransactionByIdAsync(string transactionId)
        {
            return await _transactionsRepository.GetTransactionByIdAsync(transactionId);
        }

        /// <summary>
        /// Retrieves all transactions asynchronously.
        /// </summary>
        /// <returns>A list of all transactions.</returns>
        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionsRepository.GetAllTransactionsAsync();
        }

        /// <summary>
        /// Retrieves transactions by fund ID asynchronously.
        /// </summary>
        /// <param name="fundId">The ID of the fund.</param>
        /// <returns>A list of transactions associated with the specified fund.</returns>
        public async Task<List<Transaction>> GetTransactionsByFundIdAsync(int fundId)
        {
            return await _transactionsRepository.GetTransactionsByFundIdAsync(fundId);
        }


        /// <summary>
        /// Retrieves transactions by customer ID asynchronously.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>A list of transactions associated with the specified customer.</returns>
        public async Task<List<Transaction>> GetTransactionsByCustomerIdAsync(string customerId)
        {
            return await _transactionsRepository.GetTransactionsByCustomerIdAsync(customerId);
        }

        /// <summary>
        /// Deletes a transaction by its ID asynchronously.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction to delete.</param>
        public async Task DeleteTransactionAsync(string transactionId)
        {
            await _transactionsRepository.DeleteTransactionAsync(transactionId);
        }
    }
}
