namespace FundCoreAPI.Models
{
    using Amazon.DynamoDBv2.DataModel;

    [DynamoDBTable("Funds")]
    public class Fund
    {
        [DynamoDBHashKey] // Partition Key
        public string PK { get; set; } = string.Empty; // Example: "FUND#1"
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal MinimumAmount { get; set; }
        public string Category { get; set; } = string.Empty; // Example: "FPV" or "FIC"
    }
}
