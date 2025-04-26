namespace FundCoreAPI.Services.Transactions
{
    using FundCoreAPI.Models;
    using FundCoreAPI.Repositories.ActiveLinkages;
    using FundCoreAPI.Repositories.Customers;
    using FundCoreAPI.Repositories.Funds;
    using FundCoreAPI.Repositories.Transactions;
    using FundCoreAPI.Services.Notifications;

    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IFundsRepository _fundsRepository;
        private readonly ICustomersRepository _customersRepository;
        private readonly INotificationService _notificationService;
        private readonly IActiveLinkagesRepository _activeLinkagesRepository;

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
        public TransactionsService(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        public async Task CreateTransactionSimpleAsync(Transaction transaction)
        {
            await _transactionsRepository.CreateTransactionAsync(transaction);
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            // Verificar si el FundId existe en la tabla Funds
            var fund = await _fundsRepository.GetFundByIdAsync(transaction.FundId);
            if (fund == null)
            {
                throw new InvalidOperationException($"El fondo con ID {transaction.FundId} no existe.");
            }

            // Verificar si el CustomerId existe en la tabla Customers
            var customer = await _customersRepository.GetCustomerByIdAsync(transaction.CustomerId);
            if (customer == null)
            {
                throw new InvalidOperationException($"El cliente con ID {transaction.CustomerId} no existe.");
            }

            // Verificar si el Amount es mayor o igual al MinimumAmount del fondo
            if (transaction.Amount < fund.MinimumAmount)
            {
                throw new InvalidOperationException($"El monto debe ser mayor o igual al mínimo requerido del fondo {fund.Name}.");
            }

            if (transaction.Type == "OPENING" && customer.AvailableBalance < transaction.Amount)
            {
                throw new InvalidOperationException($"No tiene saldo disponible para vincularse al fondo {fund.Name}.");
            }

            // Interactuar con la tabla ActiveLinkages
            if (transaction.Type == "OPENING")
            {
                var exists = await _activeLinkagesRepository.GetLinkageByIdAsync(transaction.CustomerId, transaction.FundId);
                if (exists != null)
                {
                    throw new InvalidOperationException($"El fondo {fund.Name} ya tiene una apertura para el cliente {customer.Name}.");
                }

                // Registrar la vinculación
                await _activeLinkagesRepository.CreateLinkageAsync(new ActiveLinkages
                {
                    FundId = transaction.FundId,
                    CustomerId = transaction.CustomerId,
                    LinkedAmount = transaction.Amount,
                    LinkageDate = DateTime.UtcNow,
                    Category = fund.Category
                });
                //Actualiza el monto del cliente
                customer.AvailableBalance -= transaction.Amount;
            }
            else if (transaction.Type == "CLOSURE")
            {
                var exists = await _activeLinkagesRepository.GetLinkageByIdAsync(transaction.CustomerId, transaction.FundId);
                if (exists == null)
                {
                    throw new InvalidOperationException($"El fondo {fund.Name} no tiene una apertura para el cliente {customer.Name}.");
                }

                // Eliminar la vinculación
                await _activeLinkagesRepository.DeleteLinkageAsync(transaction.CustomerId, transaction.FundId);

                //Actualiza el monto del cliente
                customer.AvailableBalance += exists.LinkedAmount;
            }

            await _customersRepository.UpdateCustomerAsync(customer);

            // Guardar la transacción
            await _transactionsRepository.CreateTransactionAsync(transaction);

            // Enviar notificación
            if (transaction.ChannelNotification == "EMAIL")
            {
                await _notificationService.SendEmailAsync(
                    subject: "Notificación de Transacción",
                    message: $"Se ha realizado una transacción de tipo {transaction.Type} por un monto de {transaction.Amount} en el fondo {fund.Name}.",
                    email: customer.Email
                );
            }
            else if (transaction.ChannelNotification == "SMS")
            {
                await _notificationService.SendSmsAsync(
                    message: $"Se ha realizado una transacción de tipo {transaction.Type} por un monto de {transaction.Amount} en el fondo {fund.Name}.",
                    phoneNumber: customer.Phone
                );
            }
        }

        public async Task<Transaction?> GetTransactionByIdAsync(string transactionId)
        {
            return await _transactionsRepository.GetTransactionByIdAsync(transactionId);
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionsRepository.GetAllTransactionsAsync();
        }

        public async Task<List<Transaction>> GetTransactionsByFundIdAsync(int fundId)
        {
            return await _transactionsRepository.GetTransactionsByFundIdAsync(fundId);
        }

        public async Task<List<Transaction>> GetTransactionsByCustomerIddAsync(int customerId)
        {
            return await _transactionsRepository.GetTransactionsByCustomerIddAsync(customerId);
        }

        public async Task DeleteTransactionAsync(string transactionId)
        {
            await _transactionsRepository.DeleteTransactionAsync(transactionId);
        }
    }
}
