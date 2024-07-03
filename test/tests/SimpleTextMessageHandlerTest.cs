using Domain;
using Moq;

/// <summary>
/// Example test class using a mock
/// </summary>
public class SimpleTextMessageHandlerTest
{
    [Fact]
    public async Task TestProduceGreetingMessageAsync()
    {
        // Given
        var input = new SimpleTextMessage("SomeText");
        // When

        var domainService = new Mock<IDomainService>();
        SimpleTextMessageHandler simpleTextMessageHandler = new SimpleTextMessageHandler(
            domainService.Object
        );

        await simpleTextMessageHandler.Handle(null, input);
        // Then
        domainService.Verify(
            ds => ds.ProduceGreetingMessageAsync(input.Text),
            Times.Once,
            "Domain service was not called as expected"
        );
    }
}
