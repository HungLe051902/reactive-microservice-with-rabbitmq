using OrderService.Services;
using OrderService.Services.Interfaces;
using Plain.RabbitMQ;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddSingleton<IOrderDetailsProvider>(new OrderDetailsProvider("abc"));
builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(builder.Configuration.GetValue<string>("RabbitMQ:Url")));
builder.Services.AddScoped<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(), "report_exchange", ExchangeType.Topic));

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
