using Application.Category;
using Application.Category.Ports;
using Application.Customer;
using Application.Customer.Ports;
using Application.Order;
using Application.Order.Ports;
using Application.Payment;
using Application.Payment.Ports;
using Application.Product;
using Application.Product.Ports;
using Data;
using Data.Category;
using Data.Customer;
using Data.Order;
using Data.Product;
using Domain.Category.Ports;
using Domain.Customer.Ports;
using Domain.Order.Ports;
using Domain.Product.Ports;
using Domain.Queue.Ports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Queue.Consumers.Payment;
using Queue.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CustomerManager).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CategoryManager).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProductManager).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(OrderManager).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(PaymentManager).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(PaymentConsumer).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ProductionConsumer).Assembly));

// Register IConfiguration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

# region IoC
builder.Services.AddScoped<ICustomerManager, CustomerManager>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICategoryManager, CategoryManager>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductManager, ProductManager>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderManager, OrderManager>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IQueueRepository, RabbitMQRepository>();
builder.Services.AddScoped<IPaymentManager, PaymentManager>();
# endregion

# region Consumers
builder.Services.AddHostedService<PaymentConsumer>();
builder.Services.AddHostedService<ProductionConsumer>();
# endregion

# region DB wiring up

var connectionString = builder.Configuration.GetConnectionString("Main");
builder.Services.AddDbContext<DataDbContext>(options => options.UseNpgsql(connectionString));
#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<DataDbContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Successfully updated migrations...");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error applying migrations: " + ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection(); // Certifique-se de que essa linha não esteja presente
}

app.UseAuthorization();

app.MapControllers();

app.Run();
