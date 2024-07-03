using Domain;

public interface IDomainService
{
    public Task ProduceGreetingMessageAsync(string input);
}
