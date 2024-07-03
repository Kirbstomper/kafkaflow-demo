using Domain;
using KafkaFlow;
using KafkaFlow.Serializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddKafka(kafka =>
    kafka
        .UseConsoleLog()
        .AddCluster(cluster =>
            cluster
                .WithBrokers(new[] { builder.Configuration["ConnectionStrings:Kafka"] })
                .CreateTopicIfNotExists("sample-topic")
                .CreateTopicIfNotExists("custom-topic")
                .AddConsumer(consumer =>
                    consumer
                        .Topic("sample-topic")
                        .WithGroupId("domain-group")
                        .WithBufferSize(100)
                        .WithWorkersCount(10)
                        .AddMiddlewares(middlewares =>
                            middlewares
                                .AddSingleTypeDeserializer<
                                    SimpleTextMessage,
                                    JsonCoreDeserializer
                                >()
                                .AddTypedHandlers(handlers =>
                                    handlers.AddHandler<SimpleTextMessageHandler>()
                                )
                        )
                )
                .AddProducer(
                    "producer-name",
                    producer =>
                        producer
                            .DefaultTopic("sample-topic")
                            .WithAcks(Acks.All)
                            .AddMiddlewares(middlewares =>
                                middlewares.AddSerializer<JsonCoreSerializer>()
                            )
                )
        )
);
builder.Services.AddSingleton<IDomainService, DomainService>();

var app = builder.Build();

var domain_service = app.Services.GetService<IDomainService>();

app.MapPost("/produce", (string name) => domain_service.ProduceGreetingMessageAsync(name));

var serviceProvider = builder.Services.BuildServiceProvider();

var bus = serviceProvider.CreateKafkaBus();

await bus.StartAsync();

app.Run();

public partial class Program() { }
