using Microsoft.Extensions.DependencyInjection;
using MyBookApp.Application.Interfaces;
using MyBookApp.Application.Kafka;
using MyBookApp.Application.Services;

namespace MyBookApp.Application.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IKafkaProducer, KafkaProducer>()
            .AddScoped<IBookService,BookService>();
    }
}