# Kafkaflow Demo Project
A project demonstrating some basics of the Kafkaflow (https://farfetch.github.io/kafkaflow/)

- producer : An application that produces a event to a topic
- consumer : An application that listens to two topics and prints information about events received
- web : An application that exposes a endpoint to produce events to a topic. It also listens to another topic and produces events.
- test : A test suite for web. Showing how you can test using mocks or testcontainers for a integration test.

## Starting infra
A docker-compose file is provided to spinup kafka locally, you can run it using `docker-compose up -d`

## Running producer
`dotnet run --project producer`

## Running consumer
`dotnet run --project consumer`
You can then press any key to stop the consumer and exit

## Running web
`dotnet run --project web`
You can then send a POST to `/produce?name=NAMEHERE` replacing name here with whatever value you want to be included with the produced message.


## Running Tests
Run `dotnet test` to run tests


