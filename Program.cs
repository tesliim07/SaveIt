using FoodSaver.Contexts;
using FoodSaver.Repositories;
using FoodSaver.Repositories.Interfaces;
using FoodSaver.Services;
using FoodSaver.Services.Interfaces;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add Hangfire configuration
builder.Services.AddHangfire(config => config.UsePostgreSqlStorage(
    options =>
    {
        options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    ));

//Add Hangfire server to process jobs
builder.Services.AddHangfireServer();

// Add services to the container.
builder.Services.AddDbContext<FoodSaverDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IFoodSaverRepository, FoodSaverRepository>();
builder.Services.AddScoped<IFoodSaverService, FoodSaverService>();

builder.Services.AddControllers()
    //Allows the conversion of enum from numbers to the actual options text,
    //as C# by default treats enum as numbers when converting to or from JSON.
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Hangfire Dashboard
app.UseHangfireDashboard("/hangfire");

//Create recurring job (once app starts)
RecurringJob.AddOrUpdate<IFoodSaverService>(
    "find_expiring_foods",                    // Job ID
    s => s.SendFoodExpiryReminder(3),              // Method to run
    Cron.Daily(11));                                   // Schedule: every day

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
