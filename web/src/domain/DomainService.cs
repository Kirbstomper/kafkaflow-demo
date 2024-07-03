using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Producers;

namespace Domain;

public class DomainService : IDomainService
{
    private readonly IMessageProducer _producer;
    const string custom_topic_name = "custom-topic";

    public DomainService(IProducerAccessor producerAccessor)
    {
        _producer = producerAccessor.GetProducer("producer-name");
    }

    /// <summary>
    /// Produces a message to a topic
    /// </summary>
    public async Task ProduceGreetingMessageAsync(string name)
    {
        await _producer.ProduceAsync(
            custom_topic_name,
            Guid.NewGuid().ToString(),
            new GreetingMessage("Greeting", "Hello " + name)
        );
    }
}

public record GreetingMessage(string Title, string Content);
