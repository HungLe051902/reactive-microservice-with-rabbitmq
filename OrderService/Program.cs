using OrderService;
using OrderService.Services;
using OrderService.Services.Interfaces;
using Plain.RabbitMQ;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Build a config object, using env vars and JSON providers.
IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Order service",
        Version = "v1"
    });
});

builder.Services.AddSingleton<IOrderDetailsProvider>(new OrderDetailsProvider(connectionString));
builder.Services.AddSingleton<IOrderCreator>(x => new OrderCreator(connectionString, x.GetService<ILogger<OrderCreator>>()!));
builder.Services.AddSingleton<IOrderDeletor>(new OrderDeletor(connectionString));

builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(builder.Configuration.GetValue<string>("RabbitMQ:Url")));
builder.Services.AddSingleton<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(),
                    "order_exchange",
                    ExchangeType.Topic));
builder.Services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(),
    "inventory_exchange",
    "inventory_response",
    "inventory.response",
    ExchangeType.Topic));
builder.Services.AddHostedService<InventoryResponseListener>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
