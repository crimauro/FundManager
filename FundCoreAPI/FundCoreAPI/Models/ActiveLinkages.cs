namespace FundCoreAPI.Models
{
    using Amazon.DynamoDBv2.DataModel;


    /// <summary>
    /// Represents the ActiveLinkages table in DynamoDB.
    /// </summary>
    [DynamoDBTable("ActiveLinkages")]
    public class ActiveLinkages
    {
        /// <summary>
        /// Gets or sets the partition key (PK) for the ActiveLinkages table.
        /// </summary>
        [DynamoDBHashKey] // Partition Key
        public string PK { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the Fund ID associated with the linkage.
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Gets or sets the name of the fund.
        /// </summary>
        public string FundName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Customer ID associated with the linkage.
        /// </summary>
        public required string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the amount linked to the fund.
        /// </summary>
        public decimal LinkedAmount { get; set; }

        /// <summary>
        /// Gets or sets the date when the linkage was created.
        /// </summary>
        public DateTime LinkageDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the category of the linkage (e.g., "FPV" or "FIC").
        /// </summary>
        public string Category { get; set; } = string.Empty; // Example: "FPV" or "FIC"
    }
}
