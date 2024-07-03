using KafkaFlow;

namespace Domain;

/// <summary>
/// Simple handler to push greetings to the domain service
/// </summary>
public class SimpleTextMessageHandler : IMessageHandler<SimpleTextMessage>
{
    private readonly IDomainService _domainService;

    public SimpleTextMessageHandler(IDomainService domainService)
    {
        _domainService = domainService;
    }

    public async Task Handle(IMessageContext context, SimpleTextMessage message)
    {
        Console.WriteLine("Handling: " + message.Text);
        await _domainService.ProduceGreetingMessageAsync(message.Text);
    }
}

public record SimpleTextMessage(string Text);
