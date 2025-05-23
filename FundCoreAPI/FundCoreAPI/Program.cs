using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
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
builder.WebHost.UseUrls("http://*:60721");

/// <summary>
/// Load credentials.
/// </summary>
var awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
var awsRegion = Environment.GetEnvironmentVariable("AWS_REGION");

if (string.IsNullOrEmpty(awsAccessKey) || string.IsNullOrEmpty(awsSecretKey) || string.IsNullOrEmpty(awsRegion))
{
    throw new Exception("AWS environment variables are not set.");
}

/// <summary>
/// AWS Configuration.
/// </summary>
AwsSettings awsOptions = new AwsSettings 
{   AccessKey  = awsAccessKey, 
    SecretKey = awsSecretKey, 
    Region = awsRegion 
};

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
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() 
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
app.UseCors("AllowAll");

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
