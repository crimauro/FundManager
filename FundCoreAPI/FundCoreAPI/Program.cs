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
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Load secrets from AWS Secrets Manager.
/// </summary>
var secretsManagerClient = new AmazonSecretsManagerClient();
var secretName = "FundManagerAppSecrets"; // Secret name
var secretValueResponse = await secretsManagerClient.GetSecretValueAsync(new GetSecretValueRequest
{
    SecretId = secretName
});

if (secretValueResponse.SecretString != null)
{
    /// <summary>
    /// Add secrets to the configuration.
    /// </summary>
    var secrets = new ConfigurationBuilder()
        .AddJsonStream(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(secretValueResponse.SecretString)))
        .Build();

    builder.Configuration.AddConfiguration(secrets);
}

/// <summary>
/// AWS Configuration.
/// </summary>
AwsSettings awsOptions = builder.Configuration.GetSection("AWS").Get<AwsSettings>()!;

/// <summary>
/// Add services to the container.
/// </summary>
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // URL Angular Application
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

/// <summary>
/// Configure Amazon DynamoDB client.
/// </summary>
var config = new AmazonDynamoDBConfig
{
    RegionEndpoint = RegionEndpoint.GetBySystemName(awsOptions.Region)
};

builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    return new AmazonDynamoDBClient(awsOptions.AccessKey, awsOptions.SecretKey, config);
});

/// <summary>
/// Create and initialize DynamoDB tables.
/// </summary>
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

// Use CORS
app.UseCors("AllowAngularApp");

/// <summary>
/// Configure the HTTP request pipeline.
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

/// <summary>
/// Map controllers and default route.
/// </summary>
app.MapControllers();
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.Run();
