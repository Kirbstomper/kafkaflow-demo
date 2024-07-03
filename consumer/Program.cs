using Consumer;
using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.Extensions.DependencyInjection;

const string topicName = "sample-topic";
const string unstructuredTopicName = "custom-topic";
var services = new ServiceCollection();

const string producerName = "say-hello";

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
                .AddConsumer(consumer =>
                    consumer
                        .Topic(unstructuredTopicName)
                        .WithGroupId("sample-group")
                        .WithBufferSize(100)
                        .WithWorkersCount(10)
                        .AddMiddlewares(middlewares =>
                            middlewares
                                .AddSingleTypeDeserializer<CustomMessage, JsonCoreDeserializer>()
                                .AddTypedHandlers(h => h.AddHandler<CustomMessageHandler>())
                        )
                )
                .AddConsumer(consumer =>
                    consumer
                        .Topic(topicName)
                        .WithGroupId("sample-group")
                        .WithBufferSize(100)
                        .WithWorkersCount(10)
                        .AddMiddlewares(middlewares =>
                            middlewares
                                .AddSingleTypeDeserializer<HelloMessage, JsonCoreDeserializer>()
                                .AddTypedHandlers(h => h.AddHandler<HelloMessageHandler>())
                        )
                )
        )
);

var serviceProvider = services.BuildServiceProvider();

var bus = serviceProvider.CreateKafkaBus();

await bus.StartAsync();

Console.ReadKey();

await bus.StopAsync();
