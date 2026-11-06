using AgriStream.Services;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var storageConnection =
    builder.Configuration.GetConnectionString("AzureStorage")
    ?? throw new InvalidOperationException(
        "Azure Storage connection string is missing.");

builder.Services.AddSingleton(
    new BlobServiceClient(storageConnection));

builder.Services.AddSingleton(
    new TableServiceClient(storageConnection));

builder.Services.AddScoped<BlobStorageService>();
builder.Services.AddScoped<TelemetryStorageService>();
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
