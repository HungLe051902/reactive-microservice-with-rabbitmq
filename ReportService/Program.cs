using Plain.RabbitMQ;
using RabbitMQ.Client;
using ReportService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConnectionProvider>(new ConnectionProvider(builder.Configuration.GetValue<string>("RabbitMQ:Url")));
builder.Services.AddSingleton<ISubscriber>(x => new Subscriber(x.GetService<IConnectionProvider>(), "report_exchange", "report_queue", "report.*", ExchangeType.Topic));
builder.Services.AddSingleton<IMemoryReportStorage, MemoryReportStorage>();
builder.Services.AddHostedService<ReportDataCollector>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Report API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
