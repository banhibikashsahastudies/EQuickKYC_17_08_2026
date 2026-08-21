using eKyc.API.Middleware;
using EQuickKYC.API.Middleware;
using EQuickKYC.Application.Interfaces;
using EQuickKYC.Application.Service;
using EQuickKYC.Domain.RepoContracts;
using EQuickKYC.Infrastructure.Data;
using EQuickKYC.Infrastructure.Security;
using EQuickKYC.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddOpenApi();

//dbcontext
builder.Services.AddDbContext<EQuickKYCDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
        policy.AllowAnyOrigin();
    });
});

//services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepoService, UserRepoService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<AppBankService>();

builder.Services.AddScoped<IMobileOTPService, EQuickKYC.Infrastructure.Services.MobileOTPService>();
builder.Services.AddScoped<EQuickKYC.Application.Service.MobileOTPService>();

builder.Services.AddScoped<IEmailOTPService, EmailOTPService>();
builder.Services.AddScoped<EmailOTPServiceApplication>();

builder.Services.AddScoped<IPanRegistrationService, PanRegistrationService>();
builder.Services.AddScoped<PanService>();

//Security
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IHashService, HashService>();

// API Error Log service
builder.Services.AddScoped<IApiErrorLogService, ApiErrorLogService>();
builder.Services.AddScoped<ClientAdminService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        // Keeps the internal JSON definition mapped correctly
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
        options.RoutePrefix = "cazaayan-api-docs";
    });
}

//swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(options =>
    //{
    //    // Keeps the internal JSON definition mapped correctly
    //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
    //    options.RoutePrefix = "cazaayan-api-docs";
    //});
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseHsts();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
