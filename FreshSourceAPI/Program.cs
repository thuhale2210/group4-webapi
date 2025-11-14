using Amazon;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using FreshSourceAPI.Data;
using FreshSourceAPI.Profiles;
using FreshSourceAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Read region from env (default to us-east-1)
var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? "us-east-1";

// Create SSM client
var ssm = new AmazonSimpleSystemsManagementClient(RegionEndpoint.GetBySystemName(region));

// Helper: get required env var
string GetRequiredEnv(string name) =>
    Environment.GetEnvironmentVariable(name)
    ?? throw new InvalidOperationException($"Environment variable '{name}' is not set");

// Helper: read a parameter *value* from SSM given the env var name
async Task<string> GetParameterFromEnvAsync(string envVarName)
{
    var parameterName = GetRequiredEnv(envVarName);

    var response = await ssm.GetParameterAsync(new GetParameterRequest
    {
        Name = parameterName,
        WithDecryption = true
    });

    return response.Parameter.Value;
}

// --- actually fetch values from SSM ---
var dbHost = await GetParameterFromEnvAsync("PARAM_DB_HOST");
var dbPort = await GetParameterFromEnvAsync("PARAM_DB_PORT");
var dbUser = await GetParameterFromEnvAsync("PARAM_DB_USER");
var dbPassword = await GetParameterFromEnvAsync("PARAM_DB_PASSWORD");
var dbName = await GetParameterFromEnvAsync("PARAM_DB_NAME");

// Build connection string
var connectionString =
    $"Server=tcp:{dbHost},{dbPort};" +
    $"Database={dbName};" +
    $"User Id={dbUser};" +
    $"Password={dbPassword};" +
    "Encrypt=False;TrustServerCertificate=True;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();