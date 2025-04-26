namespace FundCoreAPI.Models
{
    using Amazon.DynamoDBv2.DataModel;

    [DynamoDBTable("Customers")]
    public class Customers
    {
        [DynamoDBHashKey] // Partition Key
        public string PK { get; set; } = Guid.NewGuid().ToString();

        public required string IdentificationNumber { get; set; }

        public required string Name { get; set; }

        public decimal AvailableBalance { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
