using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using FundCoreAPI.Configuration;
using FundCoreAPI.Repositories.ActiveLinkages;
using FundCoreAPI.Repositories.Customers;
using FundCoreAPI.Repositories.Funds;
using FundCoreAPI.Repositories.Transactions;
using FundCoreAPI.Services.ActiveLinkages;
using FundCoreAPI.Services.Customers;
using FundCoreAPI.Services.Funds;
using FundCoreAPI.Services.Transactions;

var builder = WebApplication.CreateBuilder(args);

// Cargar secretos desde AWS Secrets Manager
var secretsManagerClient = new AmazonSecretsManagerClient();
var secretName = "FundManagerAppSecrets"; // Nombre del secreto
var secretValueResponse = await secretsManagerClient.GetSecretValueAsync(new GetSecretValueRequest
{
    SecretId = secretName
});

if (secretValueResponse.SecretString != null)
{
    var secrets = new ConfigurationBuilder()
        .AddJsonStream(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(secretValueResponse.SecretString)))
        .Build();

    builder.Configuration.AddConfiguration(secrets);
}

// AWS Configuration
AwsSettings awsOptions = builder.Configuration.GetSection("AWS").Get<AwsSettings>()!;



// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new AmazonDynamoDBConfig
{
    RegionEndpoint = RegionEndpoint.GetBySystemName(awsOptions.Region)
};

builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    return new AmazonDynamoDBClient(awsOptions.AccessKey, awsOptions.SecretKey, config);
});

var tableCreator = new DynamoDBTableCreator(new AmazonDynamoDBClient(awsOptions.AccessKey, awsOptions.SecretKey, config));
await tableCreator.CreateFundsTableAsync();
await tableCreator.InsertFundsDataAsync();
await tableCreator.CreateCustomersTableAsync();
await tableCreator.InsertCustomersDataAsync();
await tableCreator.CreateTransactionsTableAsync();
await tableCreator.CreateActiveLinkagesTableAsync();

builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddScoped<IFundsRepository, FundsRepository>();
builder.Services.AddScoped<IFundsService, FundsService>();
builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
builder.Services.AddScoped<ITransactionsService, TransactionsService>();
builder.Services.AddScoped<ICustomersRepository, CustomersRepository>();
builder.Services.AddScoped<ICustomersService, CustomersService>();
builder.Services.AddScoped<IActiveLinkagesRepository, ActiveLinkagesRepository>();
builder.Services.AddScoped<IActiveLinkagesService, ActiveLinkagesService>();
builder.Services.AddAwsNotificationService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.Run();
