using MyBookApp.Api.Middlewares;
using MyBookApp.Application.Extensions;
using MyBookApp.Application.Interfaces;
using MyBookApp.Application.Kafka;
using MyBookApp.DataAccess.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.MigrateDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddTransient<CustomExceptionHandlingMiddleware>();
builder.Services.AddHostedService<KafkaConsumerService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<CustomExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();