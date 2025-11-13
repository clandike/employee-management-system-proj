using BAL.Services;
using DAL.Connection;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IReadableCompanyRepository, CompanyRepository>();
builder.Services.AddScoped<CompanyService>();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

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