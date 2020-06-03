# ASP.NET Microservices
This is a sample application to demo Microservices in .NET
using [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet),
[Docker](https://www.docker.com/),
[Docker Compose](https://docs.docker.com/compose/), 
[MongoDB])(https://www.mongodb.com/), [MySQL](https://www.mysql.com/),
[Redis](https://redis.io/), [Vue.js](https://vuejs.org/),
[Azure App Services](https://azure.microsoft.com/en-us/services/app-service/),
[Azure AKS](https://docs.microsoft.com/en-us/azure/aks/) and
[Kubernetes](https://kubernetes.io/).

To learn more about this app, Docker, Azure and microservices, please check our
blog at: [blog.hildenco.com](https://blog.hildenco.com)

## Source Code
The source code is available at
[github.com/hd9/aspnet-microservices](github.com/hd9/aspnet-microservices).

## License
This project is licensed under [The MIT License](https://opensource.org/licenses/MIT).

## Disclaimer
Please don't use this project in production. Most of the settings
used here are default so it's simpler for people to understand the
different parts of the system.

## Services
So far, the project consists of the following services:
    * Web: the frontend for the web store
    * Catalog: provides catalog information for the web store
    * Newsletter: accepts user emails and stores them in the newsletter database 
    * Order: provides order features for the web store
    * Account: provides account services (login, creation, etc) for the web store
    * Recommendation: provides recommendations between products
    * Notification: sends email notifications on certain events in the system
    * Payment: simulates a _fake_ payment store
    * Shipping: simulates a _fake_ shipping store


## Dependencies
To run this project on your machine, please make sure you have installed:
    * Docker Desktop (Mac, Windows) or Docker Engine (Linux)
    * [.NET SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
    * A git client

If you want to develop/extend/modify it, then I'd suggest you to also have:
    * a valid [GitHub](https://github.com) account
	* [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
	* (or) [Visual Studio Code](https://code.visualstudio.com/)

# Initializing the Project
To initialize the project run:   
`dotnet clone https://github.com/hd9/aspnet-microservices`


# For Developers
On this section we'll list the essentials on how to modify
and run this project on your machine.

If you're not interested in details about development feel
free to jump to the next section.

## Building the project
Assuming you have the [.NET SDK 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)
installed, you should be able to build the project with
Visual Studio 2019 or with the dotnet CLI.

## Building the Docker Images
Each project (apart from `Microservices.Core`) contains a `build` script
that should be executed on [WSL](https://docs.microsoft.com/en-us/windows/wsl/install-win10).

You could manually run each `build` script from each folder or simpler,
just run `build-all` located in the `src` folder. Please note that it'll
be necessary to run `chmod +x build-all` before you run it.

## Configurations
todo

## Solutions
There are two solutions on this project:
	* `AspNetMicroservices.sln`: the main solution, consisting on most of the
	  projects
	* `Microservices.Core.sln`: source for the core NuGet package. This package is
	  necessary so our containers be isolated from each other. The package is
	  published on GitHub at [github.com/hd9?tab=packages](https://github.com/hd9?tab=packages)


## Installing the required Docker images
To run this application, you'll need the following Docker images:
    * ASP.NET Core SDK
    * ASP.NET Core runtime
    * Mongo:latest
    * MySQL:latest
	* Redis:6-alpine
    * RabbitMQ:latest
	* Adminer
	* rediscommander/redis-commander:latest

Start by pulling the images with:   
```s
docker pull mongo:latest
docker pull rabbitmq:latest
docker pull mysql:latest
docker pull adminer:latest
docker pull redis:6-alpine
docker pull rediscommander/redis-commander:latest
```


# Setting up the microservices
Let's now review how to build each of the services.

## Web
The Web service is the frontend for our application. It requires the Redis
service to provide distributed caching and the shopping cart experience.

Redis is an open source in-memory data store, which is often used as a
distributed cache. You can use Redis locally, and you can configure an Azure
Redis Cache for an Azure-hosted ASP.NET Core app.

An app configures the cache implementation using a RedisCache instance
(AddStackExchangeRedisCache) in a non-Development environment in
Startup.ConfigureServices.

### Building the Web container
To build the Web container, run:
```s
docker build -t web .
```

### Running the container
To run the container, run:
```s
docker run --name web -p 21400:80 web
```

### Cleaning up
To remove the container and its images from the system, do:
```s
# To remove it, run:
docker image rm web -f
docker container rm -f web
```

Note: For simplicity, I'm not tagging the images so all images
will be tagged as `latest` by default by Docker. Feel free to
modify the name, ports and version.

### Running the dependencies
Web utilizes `Redis` so it can effectively cache its data and 
[Redis Commander](http://joeferner.github.io/redis-commander/) optionally,
if you want to play with your `Redis` data.
Let's see how to run them.

### Running our Redis Container
To run the redis service do:
`docker run --name web-redis -d redis:6-alpine`

Alternatively, if you want to manage your Redis container from outside of the container
network so you can use it with your development tools, run the following command:
`docker run -d --name web-redis -p 6379:6379 redis:6-alpine`

Then, install the Redis tools. For example, on Ubuntu:
`sudo apt install redis-tools`

To connect to your local Redis instance (on port 6379), run:
`redis-cli`

### Using Redis Commander 
If you want, you can optionally start a
[Redis Commander](http://joeferner.github.io/redis-commander/)
[Docker Instance](https://hub.docker.com/r/rediscommander/redis-commander)
and use as a WYSIWYG admin interface for Redis with:

First get the web-redis IP with:
`web_redis_ip=$(docker inspect web-redis -f '{{json .NetworkSettings.IPAddress}}')`

Then run the below command:
`docker run --rm --name redis-commander -d --env REDIS_HOSTS=$web_redis_ip -p 8082:8081 rediscommander/redis-commander:latest`


## RabbitMQ
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
	* [RabbitMQ @ Docker Hub](https://hub.docker.com/_/rabbitmq)



## CatalogSvc
The Catalog service holds catalog and product information.

### Building the container
To build the container, run:
```s
docker build -t catalog .
```

### Running the container
To run the container, run:
```s
docker run --name catalog -p 21401:80 catalog
```

### Cleaning up
To remove the container and its images from the system, do:
```s
# To remove it, run:
docker image rm catalog -f
docker container rm -f catalog
```

Note: For simplicity, I'm not tagging the images so all images
will be tagged as `latest` by default by Docker. Feel free to
modify the name, ports and version.


### Running Catalog database
Our `CatalogSvc` utilizes MongoDB as it data store. To run
it with Docker do:
`docker run -d --name mongo-catalog -p 32769:27017 mongo`

### Seeding Product data
Let's now seed some initial data.
Connect to the catalog / mongodb instance with:
`mongo mongodb://localhost:32769`

And run:
```js
use catalog;
db.products.remove({});
db.products.insertMany([
    { Slug: "g-xbx1-123", Name: "Xbox One", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-ps4-456", Name: "PS4", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-ns-789", Name: "Nintendo Switch", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-ps5-753;", Name: "PS5", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 4 } ,
    { Slug: "g-xbxx-951", Name: "Xbox X Series", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 4 } ,
    { Slug: "g-wiiu-789", Name: "wii U", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-xbxc-123", Name: "Xbox One Controller", Price: 59, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-ps4c-456", Name: "PS4 Controller", Price: 59, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-nsc-789", Name: "Nintendo Switch Controller", Price: 59, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-ps5c-753", Name: "PS5 Controller", Price: 69, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 4 } ,
    { Slug: "g-xbxc-951", Name: "Xbox X Series Controller", Price: 69, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 4 } ,
    { Slug: "g-wiiuc-789", Name: "wii U Controller", Price: 49, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,

    { Slug: "g-ps4-dsl3", Name: "Dark Souls 3", Price: 49, Currency: "CAD", Description: "Dark Souls, the series that spawned a hundred imitators thanks to its emphasis on difficult-but-fair gameplay, came to a close with Dark Souls 3, but what a way to go out. Featuring a variety of awe-inspiring locales (some grotesque, some majestic), the same finely-tuned combat fans had come to expect, and a new emphasis on speed and versatility inspired by FromSoftware's own Bloodborne, Dark Souls 3 is a wonderful encapsulation of the series as a whole; not as scattered as Dark Souls 2, not as rough around the edges as the original. If you've been curious about the Souls games, this is where you should start.", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-ps4-ff15", Name: "Final Fantasy 15", Price: 49, Currency: "CAD", Description: "Four adrenaline-driven teens embark on the journey of a lifetime, but this ain’t no Road Trip. The RPG tale of Prince Noctis and his merry band straddles fantasy and reality with almost balletic grace, throwing in titanic monsters and classic missions alongside conversations about the weather.", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-ps4-d00m", Name: "Doom (PS4)", Price: 49, Currency: "CAD", Description: "This modern reboot is worthy of the name Doom, and is basically the FPS equivalent of a muscular body: it's speedy, empowering, and hits incredibly hard.", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-xbx1-d00m", Name: "Doom (Xbox One)", Price: 49, Currency: "CAD", Description: "This modern reboot is worthy of the name Doom, and is basically the FPS equivalent of a muscular body: it's speedy, empowering, and hits incredibly hard.", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-ps4-rotr", Name: "Rise of the Tomb Raider", Price: 49, Currency: "CAD", Description: "The storyline, with Ms Croft venturing through Siberia in an attempt to complete her father's work in the lost city of Kitezh, doesn't scream originality but packs in some genuine shocks, while the platforming and zip-lining mechanics take Lara to heights she's never before reached – and not just figuratively.", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-ps4-fo4", Name: "Fallout 4 (PS4)", Price: 49, Currency: "CAD", Description: "The storyline, with Ms Croft venturing through Siberia in an attempt to complete her father's work in the lost city of Kitezh, doesn't scream originality but packs in some genuine shocks, while the platforming and zip-lining mechanics take Lara to heights she's never before reached – and not just figuratively.", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-xbx1-fo4", Name: "Fallout 4 (Xbox One)", Price: 49, Currency: "CAD", Description: "Hitting PS4 with the atomic force of a Fat Boy, Fallout 4’s excellent gunplay and crafting systems can trigger a nasty case of RPG-itis. Don't worry though, there's a Stimpak for that.", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "g-xbx1-rmk", Name: "Resident Evil 2 Remake (Xbox One)", Price: 49, Currency: "CAD", Description: "Capcom's remake of survival horror classic Resident Evil 2 has gone down a storm with fans and newcomers alike, and it's no surprise. With gorgeous new graphics but the same brain-scratching puzzles and terrifying zombies, Resident Evil 2 is definitely a remake done right.", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,

    { Slug: "mi-fdr-951", Name: "Fender Stratocaster", Price: 800, Currency: "CAD", Description: "TODO", CategoryId: "musical-instruments", CategoryName: "Musical Instruments", Rating: 5 } ,
    { Slug: "mi-gib-789", Name: "Gibson Les Paul", Price: 1500, Currency: "CAD", Description: "TODO", CategoryId: "musical-instruments", CategoryName: "Musical Instruments", Rating: 3 } ,
    { Slug: "mi-gljb-789", Name: "Geddy Lee's Jazz Bass", Price: 1500, Currency: "CAD", Description: "TODO", CategoryId: "musical-instruments", CategoryName: "Musical Instruments", Rating: 4 } ,
    
    { Slug: "mi-ddstr-463", Name: "D'Addario XT Electric Guitar Coated Strings", Price: 21.99, Currency: "CAD", Description: "TODO", CategoryId: "musical-instruments", CategoryName: "Musical Instruments", Rating: 4 } ,
    { Slug: "mi-ddstrl-455", Name: "D'Addario XT Acoustic Strings, Light, 12-53", Price: 21.99, Currency: "CAD", Description: "TODO", CategoryId: "musical-instruments", CategoryName: "Musical Instruments", Rating: 4 } ,
    { Slug: "mi-ddstrb-74", Name: "D'Addario XT Electric Bass Coated Nickel, Light Top/Medium Bottom Long Scale", Price: 44.34, Currency: "CAD", Description: "TODO", CategoryId: "musical-instruments", CategoryName: "Musical Instruments", Rating: 4 } ,
    
    { Slug: "s-pb-951", Name: "Paddle Boarding", Price: 500, Currency: "CAD", Description: "TODO", CategoryId: "sports", CategoryName: "Sports", Rating: 3 } ,
    { Slug: "s-jjk-789", Name: "Jiu-Jitsu Kimono", Price: 150, Currency: "CAD", Description: "TODO", CategoryId: "sports", CategoryName: "Sports", Rating: 3 } ,
    { Slug: "s-sb-789", Name: "Soccer Ball", Price: 15, Currency: "CAD", Description: "TODO", CategoryId: "sports", CategoryName: "Sports", Rating: 3 } ,
    
    { Slug: "b-peaa-789", Name: "Patterns of Enterprise Application Architecture", Price: 45, Currency: "CAD", Description: "The practice of enterprise application development has benefited from the emergence of many new enabling technologies. Multi-tiered object-oriented platforms, such as Java and .NET, have become commonplace. These new tools and technologies are capable of building powerful applications, but they are not easily implemented. Common failures in enterprise applications often occur because their developers do not understand the architectural lessons that experienced object developers have learned.", CategoryId: "books", CategoryName: "Books", Rating: 3 } ,
    { Slug: "b-goia-789", Name: "Go in Action", Price: 45, Currency: "CAD", Description: "Go in Action introduces the Go language, guiding you from inquisitive developer to Go guru. The book begins by introducing the unique features and concepts of Go. Then, you'll get hands-on experience writing real-world applications including websites and network servers, as well as techniques to manipulate and convert data at speeds that will make your friends jealous.", CategoryId: "books", CategoryName: "Books", Rating: 3 } ,
    { Slug: "b-alg-4445", Name: "Algorithms", Price: 45, Currency: "CAD", Description: "Introduction to Algorithms, the 'bible' of the field, is a comprehensive textbook covering the full spectrum of modern algorithms: from the fastest algorithms and data structures to polynomial-time algorithms for seemingly intractable problems, from classical algorithms in graph theory to special algorithms for string matching, computational geometry, and number theory.", CategoryId: "books", CategoryName: "Books", Rating: 4 } ,
    { Slug: "b-cd-789", Name: "Continuous Delivery", Price: 45, Currency: "CAD", Description: "Continuous Delivery is the logical next step after Continuous Integration for any modern software team. This book takes the admittedly ambitous goal of constantly delivering valuable software to customers, and makes it achievable through a set of clear, effective principles and practices.", CategoryId: "books", CategoryName: "Books", Rating: 5 } ,
    { Slug: "b-dpero-1515", Name: "Design Patterns: Elements of Reusable Object-Oriented Software", Price: 35, Currency: "CAD", Description: "Design Patterns: Elements of Reusable Object-Oriented Software is a software engineering book describing software design patterns. The book was written by Erich Gamma, Richard Helm, Ralph Johnson, and John Vlissides, with a foreword by Grady Booch.", CategoryId: "books", CategoryName: "Books", Rating: 5 } ,
    { Slug: "b-dedt-131", Name: "The Design of Everyday Things", Price: 35, Currency: "CAD", Description: "The Design of Everyday Things is a best-selling book by cognitive scientist and usability engineer Donald Norman about how design serves as the communication between object and user, and how to optimize that conduit of communication in order to make the experience of using the object pleasurable.", CategoryId: "books", CategoryName: "Books", Rating: 5 } ,
    { Slug: "b-caw-1773", Name: "Coders at Work", Price: 35, Currency: "CAD", Description: "This is a who's who in the programming world - a fascinating look at how some of the best in the world do their work. Patterned after the best selling Founders at Work, the book represents two years of interviews with some of the top programmers of our times.", CategoryId: "books", CategoryName: "Books", Rating: 5 } ,
    { Slug: "b-tmmm-189", Name: "The Mythical Man-Month", Price: 35, Currency: "CAD", Description: "Few books on software project management have been as influential and timeless as The Mythical Man-Month. With a blend of software engineering facts and thought-provoking opinions, Fred Brooks offers insight for anyone managing complex projects. These essays draw from his experience as project manager for the IBM System/360 computer family and then for OS/360, its massive software system. Now, 20 years after the initial publication of his book, Brooks has revisited his original ideas and added new thoughts and advice, both for readers already familiar with his work and for readers discovering it for the first time.", CategoryId: "books", CategoryName: "Books", Rating: 5 } ,
    
    { Slug: "c-rpi-789", Name: "Raspberry Pi", Price: 45, Currency: "CAD", Description: "The Raspberry Pi is a series of small single-board computers developed in the United Kingdom by the Raspberry Pi Foundation to promote teaching of basic computer science in schools and in developing countries. The original model became far more popular than anticipated, selling outside its target market for uses such as robotics.", CategoryId: "computers", CategoryName: "Computers", Rating: 3 } ,
    { Slug: "c-pppro-789", Name: "Pine Book Pro ARM", Price: 199, Currency: "CAD", Description: "A Powerful, Metal and Open Source ARM 64-Bit Laptop for Work, School or Fun. The Pinebook Pro is meant to deliver solid day-to-day Linux or *BSD experience and to be a compelling alternative to mid-ranged Chromebooks that people convert into Linux laptops.", CategoryId: "computers", CategoryName: "Computers", Rating: 3 } ,
    { Slug: "c-ptab-744", Name: "PineTab", Price: 99, Currency: "CAD", Description: "Attach the optional backlit magnetc keyboard to the PineTab for ultra-portable productivity. Use it on the go with the LTE modem adapter for untethered work or entertainment.", CategoryId: "computers", CategoryName: "Computers", Rating: 3 } ,
    
    { Slug: "tv-sbrv-55p", Name: "Sony Bravia 55", Price: 450, Currency: "CAD", Description: "TODO", CategoryId: "tvs", CategoryName: "TVs", Rating: 4 } ,
    { Slug: "tv-tnflx-55p", Name: "Toshiba Netflix 55", Price: 450, Currency: "CAD", Description: "TODO", CategoryId: "tvs", CategoryName: "TVs", Rating: 4 } ,
    
    { Slug: "hg-ggc-789", Name: "Grass Curter", Price: 40, Currency: "CAD", Description: "TODO", CategoryId: "home", CategoryName: "Home & Garden", Rating: 4 } ,
    { Slug: "hg-bbqc-789", Name: "BBQ Coal", Price: 40, Currency: "CAD", Description: "TODO", CategoryId: "home", CategoryName: "Home & Garden", Rating: 4 } ,
    
    { Slug: "aud-seab-789", Name: "Sony Earbuddy", Price: 40, Currency: "CAD", Description: "TODO", CategoryId: "headphones-audio", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "aud-bstd3-789", Name: "Beats Studio3 Wireless Headphones, Matte Black", Price: 400, Currency: "CAD", Description: "TODO", CategoryId: "headphones-audio", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "aud-smb-506041", Name: "Sennheiser MB Pro 1 (506041)", Price: 166, Currency: "CAD", Description: "TODO", CategoryId: "headphones-audio", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "aud-gcd-5041", Name: "Gaming Computer Desk with Storage for Controller, Headphone & Speaker - Black", Price: 159.71, Currency: "CAD", Description: "TODO", CategoryId: "headphones-audio", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "aud-ipn-753", Name: "iPhone 7", Price: 400, Currency: "CAD", Description: "TODO", CategoryId: "phones", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "aud-ipn-X11", Name: "iPhone X", Price: 999, Currency: "CAD", Description: "TODO", CategoryId: "phones", CategoryName: "Headphones & Audio", Rating: 4 },
    
    { Slug: "ph-sg-11", Name: "Samsung Galaxy 11", Price: 600, Currency: "CAD", Description: "TODO", CategoryId: "phones", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "ph-sg-20", Name: "Samsung Galaxy 20", Price: 800, Currency: "CAD", Description: "TODO", CategoryId: "phones", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "ph-iphone-X11-case1", Name: "iPhone X Case", Price: 9.99, Currency: "CAD", Description: "TODO", CategoryId: "phones", CategoryName: "Headphones & Audio", Rating: 2 },
]);

// insert categories
db.categories.insertMany([
    { Slug: "sports", Name: "Sports" } ,
    { Slug: "games", Name: "Games" } ,
    { Slug: "books", Name: "Books" } ,
    { Slug: "computers", Name: "Computers" } ,
    { Slug: "tvs", Name: "TVs" } ,
    { Slug: "home", Name: "Home" } ,
    { Slug: "musical-instruments", Name: "Musical Instruments" } ,
    { Slug: "headphones-audio", Name: "Headphones & Audio" } ,
    { Slug: "phones", Name: "Phones" }
]);
```


## AccountSvc
The Account service provides account services.

### Building the Account container
To build the Account container, run:
```s
docker build -t account .
```

### Running the container
To run the account container, run:
```s
docker run --name account -p 21404:80 account
```

### Cleaning up
To remove the container and its images from the system, do:
```s
docker image rm account -f
docker container rm -f account
```

Note: For simplicity, I'm not tagging the images so all images
will be tagged as `latest` by default by Docker. Feel free to
modify the name, ports and version.

### Running the database
AccountSvc uses `MySql` as its data store.
To run the account db, do:
`docker run -d --name mysql-accountsvc -p 3307:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

Connect to the database with:
`mysql --protocol=tcp -u root -p -P 3307`

And run the script:
```sql
create database accountdb;
use accountdb;

CREATE TABLE account (
    id                    INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name                  VARCHAR(1000)   NOT NULL,
    email                 VARCHAR(300)    NOT NULL,
    password              VARCHAR(1000)   NOT NULL,
    salt                  VARCHAR(100)    NOT NULL,
    created_at            DATETIME        NOT NULL,
    last_updated          DATETIME        NOT NULL,
    subscribe_newsletter  BIT
);

CREATE TABLE address (
    id                    INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    account_id            INT             NOT NULL,
    name                  VARCHAR(1000)   NOT NULL,
    is_default            BIT             NOT NULL,
    street                VARCHAR(1000)   NOT NULL,
    city                  VARCHAR(300)    NOT NULL,
    region                VARCHAR(100)    NOT NULL,
    postal_code           VARCHAR(10)     NOT NULL,
    country               VARCHAR(100)    NOT NULL,
    created_at            DATETIME        NOT NULL,
    last_updated          DATETIME        NOT NULL,
    FOREIGN KEY (account_id)
       REFERENCES account(id)
);

CREATE TABLE payment_info (
    id                    INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    account_id            INT             NOT NULL,
    name                  VARCHAR(1000)   NOT NULL,
    is_default            BIT             NOT NULL,
    number                VARCHAR(20)     NOT NULL,
    cvv                   INT             NOT NULL,
    exp_date              DATETIME        NOT NULL,
    method                TINYINT         NOT NULL,
    created_at            DATETIME        NOT NULL,
    last_updated          DATETIME        NOT NULL,
    FOREIGN KEY (account_id)
       REFERENCES account(id)
);

CREATE TABLE event_type (
    id                    TINYINT         NOT NULL PRIMARY KEY,
    name                  VARCHAR(1000)   NULL
);

CREATE TABLE account_history (
    id                    BIGINT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
    account_id            INT             NULL,
    event_type_id         TINYINT         NOT NULL,
    requested_by_id       VARCHAR(1000)   NULL COMMENT 'AccountId that requested the operation',
    ref_id                INT             NULL COMMENT 'Reference record ID. Ex: payment_info_id',
    ref_type_id           TINYINT         NULL COMMENT 'Type ID. Ex: Address=0, PaymentInfo=1, ShippingInfo=2 ',
    ip                    VARCHAR(100)    NULL,
    info                  VARCHAR(1000)   NULL,
    created_at            DATETIME        NOT NULL,
    FOREIGN KEY (event_type_id)
       REFERENCES event_type(id)
);

insert into event_type
values
(0, 'Login'),
(1, 'Account Created'),
(2, 'Account Updated'),
(3, 'Account Closed'),
(4, 'Password Created'),
(5, 'Password Updated'),
(6, 'Password Reset'),
(7, 'Forgot Password'),
(8, 'Address Created'),
(9, 'Address Updated'),
(10, 'Address Removed'),
(11, 'Address Set Default'),
(12, 'PaymentInfo Created'),
(13, 'PaymentInfo Updated'),
(14, 'PaymentInfo Removed'),
(15, 'PaymentInfo Set Default');
```


## OrderSvc
The Order service manages orders in the application.

### Building the container
To build the order container, run:
```s
docker build -t order .
```

### Running the container
To run the container, run:
```s
docker run --name order -p 21403:80 order
```

### Cleaning up
To remove the container and its images from the system, do:
```s
docker image rm order -f
docker container rm -f order
```

Note: For simplicity, I'm not tagging the images so all images
will be tagged as `latest` by default by Docker. Feel free to
modify the name, ports and version.

### The Order database
OrderSvc also uses `MySQL` as its data store.
To run the order database, do:
`docker run -d --name mysql-ordersvc -p 3308:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

Connect to the database with:
`mysql --protocol=tcp -u root -ptodo -P 3308`

And run the script to create the db and the tables:
```sql
create database orderdb;
use orderdb;

create table orders (
    id                  INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    number              VARCHAR(40)     NOT NULL,
    account_id          INT             NOT NULL,
    currency            VARCHAR(3)      NOT NULL,
    price               DECIMAL(10,2)   NOT NULL,
    tax                 DECIMAL(10,2)   NOT NULL,
    shipping_price      DECIMAL(10,2)   NOT NULL,
    total_price         DECIMAL(10,2)   NOT NULL,
    status              TINYINT         NOT NULL,
    pmt_status          TINYINT         NOT NULL,
    shipping_status     TINYINT         NOT NULL,    
    created_at          DATETIME        NOT NULL,
    last_modified       DATETIME        NOT NULL
);

create table lineitem (
    id                  INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    order_id            INT             NOT NULL,
    name                VARCHAR(1000)   NOT NULL,
    slug                VARCHAR(100)    NOT NULL,
    price               DECIMAL(10,2)   NOT NULL,
    qty                 INT             NOT NULL,
    FOREIGN KEY (order_id) 
        REFERENCES orders(id)
); 

CREATE TABLE payment_info (
    id                    INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    order_id              INT             NOT NULL,
    name                  VARCHAR(1000)   NOT NULL,
    number                VARCHAR(20)     NOT NULL,
    cvv                   SMALLINT        NOT NULL,
    exp_date              DATETIME        NOT NULL,
    method                TINYINT         NOT NULL,
    created_at            DATETIME        NOT NULL,
    last_updated          DATETIME        NOT NULL,
    FOREIGN KEY (order_id)
       REFERENCES orders(id)
);

CREATE TABLE shipping_info (
    id                    INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    order_id              INT             NOT NULL,
    payment_info_id       INT             NOT NULL,
    name                  VARCHAR(1000)   NOT NULL,
    street                VARCHAR(1000)   NOT NULL,
    city                  VARCHAR(300)    NOT NULL,
    region                VARCHAR(100)    NOT NULL,
    postal_code           VARCHAR(10)     NOT NULL,
    country               VARCHAR(100)    NOT NULL,
    created_at            DATETIME        NOT NULL,
    last_updated          DATETIME        NOT NULL,
    FOREIGN KEY (order_id)
       REFERENCES orders(id),
    FOREIGN KEY (payment_info_id)
       REFERENCES payment_info(id)
);

CREATE TABLE event_type (
    id                    TINYINT         NOT NULL PRIMARY KEY,
    name                  VARCHAR(1000)   NULL
);

insert into event_type
values
(1, 'Order Created'),
(2, 'Order Updated'),
(3, 'Order Cancelled'),
(4, 'Order Approved'),
(5, 'Order Shipped'),
(6, 'Order Closed'),
(10, 'Payment Submitted'),
(11, 'Payment Requested'),
(12, 'Payment Updated'),
(13, 'Payment Authorized'),
(14, 'Payment Declined'),
(20, 'ShippingInfo Submitted'),
(21, 'ShippingInfo Updated'),
(22, 'ShippingInfo Removed');

CREATE TABLE order_history (
    id                    BIGINT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
    order_id              INT             NULL,
    event_type_id         TINYINT         NOT NULL,
    requested_by_id       VARCHAR(1000)   NULL COMMENT 'AccountId that requested the operation',
    ref_id                INT             NULL COMMENT 'Reference record ID. Ex: PaymentInfo, ShippingInfo, etc',
    ref_type_id           TINYINT         NULL COMMENT 'Type ID. Ex: Address=0, PaymentInfo=1, ShippingInfo=2 ',
    ip                    VARCHAR(100)    NULL,
    info                  VARCHAR(1000)   NULL,
    created_at            DATETIME        NOT NULL,
    FOREIGN KEY (event_type_id)
       REFERENCES event_type(id)
);

```


## PaymentSvc
The Payment service provides _fake_ payment data
so we can test the whole workflow.

### Building the container
To build the payment container, run:
```s
docker build -t payment .
```

### Running the container
To run the payment container, run:
```s
docker run --name payment -p 21407:80 payment
```

### Cleaning up
To remove the container and its images from the system, do:
```s
docker image rm payment -f
docker container rm -f payment
```

Note: For simplicity, I'm not tagging the images so all images
will be tagged as `latest` by default by Docker. Feel free to
modify the name, ports and version.

### The Payment Database
PaymentSvc also uses `MySQL` as its data store. To run
it, do:
`docker run -d --name mysql-paymentsvc -p 3309:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

Connect to the database with:
`mysql --protocol=tcp -u root -ptodo -P 3309`

Create the db with:
`create database paymentdb;`

And run the script to create the tables:
```sql
use paymentdb;

create table payment (
    id                  INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    account_id          INT             NOT NULL,
    order_id            INT             NOT NULL,
    amount              DECIMAL(10,2)   NOT NULL,
    currency            VARCHAR(3)      NOT NULL,
    method              TINYINT         NOT NULL,
    status              TINYINT         NOT NULL,
    auth_code           VARCHAR(10)     NULL,
    created_at          DATETIME        NOT NULL,
    last_modified       DATETIME        NOT NULL
);

create table payment_request (
    pmt_gateway_id      VARCHAR(37)     NOT NULL PRIMARY KEY,
    pmt_id              INT             NOT NULL,
    amount              DECIMAL(10,2)   NOT NULL,
    currency            VARCHAR(3)      NOT NULL,
    name                VARCHAR(1000)   NOT NULL,
    number              VARCHAR(40)     NOT NULL,
    cvv                 INT             NOT NULL,
    exp_date            DATETIME        NOT NULL,
    method              TINYINT         NOT NULL,
    status              TINYINT         NOT NULL,
    auth_code           VARCHAR(10)     NULL,
    created_at          DATETIME        NOT NULL,
    FOREIGN KEY (pmt_id)
        REFERENCES payment(id)
);
```


## RecommendationSvc
The Recommendation service provides (naive) recommendations
for the application.

### Building the container
To build the recommendation container, run:
```s
docker build -t recommendation .
```

### Running the container
To run the container, run:
```s
docker run --name recommendation -p 21405:80 recommendation
```

### Cleaning up
To remove the container and its images from the system, do:
```s
docker image rm recommendation -f
docker container rm -f recommendation
```

Note: For simplicity, I'm not tagging the images so all images
will be tagged as `latest` by default by Docker. Feel free to
modify the name, ports and version.

### The Recommendation database
RecommendationSvc also uses `MySQL` as its data store.
To run the recommendation db, do:
`docker run -d --name mysql-recommendationsvc -p 3310:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

Connect to the database with:
`mysql --protocol=tcp -u root -ptodo -P 3310`

Create the db with:
`create database recommendationdb;`

And run the script to create the tables:
```sql
use recommendationdb;

create table product (
    slug                VARCHAR(100)    NOT NULL PRIMARY KEY,
    name                VARCHAR(100)    NOT NULL,
    description         VARCHAR(1000)   NOT NULL,
    price               DECIMAL(10,2)   NOT NULL,
    created_at          DATETIME        NOT NULL,
    last_update         DATETIME        NOT NULL
);

create table recommendation (
    product_slug        VARCHAR(100)    NOT NULL,
    related_slug        VARCHAR(100)    NOT NULL,
    hits                INT             NOT NULL DEFAULT 1,
    last_update         DATETIME        NOT NULL,
    PRIMARY KEY (product_slug, related_slug),
    FOREIGN KEY (product_slug) REFERENCES product(slug),
    FOREIGN KEY (related_slug) REFERENCES product(slug)
);

-- insert some products
insert into product values
(100, 'g-ps4-456', 'Playstation 4', 'PS4 is Sony\'s last gen console and one of the most sold consoles of all time', 0, sysdate(), sysdate()),
(101, 'g-ps4c-456', 'PS4 Controller', 'The PS4 Controller is the best companion for your PS4 console.', 0, sysdate(), sysdate()),
(102, 'g-ps4-fo4', 'Fallout 4 (PS4)', 'Fallout 4 (PS4) is probably the best Lara Croft ever made!', 0, sysdate(), sysdate()),
(103, 'g-ps4-dsl3', 'Dark Souls 3 (PS4)', 'Featuring a variety of awe-inspiring locales (some grotesque, some majestic), the same finely-tuned combat fans had come to expect, and a new emphasis on speed and versatility inspired by FromSoftware\'s own Bloodborne', 0, sysdate(), sysdate()),
(104, 'g-ps4-ff15', 'Final Fantasy 15 (PS4)', 'Four adrenaline-driven teens embark on the journey of a lifetime, but this ain’t no Road Trip.', 0, sysdate(), sysdate()),

-- xbox stuff
(200, 'g-xbx1-123', 'Xbox one', 'Xbox one is Microsoft\'s last gen console and full of 5-start exclusives.', 0, sysdate(), sysdate()),
(201, 'g-xbxc-123', 'Xbox One Controller', 'The Xbox One Controller is the best companion for your Xbox One console.', 0, sysdate(), sysdate());
(202, 'g-xbx1-d00m', 'Doom (Xbox One)', '', 0, sysdate(), sysdate()),
(203, 'g-xbx1-fo4', 'Fallout 4 (Xbox One)', '', 0, sysdate(), sysdate()),
(204, 'g-xbx1-rmk', 'Resident Evil 2 Remake (Xbox One)', 'Capcom\'s remake of survival horror classic Resident Evil 2 has gone down a storm with fans and newcomers alike, and it\'s no surprise.', 0, sysdate(), sysdate());

-- insert some recommendations to start
insert into recommendation (product_id, related_id, hits, last_update)
values
-- ps4 recomms
(100, 101, 10, sysdate()),
(100, 102, 10, sysdate()),
(100, 103, 10, sysdate()),
(100, 104, 100, sysdate()),
(101, 100, 100, sysdate()),
(102, 100, 100, sysdate()),
(103, 100, 100, sysdate()),
(104, 100, 100, sysdate()),

-- xbox recomms
(200, 201, 10, sysdate()),
(200, 202, 10, sysdate()),
(200, 203, 10, sysdate()),
(200, 204, 100, sysdate()),
(201, 200, 100, sysdate()),
(202, 200, 100, sysdate()),
(203, 200, 100, sysdate()),
(204, 200, 100, sysdate());
```


## NotificationSvc
The Notification service provides simple notification
via SMTP (you can use your Gmail, for example) for events
that trigger that functionality.

### Building the container
To build the notification container, run:
```s
docker build -t notification .
```

### Running the container
To run the container, run:
```s
docker run --name notification -p 21406:80 notification
```

### Cleaning up
To remove the container and its images from the system, do:
```s
docker image rm notification -f
docker container rm -f notification
```

Note: For simplicity, I'm not tagging the images so all images
will be tagged as `latest` by default by Docker. Feel free to
modify the name, ports and version.

### The Notification database
NotificationSvc also uses `MySQL` as its data store.
To run the it, do:
`docker run -d --name mysql-notificationsvc -p 3311:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

Connect to the database with:
`mysql --protocol=tcp -u root -ptodo -P 3311`

Create a database and table:
```sql
create database notificationdb;
use notificationdb;

CREATE TABLE notification (
    id          INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(1000)   NOT NULL,
    email       VARCHAR(300)    NOT NULL,
    created_at  DATETIME        NOT NULL,
    type        char(1)         NOT NULL
);
```


## NewsletterSvc
The Newsletter service provides simple newsletter functionality.

### Building the container
To build the newsletter container, run:
```s
docker build -t newsletter .
```

### Running the container
To run the container, run:
```s
docker run --name newsletter -p 21402:80 newsletter
```

### Cleaning up
To remove the container and its images from the system, do:
```s
docker image rm newsletter -f
docker container rm -f newsletter
```

Note: For simplicity, I'm not tagging the images so all images
will be tagged as `latest` by default by Docker. Feel free to
modify the name, ports and version.


### The newsletter database
NewsletterSvc also uses `MySQL` as its data store. To
run the Newsletter db, do:
`docker run -d --name mysql-newslettersvc -p 3312:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

Connect to the database with:
`mysql --protocol=tcp -u root -ptodo -P 3312`

Create a database and table:
```sql
create database newsletterdb;
use newsletterdb;

CREATE TABLE newsletter (
    id          INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(1000)   NOT NULL,
    email       VARCHAR(300)    NOT NULL,
    created_at  DATETIME        NOT NULL
);
```


## ShippingSvc
The Shipping service provides _fake_ shipping information
so the application can complete some simple workflows.

### Building the Shipping container
To build the Shipping container, run:
```s
docker build -t shipping .
```

### Running the container
To run the container, run:
```s
docker run --name shipping -p 21408:80 shipping
```

### Cleaning up
To remove the container and its images from the system, do:
```s
docker image rm shipping -f
docker container rm -f shipping
```

Note: For simplicity, I'm not tagging the images so all images
will be tagged as `latest` by default by Docker. Feel free to
modify the name, ports and version.

### The Shipping database
ShippingSvc also uses `MySQL` as its data store. To run
the Shipping db, do:
`docker run -d --name mysql-shippingsvc -p 3313:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

Connect to the database with:
`mysql --protocol=tcp -u root -ptodo -P 3313`

Create a database and table:
```sql
create database shippingdb;
use shippingdb;

CREATE TABLE shipping (
    id                  INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    number              VARCHAR(100)    NOT NULL,
    account_id          INT             NOT NULL,
    order_id            INT             NOT NULL,
    name                VARCHAR(1000)   NOT NULL COMMENT 'Name of the recipient',
    amount              DECIMAL(10,2)   NOT NULL,
    currency            VARCHAR(3)      NOT NULL,
    street              VARCHAR(1000)   NOT NULL,
    city                VARCHAR(300)    NOT NULL,
    region              VARCHAR(100)    NOT NULL,
    postal_code         VARCHAR(10)     NOT NULL,
    country             VARCHAR(100)    NOT NULL,
    status              TINYINT         NOT NULL,
    provider            TINYINT         NOT NULL,
    created_at          DATETIME        NOT NULL
);
```


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
docker run -d -p 8080:8080 --name adminer adminer

To open Adminer, please open [http://localhost:8080/](http://localhost:8080/)
on your browser, enter the IP of your MySQL Docker instance (see below) as host
and login with its password (default: root|todo)

To get the IPs of the containers inside the Docker network. That can be queried
with: `docker inspect network bridge -f '{{json .Containers}}' | jq`

* Notice that you'll need jq to format the output.


## RabbitMQ Management Console
RabbitMQ is an open source multi-protocol messaging broker. It's used in this
project via MassTransit to provide asynchronous communications via pub/sub and
async request/responses. RabbitMQ Management Console is available at:
[http://localhost:15672/](http://localhost:15672/). Login with guest/guest.


# Configuration
By default, the apps are configured at:
    * Web: http://localhost:21400
    * Catalog: http://localhost:21401
    * Newsletter: http://localhost:21402
    * Order: http://localhost:21403
    * Account: http://localhost:21404
    * Recommendation: http://localhost:21405
    * Notification: http://localhost:21406
    * Payment: http://localhost:21407
    * Shipping: http://localhost:21408


## Changing Configuration
todo add:
    * configure volumes?
    * configure networks
    * configure GMAIL smtp

Ex: passing config to dotnet on the command line:
dotnet run DbSettings:ConnStr="mongodb://brunobruno:12345"

With env variables:
todo


## Docker Compose
todo


## Kubernetes
todo


# Cheatsheet
This got complicated enough. Here are some commands to free up
some of your memory.

```s
# running the instances individually
docker run --name web -p 21400:80 web
docker run --name catalog -p 21401:80 catalog
docker run --name account -p 21404:80 account
docker run --name order -p 21403:80 order
docker run --name payment -p 21407:80 payment
docker run --name recommendation -p 21405:80 recommendation
docker run --name notification -p 21406:80 notification
docker run --name newsletter -p 21402:80 newsletter
docker run --name shipping -p 21408:80 shipping

```


# References
* Create a web API with ASP.NET Core and MongoDB | https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-3.1&tabs=visual-studio
* Google Accounts - Sign in using App Passwords | https://support.google.com/accounts/answer/185833
