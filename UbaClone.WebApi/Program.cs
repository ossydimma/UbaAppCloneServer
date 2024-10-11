using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UbaClone.WebApi.Data;
using UbaClone.WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { Title = "My Api",
      Version = "v1",
      Description = "An Api for Uba Mobile app clone",
      Contact = new OpenApiContact
      { 
          Name = "Osita Chris",
          Email = "chrisjerry070@gmail.com",
          Url = new Uri ("https://example.com/terms")

      }
    });
});
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

var provider = builder.Services.BuildServiceProvider();
var config = provider.GetRequiredService<IConfiguration>();

builder.Services.AddCors(options =>
{
    var frontendUrl = config.GetValue<string>("frontend_url");

    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendUrl!)
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        builder =>
//        {
//            builder.AllowAnyOrigin()
//                   .AllowAnyMethod()
//                   .AllowAnyHeader();
//        });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers();

// Enable CORS for your React app
//app.UseCors("AllowAll");
app.UseCors();

app.UseAuthorization();

app.Run();
