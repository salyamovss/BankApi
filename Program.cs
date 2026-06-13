using BankApi.Common;
using BankApi.Data;
using BankApi.Mappers;
using BankApi.Services;
using BankApi.dal.Repositories;
using BankApi.dal.Repositories.impl;
using BankApi.Services.job;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
    ));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

// Мапперы
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<AccountMapper>();
builder.Services.AddScoped<CardMapper>();
builder.Services.AddScoped<TransactionMapper>();
builder.Services.AddScoped<PhoneMapper>();
// Репозитории
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IPhoneRepository, PhoneRepository>();

// Сервисы
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<CardService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<PhoneService>();
builder.Services.AddHostedService<CurrencyUpdateJob>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.EnsureCreatedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();