using AspNetWebApiWithDbContext.DataProvider;
using AspNetWebApiWithDbContext.Domain;
using AspNetWebApiWithDbContext.Middlewares;
using AspNetWebApiWithDbContext.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString)
    .EnableSensitiveDataLogging());

builder.Services.AddScoped<IDbSeeder,DbSeeder>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped(typeof(GenericRepository<>));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<MyDbContext>();
        context.Database.Migrate();

        var seeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
        seeder.Seed();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error On Migration and Seed : {0}",ex.ToString());
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<TraceIdMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
