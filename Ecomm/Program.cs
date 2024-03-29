using Ecomm;
using Ecomm.DataAccess;
using Plain.RabbitMQ;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Build a config object, using env vars and JSON providers.
IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

string connectionString = config.GetConnectionString("DefaultConnection")!;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IProductProvider>(new ProductProvider(connectionString));
builder.Services.AddSingleton<IInventoryUpdator>(new InventoryUpdator(connectionString));

builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(builder.Configuration.GetValue<string>("RabbitMQ:Url")));
builder.Services.AddSingleton<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(), "inventory_exchange", ExchangeType.Topic));
builder.Services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(), "order_exchange", "order_response", "order.created",  ExchangeType.Topic));

builder.Services.AddHostedService<OrderCreatedListener>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Ecomm service",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecomm API");
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
