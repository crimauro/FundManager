namespace FundCoreAPI.Models
{
    using Amazon.DynamoDBv2.DataModel;

    /// <summary>
    /// Represents a transaction entity stored in the DynamoDB "Transactions" table.
    /// </summary>
    [DynamoDBTable("Transactions")]
    public class Transaction
    {
        /// <summary>
        /// Gets or sets the partition key (Primary Key) for the transaction.
        /// </summary>
        [DynamoDBHashKey] // Partition Key
        public string PK { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the unique identifier for the transaction.
        /// </summary>
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the identifier of the customer associated with the transaction.
        /// </summary>
        public required string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the fund associated with the transaction.
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Gets or sets the name of the fund associated with the transaction.
        /// </summary>
        public string? FundName { get; set; }

        /// <summary>
        /// Gets or sets the type of the transaction (e.g., "OPENING" or "CLOSURE").
        /// </summary>
        public string OperationType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the amount involved in the transaction.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of when the transaction occurred.
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the notification channel for the transaction (e.g., "EMAIL" or "SMS").
        /// </summary>
        public string NotificationType { get; set; } = string.Empty;
    }
}
