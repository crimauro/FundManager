namespace FundCoreAPI.Configuration
{
    using Amazon.DynamoDBv2.Model;
    using Amazon.DynamoDBv2;
    public class DynamoDBTableCreator
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;

        public DynamoDBTableCreator(IAmazonDynamoDB dynamoDbClient)
        {
            _dynamoDbClient = dynamoDbClient;
        }

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
            // Esperar a que la tabla esté activa antes de continuar
            await WaitForTableToBeActiveAsync(tableName);
        }

        public async Task InsertFundsDataAsync()
        {
            var tableName = "Funds";

            // Verificar si la tabla existe antes de insertar datos
            if (!await CheckExistsTableAsync(tableName)) 
            {
                Console.WriteLine($"La tabla '{tableName}' no existe. No se pueden insertar datos.");
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
                Console.WriteLine($"Registro insertado: {fund["PK"].S}");
            }
        }

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

            // Esperar a que la tabla esté activa antes de continuar
            await WaitForTableToBeActiveAsync(tableName);
        }

        public async Task InsertCustomersDataAsync()
        {
            var tableName = "Customers";

            // Verificar si la tabla existe antes de insertar datos
            if (!await CheckExistsTableAsync(tableName))
            {
                Console.WriteLine($"La tabla '{tableName}' no existe. No se pueden insertar datos.");
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
                Console.WriteLine($"Registro insertado: {customer["PK"].S}");
            }
        }

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

        private async Task<bool> CheckExistsTableAsync(string tableName)
        {
            var response = await _dynamoDbClient.ListTablesAsync();
            return response.TableNames.Contains(tableName);
        }

        private async Task WaitForTableToBeActiveAsync(string tableName)
        {
            Console.WriteLine($"Esperando a que la tabla '{tableName}' esté en estado 'ACTIVE'...");
            while (true)
            {
                var response = await _dynamoDbClient.DescribeTableAsync(new DescribeTableRequest
                {
                    TableName = tableName
                });

                if (response.Table.TableStatus == TableStatus.ACTIVE)
                {
                    Console.WriteLine($"La tabla '{tableName}' está ahora en estado 'ACTIVE'.");
                    break;
                }

                await Task.Delay(1000); // Esperar 1 segundo antes de volver a verificar
            }
        }


    }
}
