# Kafkaflow Demo Project
A project demonstrating some basics of the Kafkaflow (https://farfetch.github.io/kafkaflow/)

- producer : An application that produces a event to a topic
- consumer : An application that listens to two topics and prints information about events received
- web : An application that exposes a endpoint to produce events to a topic. It also listens to another topic and produces events.
- test : A test suite for web. Showing how you can test using mocks or testcontainers for a integration test.

