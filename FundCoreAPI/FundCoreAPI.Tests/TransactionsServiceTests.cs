using FundCoreAPI.Models;
using FundCoreAPI.Repositories.ActiveLinkages;
using FundCoreAPI.Repositories.Customers;
using FundCoreAPI.Repositories.Funds;
using FundCoreAPI.Repositories.Transactions;
using FundCoreAPI.Services.Notifications;
using FundCoreAPI.Services.Transactions;
using Moq;

namespace FundCoreAPI.Tests
{
    /// <summary>
    /// Unit tests for the TransactionsService class.
    /// </summary>
    public class TransactionsServiceTests
    {
        /// <summary>
        /// Mock for the transactions repository.
        /// </summary>
        private readonly Mock<ITransactionsRepository> _transactionsRepositoryMock;

        /// <summary>
        /// Mock for the funds repository.
        /// </summary>
        private readonly Mock<IFundsRepository> _fundsRepositoryMock;

        /// <summary>
        /// Mock for the customers repository.
        /// </summary>
        private readonly Mock<ICustomersRepository> _customersRepositoryMock;

        /// <summary>
        /// Mock for the notification service.
        /// </summary>
        private readonly Mock<INotificationService> _notificationServiceMock;

        /// <summary>
        /// Mock for the active linkages repository.
        /// </summary>
        private readonly Mock<IActiveLinkagesRepository> _activeLinkagesRepositoryMock;

        /// <summary>
        /// Instance of the TransactionsService being tested.
        /// </summary>
        private readonly TransactionsService _transactionsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionsServiceTests"/> class.
        /// </summary>
        public TransactionsServiceTests()
        {
            _transactionsRepositoryMock = new Mock<ITransactionsRepository>();
            _fundsRepositoryMock = new Mock<IFundsRepository>();
            _customersRepositoryMock = new Mock<ICustomersRepository>();
            _notificationServiceMock = new Mock<INotificationService>();
            _activeLinkagesRepositoryMock = new Mock<IActiveLinkagesRepository>();

            _transactionsService = new TransactionsService(
                _transactionsRepositoryMock.Object,
                _fundsRepositoryMock.Object,
                _customersRepositoryMock.Object,
                _notificationServiceMock.Object,
                _activeLinkagesRepositoryMock.Object
            );
        }

        /// <summary>
        /// Verifies that an exception is thrown when the fund does not exist.
        /// </summary>
        [Fact]
        public async Task CreateTransactionAsync_ShouldThrowException_WhenFundDoesNotExist()
        {
            // Arrange
            var transaction = new Transaction { FundId = 101, CustomerId = "C1", Amount = 1000, OperationType = "OPENING" };

            _fundsRepositoryMock
                .Setup(repo => repo.GetFundByIdAsync(transaction.FundId))
                .ReturnsAsync((Fund?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _transactionsService.CreateTransactionAsync(transaction));

            Assert.Equal($"The fund with ID {transaction.FundId} does not exist.", exception.Message);
        }

        /// <summary>
        /// Verifies that an exception is thrown when the customer does not exist.
        /// </summary>
        [Fact]
        public async Task CreateTransactionAsync_ShouldThrowException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var transaction = new Transaction { FundId = 101, CustomerId = "C1", Amount = 1000, OperationType = "OPENING" };

            _fundsRepositoryMock
                .Setup(repo => repo.GetFundByIdAsync(transaction.FundId))
                .ReturnsAsync(new Fund { Id = 101, MinimumAmount = 500 });

            _customersRepositoryMock
                .Setup(repo => repo.GetCustomerByIdAsync(transaction.CustomerId))
                .ReturnsAsync((Customers?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _transactionsService.CreateTransactionAsync(transaction));

            Assert.Equal($"The customer with ID {transaction.CustomerId} does not exist.", exception.Message);
        }

        /// <summary>
        /// Verifies that an exception is thrown when the transaction amount is less than the fund's minimum amount.
        /// </summary>
        [Fact]
        public async Task CreateTransactionAsync_ShouldThrowException_WhenAmountIsLessThanMinimum()
        {
            // Arrange
            var transaction = new Transaction { FundId = 101, CustomerId = "C1", Amount = 400, OperationType = "OPENING" };

            _fundsRepositoryMock
                .Setup(repo => repo.GetFundByIdAsync(transaction.FundId))
                .ReturnsAsync(new Fund { Id = 101, Name = "Fund for Children", MinimumAmount = 500 });

            _customersRepositoryMock
                .Setup(repo => repo.GetCustomerByIdAsync(transaction.CustomerId))
                .ReturnsAsync(new Customers { Name = "Juan", IdentificationNumber = "C1", AvailableBalance = 1000 });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _transactionsService.CreateTransactionAsync(transaction));

            Assert.Equal($"The amount must be greater than or equal to the minimum required for the fund Fund for Children.", exception.Message);
        }

        /// <summary>
        /// Verifies that a transaction is created successfully when all validations pass.
        /// </summary>
        [Fact]
        public async Task CreateTransactionAsync_ShouldCreateTransaction_WhenValid()
        {
            // Arrange
            var transaction = new Transaction { FundId = 101, CustomerId = "C1", Amount = 1000, OperationType = "OPENING",  NotificationType = "EMAIL" };

            var fund = new Fund { Id = 101, MinimumAmount = 500, Name = "Test Fund", Category = "FPV" };
            var customer = new Customers { IdentificationNumber = "C1", Name = "Test Customer", AvailableBalance = 2000, Email = "test@example.com" };

            _fundsRepositoryMock
                .Setup(repo => repo.GetFundByIdAsync(transaction.FundId))
                .ReturnsAsync(fund);

            _customersRepositoryMock
                .Setup(repo => repo.GetCustomerByIdAsync(transaction.CustomerId))
                .ReturnsAsync(customer);

            _activeLinkagesRepositoryMock
                .Setup(repo => repo.GetLinkageByIdAsync(transaction.CustomerId, transaction.FundId))
                .ReturnsAsync((ActiveLinkages?)null);

            // Act
            await _transactionsService.CreateTransactionAsync(transaction);

            // Assert
            _transactionsRepositoryMock.Verify(repo => repo.CreateTransactionAsync(transaction), Times.Once);
            _notificationServiceMock.Verify(service => service.SendEmailAsync(
                "Transaction Notification",
                It.IsAny<string>(),
                customer.Email
            ), Times.Once);
        }

        /// <summary>
        /// Verifies that all transactions are retrieved successfully.
        /// </summary>
        [Fact]
        public async Task GetAllTransactionsAsync_ShouldReturnTransactionsList()
        {
            // Arrange
            var transactions = new List<Transaction>
                {
                    new Transaction { TransactionId = "1", FundId = 101, CustomerId = "C1", Amount = 1000, OperationType = "OPENING" },
                    new Transaction { TransactionId = "2", FundId = 102, CustomerId = "C2", Amount = 2000, OperationType = "CLOSURE" }
                };

            _transactionsRepositoryMock
                .Setup(repo => repo.GetAllTransactionsAsync())
                .ReturnsAsync(transactions);

            // Act
            var result = await _transactionsService.GetAllTransactionsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("1", result[0].TransactionId);
            Assert.Equal("2", result[1].TransactionId);

            _transactionsRepositoryMock.Verify(repo => repo.GetAllTransactionsAsync(), Times.Once);
        }
    }
}
