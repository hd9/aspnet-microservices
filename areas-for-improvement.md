# Areas for improvement
This project is supposed to be a lightweight introduction to microservices and
adding all the recommended patterns would increase a lot its complexity. This
document lists some of the areas that could be improved


## Testing
As any time-constraint project, no tests were written for the services. However,
if you'd like to implement yours, feel free to write your tests using your
testing framework of choice.  [For a good introduction on how to test .NET Core
and ASP.NET Core, read this](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/test-aspnet-core-services-web-apps).

## Security
Most of the settings here are default, or even no credentials.  That was done
un purpose so it's simpler for people to understand the different parts of the
application, interact with the services (Redis, MongoDB, MySQL and  RabbitMQ
for example). Minimum cryptography and random Ids (like GUIDs) were used to
keep the system as simple as possible. For more informantion on security best
practices on ASP.NET microservices, [check this
link](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/secure-net-microservices-web-applications/).

## Performance
Performance isn't also implemented by default - the databases contain no indexes
and no microservice (apart from `Web`) contains caching. However, I implemented
a simple caching strategy on `Web` using `Redis` so everyone understands the
essentials of [distributed caching in ASP.NET Core
3.1](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed).

## Async-Communication
The project used `MassTransit` to provide async request/response essentially
between the backend services. While I do use async request/replies between
`Order`, `Shipping` and `Payment`, I didn't utilize the pattern in   `Web` where
I essentially proxied external services and issued restful calls to those
services. For most queries, this is an acceptable pattern but some `commands`
were also implemented like that.  Wen designing microservices, asynchronous
communication should be preferred as it reduces coupling and increases the
resilience of the communicating service especially for background events such as
subscribing to newsletters or creating orders.

## Infrastructure
For this exercise, we utilized `RabbitMQ` as a lightweight message-broker
running on a `Docker` container. However, note that in production it would
represent a risk as it would become a single point of failure. For high
availability and high scalability, you should consider using cloud-based
solutions such as [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/)
or equivalent. 

## Versioning
when developing complex applications, its inevitable that we'll have to make
changes to our apis. This project takes opinionated decisions which may not be
the best for every project.  For more information on the topic, consider reading 
[this document](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design#versioning-a-restful-web-api).

## Single Git Repository
At first sight, having all microservices in the same .NET solution may sound
strange. And indeed it is because on large organizations, different
microservices are developed by different teams using potentially different tools
(programming languages, databases, etc) and are usually hosted on different
repositories. However, the purpose of this project is to expose the essential
bits of the microservice architecture without significantily
increased complexity.

## Resiliency
The project also does not apply resiliency patterns such as [exponential
backoffs](https://en.wikipedia.org/wiki/Exponential_backoff) or [circuit
breakers](https://microservices.io/patterns/reliability/circuit-breaker.html).
But feel free to fork and write your own.  For more information, [check this
document](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/resilient-high-availability-microservices).
A nice way to learn that would be using dedicated libraries to the purpose such
as [Polly.net](https://github.com/App-vNext/Polly) or use the built-in
functionality provided by frameworks such as
[NServiceBus](https://github.com/App-vNext/Polly) or
[MassTransit](https://masstransit-project.com/).

## Monolithic UI
The `Web` service is also implemented as a [monolithic
frontend](https://xebia.com/blog/the-monolithic-frontend-in-the-microservices-architecture/).
While today, the preferred approach would be using [Micro
Frontends](https://martinfowler.com/articles/micro-frontends.html), it would
almost exponentially increase the scope of work and the complexity of the
project. However, this project serves as a solid foundation for those
wanting to learn and implement that pattern.

## Diagnostics
There's also room for improvemente on the diagnostics side either with health
checks either with out of the box functionality [provided by ASP.NET
Core](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health#implement-health-checks-in-aspnet-core-services)
or with external libraries such as [Beat
Pulse](https://github.com/Xabaril/BeatPulse).

## Logs and event streams
A microservice-based application should not try to store the output stream of
events or logfiles by itself, nor try to manage the routing of the events to a
central place. Instead it should be transparent, meaning that each process
should just write its event stream to a standard output so that it could be
collected by the execution environment infrastructure where it's running.
