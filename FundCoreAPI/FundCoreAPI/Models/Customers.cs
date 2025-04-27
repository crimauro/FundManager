namespace FundCoreAPI.Models
{
    using Amazon.DynamoDBv2.DataModel;

    /// <summary>
    /// Represents a customer entity stored in the DynamoDB "Customers" table.
    /// </summary>
    [DynamoDBTable("Customers")]
    public class Customers
    {
        /// <summary>
        /// Gets or sets the partition key (Primary Key) for the customer.
        /// </summary>
        [DynamoDBHashKey] // Partition Key
        public string PK { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the identification number of the customer.
        /// </summary>
        public required string IdentificationNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the available balance of the customer.
        /// </summary>
        public decimal AvailableBalance { get; set; }

        /// <summary>
        /// Gets or sets the email address of the customer.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone number of the customer.
        /// </summary>
        public string Phone { get; set; } = string.Empty;
    }
}
