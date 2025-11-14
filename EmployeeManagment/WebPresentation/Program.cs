using BAL.Helpers;
using BAL.Services;
using BAL.Services.Interfaces;
using DAL.Connection;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using WebPresentation.Service;
using WebPresentation.Service.Interfaces;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

        builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
        builder.Services.AddScoped<CompanyService>();

        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IEmployeeService, EmployeeService>();

        builder.Services.AddScoped<IEmployeeInfoRepository, EmployeeInfoRepository>();
        builder.Services.AddScoped<IEmployeeInfoService, EmployeeInfoService>();

        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        builder.Services.AddScoped<IDepartmentService, DepartmentService>();

        builder.Services.AddScoped<IPositionRepository, PositionRepository>();
        builder.Services.AddScoped<IPositionService, PositionService>();

        builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
        builder.Services.AddScoped<ICompanyService, CompanyService>();

        builder.Services.AddScoped<IEmployeeAppService, EmployeeAppService>();
        builder.Services.AddScoped<ISalaryReportService, SalaryReportService>();

        builder.Services.AddControllersWithViews();

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

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
            pattern: "{controller=Company}/{action=Index}/{id?}");

        app.Run();
    }
}