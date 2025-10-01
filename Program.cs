using Microsoft.EntityFrameworkCore;
using okenovTest.Entity;
using okenovTest.Repositories;
using okenovTest.Repositories.impl;
using okenovTest.Services;
using okenovTest.Services.impl;
using System.Text;
using System.Security.Cryptography;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IUserSessionRepository, EfUserSessionRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddMemoryCache();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<AppDbContext>();

    int retries = 10;
    while (retries > 0)
    {
        try
        {
            logger.LogInformation("Trying to apply migrations. Attempt {Attempt}", 10 - retries + 1);
            context.Database.Migrate();
            logger.LogInformation("Migrations applied successfully.");
            break;
        }
        catch (Exception ex)
        {
            retries--;
            logger.LogError(ex, "Failed to apply migrations. Retries left: {Retries}", retries);
            if (retries == 0) throw;
            Thread.Sleep(5000);
        }
    }
}

app.Urls.Add("http://*:5000");
app.Urls.Add("http://*:5001");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
