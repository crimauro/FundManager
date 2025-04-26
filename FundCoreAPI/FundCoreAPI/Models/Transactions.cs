namespace FundCoreAPI.Models
{
    using Amazon.DynamoDBv2.DataModel;

    [DynamoDBTable("Transactions")]
    public class Transaction
    {
        [DynamoDBHashKey] // Partition Key
        public string PK { get; set; } = Guid.NewGuid().ToString();
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();

        public required string CustomerId { get; set; }
        public int FundId { get; set; }
        public string Type { get; set; } = string.Empty; // Example: "OPENING" or "CLOSURE"
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string ChannelNotification { get; set; } = string.Empty; // Example: "EMAIL" or "SMS"
    }
}
