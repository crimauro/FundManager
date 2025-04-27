namespace FundCoreAPI.Configuration
{
    using Amazon.DynamoDBv2.Model;
    using Amazon.DynamoDBv2;
    /// <summary>
    /// Provides methods to create and manage DynamoDB tables and their data.
    /// </summary>
    public class DynamoDBTableCreator
    {
        /// <summary>
        /// The DynamoDB client used to interact with the database.
        /// </summary>
        private readonly IAmazonDynamoDB _dynamoDbClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoDBTableCreator"/> class.
        /// </summary>
        /// <param name="dynamoDbClient">The DynamoDB client.</param>
        public DynamoDBTableCreator(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

        /// <summary>
        /// Creates the "Funds" table if it does not already exist.
        /// </summary>
        public async Task CreateFundsTableAsync()
        {
            var tableName = "Funds";

            if (await CheckExistsTableAsync(tableName))
            {
                return;
            }

            // Create the table if it does not exist
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                   {
                       new AttributeDefinition("PK", "S")
                   },
                KeySchema = new List<KeySchemaElement>
                   {
                       new KeySchemaElement("PK", KeyType.HASH) // Partition Key
                   },
                ProvisionedThroughput = new ProvisionedThroughput(5, 5)
            };

            await _dynamoDbClient.CreateTableAsync(request);
            Console.WriteLine($"Table '{tableName}' has been created successfully.");
            // Wait for the table to become active before proceeding
            await WaitForTableToBeActiveAsync(tableName);
        }

        /// <summary>
        /// Inserts predefined data into the "Funds" table.
        /// </summary>
        public async Task InsertFundsDataAsync()
        {
            var tableName = "Funds";

            // Check if the table exists before inserting data
            if (!await CheckExistsTableAsync(tableName))
            {
                Console.WriteLine($"The table '{tableName}' does not exist. Data cannot be inserted.");
                return;
            }

            var fundsData = new List<Dictionary<string, AttributeValue>>
               {
                   new Dictionary<string, AttributeValue>
                   {
                       { "PK", new AttributeValue { S = "1" } },
                       { "Id", new AttributeValue { N = "1" } },
                       { "Name", new AttributeValue { S = "FPV_EL_CLIENTE_RECAUDADORA" } },
                       { "MinimumAmount", new AttributeValue { N = "75000" } },
                       { "Category", new AttributeValue { S = "FPV" } }
                   },
                   new Dictionary<string, AttributeValue>
                   {
                       { "PK", new AttributeValue { S = "2" } },
                       { "Id", new AttributeValue { N = "2" } },
                       { "Name", new AttributeValue { S = "FPV_EL_CLIENTE_ECOPETROL" } },
                       { "MinimumAmount", new AttributeValue { N = "125000" } },
                       { "Category", new AttributeValue { S = "FPV" } }
                   },
                   new Dictionary<string, AttributeValue>
                   {
                       { "PK", new AttributeValue { S = "3" } },
                       { "Id", new AttributeValue { N = "3" } },
                       { "Name", new AttributeValue { S = "DEUDAPRIVADA" } },
                       { "MinimumAmount", new AttributeValue { N = "50000" } },
                       { "Category", new AttributeValue { S = "FIC" } }
                   },
                   new Dictionary<string, AttributeValue>
                   {
                       { "PK", new AttributeValue { S = "4" } },
                       { "Id", new AttributeValue { N = "4" } },
                       { "Name", new AttributeValue { S = "FDO-ACCIONES" } },
                       { "MinimumAmount", new AttributeValue { N = "250000" } },
                       { "Category", new AttributeValue { S = "FIC" } }
                   },
                   new Dictionary<string, AttributeValue>
                   {
                       { "PK", new AttributeValue { S = "5" } },
                       { "Id", new AttributeValue { N = "5" } },
                       { "Name", new AttributeValue { S = "FPV_EL_CLIENTE_DINAMICA" } },
                       { "MinimumAmount", new AttributeValue { N = "100000" } },
                       { "Category", new AttributeValue { S = "FPV" } }
                   }
               };

            foreach (var fund in fundsData)
            {
                var request = new PutItemRequest
                {
                    TableName = tableName,
                    Item = fund
                };

                await _dynamoDbClient.PutItemAsync(request);
                Console.WriteLine($"Record inserted: {fund["PK"].S}");
            }
        }

        /// <summary>
        /// Creates the "Customers" table if it does not already exist.
        /// </summary>
        public async Task CreateCustomersTableAsync()
        {
            var tableName = "Customers";

            if (await CheckExistsTableAsync(tableName))
            {
                return;
            }

            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                   {
                       new AttributeDefinition("PK", "S"),
                   },
                KeySchema = new List<KeySchemaElement>
                   {
                       new KeySchemaElement("PK", KeyType.HASH), // Partition Key
                   },
                ProvisionedThroughput = new ProvisionedThroughput(5, 5)
            };

            await _dynamoDbClient.CreateTableAsync(request);

            // Wait for the table to become active before proceeding
            await WaitForTableToBeActiveAsync(tableName);
        }

        /// <summary>
        /// Inserts predefined data into the "Customers" table.
        /// </summary>
        public async Task InsertCustomersDataAsync()
        {
            var tableName = "Customers";

            // Check if the table exists before inserting data
            if (!await CheckExistsTableAsync(tableName))
            {
                Console.WriteLine($"The table '{tableName}' does not exist. Data cannot be inserted.");
                return;
            }

            var customersData = new List<Dictionary<string, AttributeValue>>
               {
                   new Dictionary<string, AttributeValue>
                   {
                       { "PK", new AttributeValue { S =  "1111" } },
                       { "IdentificationNumber", new AttributeValue { S = "1111" } },
                       { "Name", new AttributeValue { S = "Jose" } },
                       { "AvailableBalance", new AttributeValue { N = "300000.50" } },
                       { "Email", new AttributeValue { S = "customer1@example.com" } },
                       { "Phone", new AttributeValue { S = "1234567890" } }
                   },
                   new Dictionary<string, AttributeValue>
                   {
                       { "PK", new AttributeValue { S = "2222" } },
                       { "IdentificationNumber", new AttributeValue { S = "2222" } },
                       { "Name", new AttributeValue { S = "Diana" } },
                       { "AvailableBalance", new AttributeValue { N = "50000.75" } },
                       { "Email", new AttributeValue { S = "customer2@example.com" } },
                       { "Phone", new AttributeValue { S = "0987654321" } }
                   },
                   new Dictionary<string, AttributeValue>
                   {
                       { "PK", new AttributeValue { S = "3333"} },
                       { "IdentificationNumber", new AttributeValue { S = "3333" } },
                       { "Name", new AttributeValue { S = "Efren" } },
                       { "AvailableBalance", new AttributeValue { N = "145000.00" } },
                       { "Email", new AttributeValue { S = "customer3@example.com" } },
                       { "Phone", new AttributeValue { S = "1122334455" } }
                   },
                   new Dictionary<string, AttributeValue>
                   {
                       { "PK", new AttributeValue { S = "4444"} },
                       { "IdentificationNumber", new AttributeValue { S = "4444" } },
                       { "Name", new AttributeValue { S = "Margaret" } },
                       { "AvailableBalance", new AttributeValue { N = "280000.00" } },
                       { "Email", new AttributeValue { S = "customer4@example.com" } },
                       { "Phone", new AttributeValue { S = "5566778899" } }
                   },
                   new Dictionary<string, AttributeValue>
                   {
                       { "PK", new AttributeValue { S = "5555" } },
                       { "IdentificationNumber", new AttributeValue { S = "5555" } },
                       { "Name", new AttributeValue { S = "James" } },
                       { "AvailableBalance", new AttributeValue { N = "90000.25" } },
                       { "Email", new AttributeValue { S = "customer5@example.com" } },
                       { "Phone", new AttributeValue { S = "6677889900" } }
                   }
               };

            foreach (var customer in customersData)
            {
                var request = new PutItemRequest
                {
                    TableName = tableName,
                    Item = customer
                };

                await _dynamoDbClient.PutItemAsync(request);
                Console.WriteLine($"Record inserted: {customer["PK"].S}");
            }
        }

        /// <summary>
        /// Creates the "Transactions" table if it does not already exist.
        /// </summary>
        public async Task CreateTransactionsTableAsync()
        {
            var tableName = "Transactions";

            if (await CheckExistsTableAsync(tableName))
            {
                return;
            }

            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                   {
                       new AttributeDefinition("PK", "S"),
                       new AttributeDefinition("FundId", "N"),
                       new AttributeDefinition("Timestamp", "S")
                   },
                KeySchema = new List<KeySchemaElement>
                   {
                       new KeySchemaElement("PK", KeyType.HASH), // Partition Key
                   },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                   {
                       new GlobalSecondaryIndex
                       {
                           IndexName = "TransactionsByFund",
                           KeySchema = new List<KeySchemaElement>
                           {
                               new KeySchemaElement("FundId", KeyType.HASH),
                               new KeySchemaElement("Timestamp", KeyType.RANGE)
                           },
                           Projection = new Projection { ProjectionType = ProjectionType.ALL },
                           ProvisionedThroughput = new ProvisionedThroughput(5, 5)
                       }
                   },
                ProvisionedThroughput = new ProvisionedThroughput(5, 5)
            };

            await _dynamoDbClient.CreateTableAsync(request);
        }

        /// <summary>
        /// Creates the "ActiveLinkages" table if it does not already exist.
        /// </summary>
        public async Task CreateActiveLinkagesTableAsync()
        {
            var tableName = "ActiveLinkages";

            if (await CheckExistsTableAsync(tableName))
            {
                return;
            }

            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>
                   {
                       new AttributeDefinition("PK", "S"),
                       new AttributeDefinition("Category", "S")
                   },
                KeySchema = new List<KeySchemaElement>
                   {
                       new KeySchemaElement("PK", KeyType.HASH), // Partition Key
                   },
                GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                   {
                       new GlobalSecondaryIndex
                       {
                           IndexName = "LinkagesByCategory",
                           KeySchema = new List<KeySchemaElement>
                           {
                               new KeySchemaElement("PK", KeyType.HASH),
                               new KeySchemaElement("Category", KeyType.RANGE)
                           },
                           Projection = new Projection { ProjectionType = ProjectionType.ALL },
                           ProvisionedThroughput = new ProvisionedThroughput(5, 5)
                       }
                   },
                ProvisionedThroughput = new ProvisionedThroughput(5, 5)
            };

            await _dynamoDbClient.CreateTableAsync(request);
        }

        /// <summary>
        /// Checks if a table with the specified name exists in DynamoDB.
        /// </summary>
        /// <param name="tableName">The name of the table to check.</param>
        /// <returns>True if the table exists; otherwise, false.</returns>
        private async Task<bool> CheckExistsTableAsync(string tableName)
        {
            var response = await _dynamoDbClient.ListTablesAsync();
            return response.TableNames.Contains(tableName);
        }

        /// <summary>
        /// Waits for a table to reach the "ACTIVE" state before proceeding.
        /// </summary>
        /// <param name="tableName">The name of the table to wait for.</param>
        private async Task WaitForTableToBeActiveAsync(string tableName)
        {
            Console.WriteLine($"Waiting for the table '{tableName}' to reach 'ACTIVE' state...");
            while (true)
            {
                var response = await _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
                {
                    TableName = tableName
                });

                if (response.Table.TableStatus == TableStatus.ACTIVE)
                {
                    Console.WriteLine($"The table '{tableName}' is now in 'ACTIVE' state.");
                    break;
                }

                await Task.Delay(1000); // Wait 1 second before checking again
            }
        }
    }
}
