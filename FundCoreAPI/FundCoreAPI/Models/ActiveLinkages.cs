namespace FundCoreAPI.Models
{
    using Amazon.DynamoDBv2.DataModel;


    [DynamoDBTable("ActiveLinkages")]
    public class ActiveLinkages
    {
        [DynamoDBHashKey] // Partition Key
        public string PK { get; set; } = Guid.NewGuid().ToString();

        public int FundId { get; set; } 

        public required string CustomerId { get; set; }
        public decimal LinkedAmount { get; set; }
        public DateTime LinkageDate { get; set; } = DateTime.UtcNow;
        public string Category { get; set; } = string.Empty; // Example: "FPV" or "FIC"
    }
}
