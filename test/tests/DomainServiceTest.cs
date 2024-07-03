using Domain;
using KafkaFlow;
using KafkaFlow.Producers;
using Moq;

/// <summary>
/// Example test class using a mock
/// </summary>
public class DomainServiceTest
{
    [Fact]
    public async Task TestProduceGreetingMessageAsync()
    {
        // Given
        string input = "chris";
        // When
        var producerFactory = new Mock<IProducerAccessor>();
        var producer = new Mock<IMessageProducer>();

        producerFactory.Setup(p => p.GetProducer("producer-name")).Returns(producer.Object);

        DomainService domainService = new DomainService(producerFactory.Object);

        await domainService.ProduceGreetingMessageAsync(input);
        // Then

        producer.Verify(
            p =>
                p.ProduceAsync(
                    "custom-topic",
                    It.IsAny<string>(),
                    new GreetingMessage("Greeting", "Hello " + input),
                    null,
                    null
                ),
            Times.Once,
            "Message was not produced properly"
        );
    }
}
