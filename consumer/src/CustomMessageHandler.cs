using KafkaFlow;
namespace Consumer;

public class CustomMessageHandler : IMessageHandler<CustomMessage>
{
    public Task Handle(IMessageContext context, CustomMessage message)
    {
        Console.WriteLine(
            " Handling Custom Message! Partition: {0} | Offset: {1} | Message: {2}",
            context.ConsumerContext.Partition,
            context.ConsumerContext.Offset,
            message.Title + ":" + message.Content
        );

        return Task.CompletedTask;
    }
}
