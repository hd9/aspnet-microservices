# ASP.NET Microservices
This is a sample application to demo Microservices in .NET
using [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet),
[Docker](https://www.docker.com/),
[Docker Compose](https://docs.docker.com/compose/), 
[MongoDB](https://www.mongodb.com/), [MySQL](https://www.mysql.com/),
[Redis](https://redis.io/), [Vue.js](https://vuejs.org/),
[Azure App Services](https://azure.microsoft.com/en-us/services/app-service/),
[Azure AKS](https://docs.microsoft.com/en-us/azure/aks/) and
[Kubernetes](https://kubernetes.io/).

To learn more about this app, Docker, Azure and microservices,
please check my blog at:
[blog.hildenco.com](https://blog.hildenco.com)

## Source Code
The source code is available at
[github.com/hd9/aspnet-microservices](github.com/hd9/aspnet-microservices).


## Disclaimer
When you create a microservice-based application, you need to deal with
complexity and make some choices. Ours were to reduce the complexity by
avoiding some parlalel design patterns and focusing on development of the
services themselves. That said, I chose to reduce the scope of work to make this
application more intuitive so that those starting with microservices and .NET
Core.

Please, **don't use this project in production**. Its objective is to serve
as a simple reference for those building microservices in .NET and potentially
a good fork candidate for implementing some of the _areas of improvement_
listed next.


## Areas for improvement
Because this project is supposed to be a lightweight introduction to microservices
adding all the recommended patterns would increase a lot its complexity. Here
are some of the areas that could be improved:
* **Testing**: as any time-constraint project, no testing was written for the
  services included in this project. However, if you'd like to implement yours,
  feel free to add your testing framework of choice. For a good introduction on 
  how to test ASP.CORE, [check this page](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/test-aspnet-core-services-web-apps).
* **Security**: most of the settings here are default, or even no credentials.
  That was done un purpose so it's simpler for people to understand the
  different parts of the application, interact with the services (Redis, MongoDB,
  MySQL and  RabbitMQ for example). Minimum cryptography and random Ids (like GUIDs)
  were used to keep the system as simple as possible. For more informantion
  on security best practices on ASP.NET microservices, 
  [check this link](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/secure-net-microservices-web-applications/).
* **Performance**: performance isn't also implemented by default -
  the databases contain no indexes and no microservice (apart from `Web`) contain 
  caching.
* **Async-Communication**: the project used MassTransit to provide async
  request/response essentially between the backend services. While I do 
  use async request/replies between `Order`, `Shipping` and `Payment`,
  I didn't utilize the pattern in   `Web` where I essentially proxied external
  services and issued restful calls to those services. For most queries, this is
  an acceptable pattern but some `commands` were also implemented like that.
  Wen designing microservices, asynchronous communication should be preferred
  as it reduces coupling and increases the resilience of the communicating service
  especially for background events such as subscribing to newsletters or creating
  orders.
* **Infrastructure**: for the purposes of this exercise, we utilized `RabbitMQ`
  as a lightweight message-broker running on a `Docker` continaer. However,
  in production, it would represent a risk as it would become a single point
  of failure. For high availability and high scalability, you should consider
  using [Azure Service Bus](https://azure.microsoft.com/en-us/services/service-bus/). 
* **Versioning**: when developing complex applications, its inevitable that
  we'll have to make changes to our apis. This project doesn't take that into
  account as it would grow it in size and complexity. For more information on 
  that topic, consider reading 
  [this document](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design#versioning-a-restful-web-api).
* **Single Repository**: at first sight, having all microservices in the same .NET
  solution may sound strange. And indeed it is because on large organizations,
  different microservices are developed by different teams using potentially
  different tools (programming languages, databases, etc) and hosted on different
  repositories. However, the purpose of this project is to expose the
  essential bits of the microservice architecture which, as previously stated,
  if we adopted the previous practices would significantily grow its complexity.
* **Resiliency**: the project also does not apply resiliency patterns such as
  exponential backoffs or circuit breakers. Feel free to fork and write your own.
  For more information, [check this document](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/resilient-high-availability-microservices).
  A nice way to learn that would be using dedicated libraries to the purpose 
  such as [Polly.net](https://github.com/App-vNext/Polly) or use the built-in
  functionality provided by frameworks
  such as [NServiceBus](https://github.com/App-vNext/Polly) or
  [MassTransit](https://masstransit-project.com/).
* **Monolithic UI**: the `Web` service is also implemented as a monolithic UI.
  While today, the preferred approach would be using [Micro Frontends](https://martinfowler.com/articles/micro-frontends.html),
  it would almost exponentially increase the scope of work and the complexity of 
  the project. However, this project serves as a solid foundation for those
  wanting to learn and implement that pattern.
* **Diagnostics**: there's also room for improvemente on the diagnostics
  side either with health checks either with out of the box functionality
  [provided by ASP.NET Core](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/monitor-app-health#implement-health-checks-in-aspnet-core-services)
  or with external libraries such as [Beat Pulse](https://github.com/Xabaril/BeatPulse).
* **Logs and event streams**: A microservice-based application should not try
  to store the output stream of events or logfiles by itself, nor try to manage
  the routing of the events to a central place. Instead it should be transparent,
  meaning that each process should just write its event stream to a standard output
  so that it could be collected by the execution environment infrastructure
  where it's running.

## Services included in this project
So far, the project consists of the following services:
* **Web**: the frontend for the web store
* **Catalog**: provides catalog information for the web store
* **Newsletter**: accepts user emails and stores them in the newsletter database 
* **Order**: provides order features for the web store
* **Account**: provides account services (login, creation, etc) for the web store
* **Recommendation**: provides recommendations between products
* **Notification**: sends email notifications on certain events in the system
* **Payment**: simulates a _fake_ payment store
* **Shipping**: simulates a _fake_ shipping store


# Info for developers
On this section we'll list the essentials on how to modify
and run this project on your machine.
If you're not interested in details about development feel
free to jump to the next section.


## Technical Dependencies
To run this project on your machine, please make sure you have installed:
* Docker Desktop (Mac, Windows) or Docker Engine (Linux)
* [.NET SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
* A git client
* A Linux terminal or WSL

If you want to develop/extend/modify it, then I'd suggest you to also have:
* a valid [GitHub](https://github.com) account
* [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
* (or) [Visual Studio Code](https://code.visualstudio.com/)

### Images used
These are the Docker images that we'll use:
* ASP.NET Core SDK
* ASP.NET Core runtime
* Mongo:latest
* MySQL:latest
* Redis:6-alpine
* RabbitMQ:latest
* Adminer
* rediscommander/redis-commander:latest

### Initializing the Project
To initialize the project run:   
```s
git clone https://github.com/hd9/aspnet-microservices`
```

### Building with Visual Studio
Building with Visual Studio 2019 should be straight forward.
Simply open the `AspNetMicroservices.sln` file from the `src` folder
and Visual Studio should be able to build the project.

### Building the Docker images with Docker Compose
To build all the images, the quickest way is to use
[Docker Compose](https://docs.docker.com/compose/). Assuming you have
[.NET SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
installed, you should be able to build the project with:   
`docker-compose build`

### Building the Docker images independently
But if you really want, you can also build the images independently.
Inside each project (and apart from [Microservices.Core](https://github.com/hd9/aspnet-microservices/tree/master/src/Microservices.Core))
you should see a `build` script that should be executed on a Linux terminal
or on [WSL](https://docs.microsoft.com/en-us/windows/wsl/install-win10).

You could manually run each `build` script from each folder or simpler,
just run `build-all` located in the `src` folder. Please note that it'll
be necessary to run `chmod +x build-all` before you run it.

### Configuration
The easiest way to understand the services and their configurations
is by opening the `src/docker-compose.yml` file.

### Making changes to the project
If you want to make changes to the project, the simplest way by opening
one of the solutions on this project:
* `AspNetMicroservices.sln`: the main solution, consisting on most of the
  projects and services.
* `Microservices.Core.sln`: source for the core NuGet package. This package is
  necessary so our containers be isolated from each other. The package is
  published on GitHub at [github.com/hd9?tab=packages](https://github.com/hd9?tab=packages)

# Running the services
Let's quickly review how to run the services.

## Running with Docker compose
The most straight forward way to run the application is
by using [Docker Compose](https://docs.docker.com/compose/).
Docker Compose is a tool that can be used to define and run
multiple containers as a single service using the command below:   
`docker-compose up`

## Manually running the services
This is a more detailed guide on how to run the services one by one.
Feel free to jump to the next section if running with compose
is sufficient for you.

Start by pulling the required images with:   
```s
docker pull mongo:latest
docker pull rabbitmq:latest
docker pull mysql:latest
docker pull adminer:latest
docker pull redis:6-alpine
docker pull rediscommander/redis-commander:latest
```
Let's now review how to build each of the services.

***Note***: For simplicity, I'm not tagging the images so all images
will be tagged as `latest` by default by Docker. Feel free to
modify the name, ports and version.

## Setting up the Web service
The Web service is the frontend for our application. It requires a Redis
instance to provide distributed caching on the server and a distributed and
faster shopping cart experience.
Redis is an open source in-memory data store, which is often used as a
distributed cache. You can use Redis locally, and you can configure an Azure
Redis Cache for an Azure-hosted ASP.NET Core app.

An app configures the cache implementation using a RedisCache instance
(`AddStackExchangeRedisCache`) in a non-Development environment in the
`Startup.ConfigureServices` method.

To build the Web container, run:   
```s
docker build -t web .
```

To run the container, run:   
```s
docker run --name web -p 8000:80 web
```

To remove the container and its images from the system, do:   
```s
docker container rm -f web
docker image rm web -f
```


### Running Web's dependencies
Web utilizes `Redis` so it can effectively cache its data and 
[Redis Commander](http://joeferner.github.io/redis-commander/) optionally,
if you want to play with your `Redis` data.
Let's see how to run them.

To run the redis service do:   
`docker run --name redis -d redis:6-alpine`

Alternatively, if you want to manage your Redis container
from outside of the container network so you can use it with
your development tools, run the following command:   
`docker run -d --name redis -p 6379:6379 redis:6-alpine`

Then, install the Redis tools. For example, on Ubuntu:   
`sudo apt install redis-tools`

To connect to your local Redis instance (on port 6379), run:
`redis-cli`

#### Redis Commander 
If you want, you can optionally start a
[Redis Commander](http://joeferner.github.io/redis-commander/)
[Docker Instance](https://hub.docker.com/r/rediscommander/redis-commander)
and use as a WYSIWYG admin interface for Redis with the information below.

First get the redis IP with:   
`web_redis_ip=$(docker inspect redis -f '{{json .NetworkSettings.IPAddress}}')`

Then run the below command:   
`docker run --rm --name redis-commander -d --env REDIS_HOSTS=$web_redis_ip -p 8082:8081 rediscommander/redis-commander:latest`

### Setting up RabbitMQ
This project uses RabbitMQ to provide an asyncrhonous pub/sub interface
where the services can communicate.

To run RabbitMQ, do:   
`docker run -d -h hildenco --name rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:management-alpine`

On the command above we essentially exposed 2 ports
from the containers to our localhost:
  * 15672: Rabbitmq's management interface. Can be accessed at: http://localhost:15672/.
  * 5672: this is what our services will use to intercommunicate

We'll use MassTransit to abstract RabbitMQ so we can implement patterns like
pub/sub with minimum effort. Please note that we're running our RabbitMQ
instance using the default password (guest|guest) and MassTransit's already
pre-configured with that. 

If for some reason you decide to change your RabbitMQ password, you'll have to 
do two things:
1. update the above docker command setting the username and password with
   `-e RABBITMQ_DEFAULT_USER=<your-username> -e RABBITMQ_DEFAULT_PASS=<your-password>`
2. Update your `Startup.cs` file(s) with the snippet below.

```csharp
	c.Host(cfg.MassTransit.Host, h =>
	{
		h.Username("<your-username>");
		h.Password("<your-password>");
	});
```

For more information about RabbitMQ, visit:
* [RabbitMQ's website](https://rabbitmq.com/)
* [RabbitMQ on Docker Hub](https://hub.docker.com/_/rabbitmq)


## CatalogSvc
The Catalog service holds catalog and product information.

To build the container, run:   
```s
docker build -t catalog .
```

To run the container, run:   
```s
docker run --name catalog -p 8001:80 catalog
```

To remove the container and its images from the system, do:   
```s
docker container rm -f catalog
docker image rm catalog -f
```


### Running Catalog database
Our `CatalogSvc` utilizes MongoDB as it data store. To run
it with Docker do:   
`docker run -d --name catalog-db -p 3301:27017 mongo`

#### Seeding Product data
To seed some initial data, connect to the catalog / mongodb
instance with:   
`mongo mongodb://localhost:32769`

And run the commands:   
```
mongoimport --db catalog --collection products products.json --port 3301 --jsonArray
mongoimport --db catalog --collection categories categories.json --port 3301 --jsonArray
```



## AccountSvc
The Account service provides account services.

To build the Account container, run:   
```s
docker build -t account .
```

To run the account container, run:   
```s
docker run --name account -p 8004:80 account
```

To remove the container and its images from the system, do:   
```s
docker container rm -f account
docker image rm account -f
```


### Running the database
AccountSvc uses `MySql` as its data store.
To run the account db, do:   
`docker run -d --name account-db -p 3304:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

To import the database, run:   
`mysql --protocol=tcp -u root -ptodo -P 3304 < AccountSvc/db.sql`



## OrderSvc
The Order service manages orders in the application.

To manually build the order container, run:   
`docker build -t order .`

To run the container, do:
`docker run --name order -p 8003:80 order`

To remove the container and its images from the system, do:   
```s
docker container rm -f order
docker image rm order -f
```


### The Order database
OrderSvc also uses `MySQL` as its data store.
To run the order database, do:   
`docker run -d --name order-db -p 3303:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

Connect to the database with:   
`mysql --protocol=tcp -u root -ptodo -P 3303 < OrderSvc/db.sql`


## PaymentSvc
The Payment service provides _fake_ payment data
so we can test the whole workflow.

To build the payment container, run:   
`docker build -t payment .`

To run the payment container, run:   
`docker run --name payment -p 8007:80 payment`

To remove the container and its images from the system, do:   
```s
docker container rm -f payment
docker image rm payment -f
```


### The Payment Database
PaymentSvc also uses `MySQL` as its data store. To run
it, do:   
`docker run -d --name payment-db -p 3307:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

To import the database, run:   
`mysql --protocol=tcp -u root -ptodo -P 3307 < PaymentSvc/db.sql`



## RecommendationSvc
The Recommendation service provides (naive) recommendations
for the application.

To build the recommendation container, run:   
`docker build -t recommendation .`

To run the container, run:   
`docker run --name recommendation -p 8005:80 recommendation`

To remove the container and its images from the system, do:   
```s
docker container rm -f recommendation
docker image rm recommendation -f
```


### The Recommendation database
RecommendationSvc also uses `MySQL` as its data store.
To run the recommendation db, do:   
`docker run -d --name recommendation-db -p 3305:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

To import the database, run:   
`mysql --protocol=tcp -u root -ptodo -P 3305 < RecommendationSvc/db.sql`


## NotificationSvc
The Notification service provides simple notification
via SMTP (you can use your Gmail, for example) for events
that trigger that functionality.

To build the notification container, run:   
`docker build -t notification .`

To run the container, run:   
`docker run --name notification -p 8006:80 notification`

To remove the container and its images from the system, do:   
```s
docker container rm -f notification
docker image rm notification -f
```


### The Notification database
NotificationSvc also uses `MySQL` as its data store.
To run the it, do:   
`docker run -d --name notification-db -p 3306:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

To import the database, run:   
`mysql --protocol=tcp -u root -ptodo -P 3306 < NotificationSvc/db.sql`



## NewsletterSvc
The Newsletter service provides simple newsletter functionality.

### Building the Newsletter image
To build the newsletter container, run:   
`docker build -t newsletter .`

To run the container, run:   
`docker run --name newsletter -p 8002:80 newsletter`

To remove the container and its images from the system, do:   
```s
docker container rm -f newsletter
docker image rm newsletter -f
```


### The Newsletter database
NewsletterSvc also uses `MySQL` as its data store. To
run the Newsletter db, do:
`docker run -d --name newsletter-db -p 3302:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

To import the database, run:   
`mysql --protocol=tcp -u root -ptodo -P 3304 < NewsletterSvc/db.sql`


## ShippingSvc
The Shipping service provides _fake_ shipping information
so the application can complete some simple workflows.

To build the Shipping container, run:   
`docker build -t shipping .`

To run the container, run:   
`docker run --name shipping -p 8007:80 shipping`

To remove the container and its images from the system, do:   
```s
docker container rm -f shipping
docker image rm shipping -f
```


### The Shipping database
ShippingSvc also uses `MySQL` as its data store. To run
the Shipping db, do:
`docker run -d --name shipping-db -p 3308:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

To import the database, run:   
`mysql --protocol=tcp -u root -ptodo -P 3308 < ShippingSvc/db.sql`


## Management Interfaces
The project also includes management interfaces for RabbitMQ and MySQL
databases. If running the default settings, you should have available:
* Adminer: manage your MySQL databases
* RabbitMQ Management Console: manage your rabbitmq broker


### Adminer
Adminer (formerly phpMinAdmin) is a full-featured database management tool
written in PHP. Conversely to phpMyAdmin, it consist of a single file ready to
deploy to the target server. Adminer is available for MySQL, PostgreSQL, SQLite,
MS SQL, Oracle, Firebird, SimpleDB, Elasticsearch and MongoDB.

If you want to manage your MySQL databases with adminer, run it with:   
`docker run -d -p 8012:8080 --name adminer adminer`

To open Adminer, please open [http://localhost:8012/](http://localhost:8012/)
on your browser, enter the IP of your MySQL Docker instance (see below) as host
and login with its password (default: root|todo)

To get the IPs of the containers inside the Docker network. That can be queried
with:   
`docker inspect network bridge -f '{{json .Containers}}' | jq`

**Note**: you'll need [jq](https://stedolan.github.io/jq/) to format the output.


## RabbitMQ Management Console
RabbitMQ is an open source multi-protocol messaging broker. It's used in this
project via MassTransit to provide asynchronous communications via pub/sub and
async request/responses. RabbitMQ Management Console is available at:
[http://localhost:8010/](http://localhost:8010/). Login with guest/guest.


# Configuration
For a complete information on the services running,
check the `docker-compose.yml` file in the `src` folder.
For more information about Docker Compose, please 
[check this link](https://docs.docker.com/compose/).

## Changing Configuration
To change configuration, check `appsettings.json` and `appsettings.Development.json`
in each project's folder. Essentially you want to modify:
* appsettings.json: for your container configuration
* appsettings.Development.json: for your Visual Studio debugging configuration

## Docker Compose
For more information about the compose spec, please
check the `docker-compose.yml` file in the `src` folder.

## Kubernetes
todo


# Cheatsheet
Okay, I admit, this got complicated enough. So here are some
commands to free up some of your memory.

## Urls
By default, the apps are configured at:
* Web: http://localhost:8000
* Catalog: http://localhost:8001
* Newsletter: http://localhost:8002
* Order: http://localhost:8003
* Account: http://localhost:8004
* Recommendation: http://localhost:8005
* Notification: http://localhost:8006
* Payment: http://localhost:8007
* Shipping: http://localhost:8008

And the management tools are available on:
* RabbitMQ dashboard: http://localhost:8010/
* Redis Commander: http://localhost:8011/
* MySQL Admin: http://localhost:8012/

## Databases
* catalog-db: mongodb://localhost:3301
* newsletter-db: mysql://localhost:3302
* order-db: mysql://localhost:3303
* account-db: mysql://localhost:3304
* recommendation-db: mysql://localhost:3305
* notification-db: mysql://localhost:3306
* payment-db: mysql://localhost:3307
* shipping-db: mysql://localhost:3308

## Commands
```s
# running it all with docker-compose
# on the src folder, run:
docker-compose up                           # start all the services
docker-compose down                         # stop all the services
docker-compose up <service-name>            # start <service-name> and its dependencies. Ex: docker-compose up shipping
docker-compose build                        # build all the services

# running the instances individually
docker run --name web -p 8000:80 web
docker run --name catalog -p 8001:80 catalog
docker run --name account -p 8004:80 account
docker run --name order -p 8003:80 order
docker run --name payment -p 8007:80 payment
docker run --name recommendation -p 8005:80 recommendation
docker run --name notification -p 8006:80 notification
docker run --name newsletter -p 8002:80 newsletter
docker run --name shipping -p 8007:80 shipping

```


# Further Reading
* [Microservices architecture](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/microservices-architecture)
* [Create a web API with ASP.NET Core and MongoDB](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-3.1&tabs=visual-studio)
* [Google Accounts - Sign in using App Passwords](https://support.google.com/accounts/answer/185833)
* [Docker images for ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-3.1)
* [Development workflow for Docker apps](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/docker-application-development-process/docker-app-development-workflow)
* [Dockerize an ASP.NET Core application](https://docs.docker.com/engine/examples/dotnetcore/)

## License
This project is licensed under [the MIT License](https://opensource.org/licenses/MIT).