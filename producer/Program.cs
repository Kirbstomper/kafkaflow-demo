using KafkaFlow;
using KafkaFlow.Producers;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Producer;

var services = new ServiceCollection();

const string topicName = "sample-topic";
const string producerName = "say-hello";
const string unstructuredTopicName = "custom-topic";

services.AddKafka(kafka =>
    kafka
        .UseConsoleLog()
        .AddCluster(cluster =>
            cluster
                .WithBrokers(["localhost:9092"])
                .CreateTopicIfNotExists(topicName, 1, 1)
                .CreateTopicIfNotExists(unstructuredTopicName, 1, 1)
                .AddProducer(
                    producerName,
                    producer =>
                        producer
                            .DefaultTopic(topicName)
                            .AddMiddlewares(m => m.AddSerializer<JsonCoreSerializer>())
                )
        )
);

var serviceProvider = services.BuildServiceProvider();

var producer = serviceProvider.GetRequiredService<IProducerAccessor>().GetProducer(producerName);

await producer.ProduceAsync(
    topicName,
    Guid.NewGuid().ToString(),
    new HelloMessage { Text = "Hello!" }
);

Console.WriteLine("Message sent!");
