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
    public class TransactionsServiceTests
    {
        private readonly Mock<ITransactionsRepository> _transactionsRepositoryMock;
        private readonly Mock<IFundsRepository> _fundsRepositoryMock;
        private readonly Mock<ICustomersRepository> _customersRepositoryMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<IActiveLinkagesRepository> _activeLinkagesRepositoryMock;
        private readonly TransactionsService _transactionsService;

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

        [Fact]
        public async Task CreateTransactionAsync_ShouldThrowException_WhenFundDoesNotExist()
        {
            // Arrange
            var transaction = new Transaction { FundId = 101, CustomerId = "C1", Amount = 1000, Type = "OPENING" };

            _fundsRepositoryMock
                .Setup(repo => repo.GetFundByIdAsync(transaction.FundId))
                .ReturnsAsync((Fund?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _transactionsService.CreateTransactionAsync(transaction));

            Assert.Equal($"El fondo con ID {transaction.FundId} no existe.", exception.Message);
        }

        [Fact]
        public async Task CreateTransactionAsync_ShouldThrowException_WhenCustomerDoesNotExist()
        {
            // Arrange
            var transaction = new Transaction { FundId = 101, CustomerId = "C1", Amount = 1000, Type = "OPENING" };

            _fundsRepositoryMock
                .Setup(repo => repo.GetFundByIdAsync(transaction.FundId))
                .ReturnsAsync(new Fund { Id = 101, MinimumAmount = 500 });

            _customersRepositoryMock
                .Setup(repo => repo.GetCustomerByIdAsync(transaction.CustomerId))
                .ReturnsAsync((Customers?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _transactionsService.CreateTransactionAsync(transaction));

            Assert.Equal($"El cliente con ID {transaction.CustomerId} no existe.", exception.Message);
        }

        [Fact]
        public async Task CreateTransactionAsync_ShouldThrowException_WhenAmountIsLessThanMinimum()
        {
            // Arrange
            var transaction = new Transaction { FundId = 101, CustomerId = "C1", Amount = 400, Type = "OPENING" };

            _fundsRepositoryMock
                .Setup(repo => repo.GetFundByIdAsync(transaction.FundId))
                .ReturnsAsync(new Fund { Id = 101, Name= "Fund for Childs", MinimumAmount = 500 });

            _customersRepositoryMock
                .Setup(repo => repo.GetCustomerByIdAsync(transaction.CustomerId))
                .ReturnsAsync(new Customers { Name ="Juan", IdentificationNumber = "C1", AvailableBalance = 1000 });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _transactionsService.CreateTransactionAsync(transaction));

            Assert.Equal($"El monto debe ser mayor o igual al mínimo requerido del fondo Fund for Childs.", exception.Message);
        }

        [Fact]
        public async Task CreateTransactionAsync_ShouldCreateTransaction_WhenValid()
        {
            // Arrange
            var transaction = new Transaction { FundId = 101, CustomerId = "C1", Amount = 1000, Type = "OPENING", ChannelNotification = "EMAIL" };

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
                "Notificación de Transacción",
                It.IsAny<string>(),
                customer.Email
            ), Times.Once);
        }

        [Fact]
        public async Task GetAllTransactionsAsync_ShouldReturnTransactionsList()
        {
            // Arrange
            var transactions = new List<Transaction>
            {
                new Transaction { TransactionId = "1", FundId = 101, CustomerId = "C1", Amount = 1000, Type = "OPENING" },
                new Transaction { TransactionId = "2", FundId = 102, CustomerId = "C2", Amount = 2000, Type = "CLOSURE" }
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
