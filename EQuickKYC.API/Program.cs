using EQuickKYC.Application.Interfaces;
using EQuickKYC.Application.Service;
using EQuickKYC.Infrastructure.Data;
using EQuickKYC.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//dbcontext
//builder.Services.AddDbContext<EQuickKYCDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

//Console.WriteLine("=================================");
//Console.WriteLine($"Connection String: {connectionString}");
//Console.WriteLine("=================================");

builder.Services.AddDbContext<EQuickKYCDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});



//services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<AppBankService>();

builder.Services.AddScoped<IMobileOTPService, EQuickKYC.Infrastructure.Services.MobileOTPService>();
builder.Services.AddScoped<EQuickKYC.Application.Service.MobileOTPService>();

builder.Services.AddScoped<IEmailOTPService, EmailOTPService>();
builder.Services.AddScoped<EmailOTPServiceApplication>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
