using BuyurtmaGo.Core;
using BuyurtmaGo.Core.Authentications;
using BuyurtmaGo.Core.Authentications.Entities;
using BuyurtmaGo.Core.Authentications.Options;
using BuyurtmaGo.Core.Extentions;
using BuyurtmaGo.Core.Interfaces;
using BuyurtmaGo.Core.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddDbContext<BuyurtmaGoDb>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(BuyurtmaGoDb)), o =>
    {
        o.MigrationsHistoryTable("migrations", "sys");
    });
    options.UseSnakeCaseNamingConvention();
});

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<BuyurtmaGoDb>()
    .AddDefaultTokenProviders();

builder.Services.AddAuth(builder.Configuration)
    .AddDefaultAuthorization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<JwtTokenReader>();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.Configure<TokenGenerationOptions>(builder.Configuration.GetSection(nameof(TokenGenerationOptions)));

builder.Services.AddScoped<AuthManager>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
