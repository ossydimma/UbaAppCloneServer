using Microsoft.EntityFrameworkCore;
using UbaClone.WebApi.Data;
using UbaClone.WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

});

//builder.Services.AddStackExchangeRedisCache(redisOptions =>
//{
//    string? connection = builder.Configuration
//    .GetConnectionString("Redis");
//    redisOptions.Configuration = connection;
//});
// Load Redis settings from appsettings.json
var redisSettings = builder.Configuration.GetSection("RedisCacheSettings");
var redisHost = redisSettings.GetValue<string>("Host");
var redisPort = redisSettings.GetValue<int>("Port");

// Configure Redis as a distributed cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = $"{redisHost}:{redisPort}";
    options.InstanceName = "myDb_";  // Optional: Prefix all cache keys with this instance name
});

var app = builder.Build();

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
