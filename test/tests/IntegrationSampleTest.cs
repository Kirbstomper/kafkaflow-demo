using Confluent.Kafka;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.Kafka;

/// <summary>
/// Sample class to run a Integration Test using TestContainers to run kafka
/// </summary>
public class IntegrationSampleTest : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public IntegrationSampleTest(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async void TestThisEvenWorks()
    {
        var client = _factory.CreateClient();
        var uuid = Guid.NewGuid().ToString();
        await client.PostAsync("/produce?name=" + uuid, null);

        //Create a consumer
        var config = new ConsumerConfig
        {
            BootstrapServers = _factory.GetConnectionString(),
            GroupId = "integration-sample-test-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            consumer.Subscribe("custom-topic");
            var consumeResult = consumer.Consume(10000);
            Xunit.Assert.NotNull(consumeResult.Message);
            Console.WriteLine(consumeResult.Message.Value);
            Xunit.Assert.Contains(uuid, consumeResult.Message.Value);
            consumer.Close();
        }
    }
}

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private KafkaContainer _kafkaContainer = new KafkaBuilder()
        .WithImage("confluentinc/cp-kafka:7.0.5")
        .Build();

    public string GetConnectionString()
    {
        return _kafkaContainer.GetBootstrapAddress();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _kafkaContainer.StartAsync().GetAwaiter().GetResult();
        builder.UseSetting("ConnectionStrings:Kafka", _kafkaContainer.GetBootstrapAddress());
    }
}
