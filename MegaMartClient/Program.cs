using MegaMartClient.Services;
using MegaMartClient.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Bind SmartStockApi section
builder.Services.Configure<SmartStockApiOptions>(
    builder.Configuration.GetSection("SmartStockApi"));

// Typed HttpClient for API calls
builder.Services.AddHttpClient<ISmartStockApiClient, SmartStockApiClient>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); 
