namespace PostApi.Application.Kafka;

public interface IKafkaProducer
{
    Task SendMessageAsync(string topic, string message);
}