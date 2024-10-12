using Confluent.Kafka;

namespace PostApi.Application.Kafka;

public class KafkaConsumer : IKafkaConsumer
{
    private readonly IConsumer<Ignore, string> _consumer;
    private bool _isConsuming;

    public KafkaConsumer(string bootstrapServers)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "book-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _isConsuming = false;
    }

    public async Task ConsumeMessagesAsync(string topic, Action<string> messageHandler, CancellationToken cancellationToken)
    {
        _consumer.Subscribe(topic);
        _isConsuming = true;

        try
        {
            while (_isConsuming && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _consumer.Consume(cancellationToken);
                    if (cr != null)
                    {
                        messageHandler(cr.Message.Value);
                    }
                }
                catch (ConsumeException e)
                {
                    // Логируем ошибки потребления
                    Console.WriteLine($"Error occurred: {e.Error.Reason}");
                }
            }
        }
        finally
        {
            StopConsuming();
        }
    }

    public void StopConsuming()
    {
        _isConsuming = false;
        _consumer.Close();
    }
}