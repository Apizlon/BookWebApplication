using Microsoft.Extensions.DependencyInjection;
using MyBookApp.Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace MyBookApp.Application.Kafka;

public class KafkaConsumerService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private Task _consumerTask;
    private CancellationTokenSource _cancellationTokenSource;

    public KafkaConsumerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Инициализируем токен для отмены
        _cancellationTokenSource = new CancellationTokenSource();

        // Запускаем задачу по обработке Kafka сообщений в фоновом режиме
        _consumerTask = Task.Run(async () => await StartKafkaConsumerAsync(), _cancellationTokenSource.Token);

        return Task.CompletedTask;
    }

    private async Task StartKafkaConsumerAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var bookService = scope.ServiceProvider.GetRequiredService<IBookService>();
            
            try
            {
                // Запуск потребления Kafka
                await bookService.StartConsumingAsync();
            }
            catch (Exception ex)
            {
                // Логирование ошибок
                Console.WriteLine($"Kafka Consumer Service encountered an error: {ex.Message}");
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        // Остановка консьюмера при завершении приложения
        _cancellationTokenSource.Cancel();
        if (_consumerTask != null)
        {
            await _consumerTask;
        }
    }
}