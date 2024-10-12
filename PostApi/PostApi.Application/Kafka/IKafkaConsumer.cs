namespace PostApi.Application.Kafka;

public interface IKafkaConsumer
{
    Task ConsumeMessagesAsync(string topic, Action<string> messageHandler, CancellationToken cancellationToken);
    void StopConsuming();
}