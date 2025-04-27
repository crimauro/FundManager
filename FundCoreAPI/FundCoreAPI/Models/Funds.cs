namespace FundCoreAPI.Models
{
    using Amazon.DynamoDBv2.DataModel;

    /// <summary>
    /// Represents a fund entity stored in the DynamoDB table "Funds".
    /// </summary>
    [DynamoDBTable("Funds")]
    public class Fund
    {
        /// <summary>
        /// Gets or sets the partition key for the DynamoDB table.
        /// Example: "FUND#1".
        /// </summary>
        [DynamoDBHashKey] // Partition Key
        public string PK { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique identifier of the fund.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the fund.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the minimum amount required for the fund.
        /// </summary>
        public decimal MinimumAmount { get; set; }

        /// <summary>
        /// Gets or sets the category of the fund.
        /// Example: "FPV" or "FIC".
        /// </summary>
        public string Category { get; set; } = string.Empty;
    }
}
