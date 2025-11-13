using Amazon.SimpleSystemsManagement.Model;
using Amazon.SimpleSystemsManagement;
using Amazon;
using FreshSourceAPI.Data;
using FreshSourceAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Read region from env
var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? "us-east-1";

// Create SSM client
var ssm = new AmazonSimpleSystemsManagementClient(RegionEndpoint.GetBySystemName(region));

// Helper function
async Task<string> GetParameterAsync(string name)
{
    var response = await ssm.GetParameterAsync(new GetParameterRequest
    {
        Name = name,
        WithDecryption = true
    });

    return response.Parameter.Value;
}

var dbHost = Environment.GetEnvironmentVariable("FRESHSOURCE_DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("FRESHSOURCE_DB_PORT") ?? "1433";
var dbUser = Environment.GetEnvironmentVariable("FRESHSOURCE_DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("FRESHSOURCE_DB_PASSWORD");
var dbName = Environment.GetEnvironmentVariable("FRESHSOURCE_DB_NAME");

if (string.IsNullOrWhiteSpace(dbHost) ||
    string.IsNullOrWhiteSpace(dbUser) ||
    string.IsNullOrWhiteSpace(dbPassword) ||
    string.IsNullOrWhiteSpace(dbName))
{
    throw new InvalidOperationException("Database connection string is not configured.");
}

var connectionString = $"Server={dbHost},{dbPort};Database={dbName};User Id={dbUser};Password={dbPassword};Encrypt=False";

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();