# ASP.NET Microservices

This is a sample application to demo Microservices in .NET
using ASP.NET Core, Docker Compose, MongoDB, Vue.js, Azure App Services,
Azure AKS and Kubernetes.

To learn more about this app, Docker, Azure and microservices, please check our blog at: blog.hildenco.com

Source code at: github.com/hd9/aspnet-microservices

## License
The license for this project is MIT.


## Dependencies
The dependencies for this project are:
    * Docker Desktop (Mac, Windows) or Docker Engine (Linux)
    * .NET Core SDK 3.1

Optionally, to debug I suggest using Visual Studio 2019.

### Installing the requirements
To run this application, you'll need the following Docker images:
    * Mongo:latest
    * ASP.NET Core SDK
    * ASP.NET Core runtime
    * RabbitMQ:latest
    * MySQL:latest

Start by pulling the images with:
docker pull mongo:latest
docker pull rabbitmq:latest
docker pull mysql:latest



# Running the microservices

## RabbitMQ
Run RabbitMQ with:
docker run --name r1 -d       -p 5672:5672 rabbitmq
docker run --name r1 -d -h rh -p 5672:5672 rabbitmq

## CatalogSvc
docker run -d --name m-cat -p 32769:27017 mongo                                        # for CatalogSvc

## OrderSvc
-- todo --

## RecommendationSvc
-- todo --

## AccountSvc
docker run -d --name mysql-accountsvc -p 3307:3306 -e MYSQL_ROOT_PASSWORD=todo mysql

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
    created_at            DATETIME        NOT NULL,
    last_updated          DATETIME        NOT NULL,
    address               VARCHAR(1000),
    city                  VARCHAR(300),
    region                VARCHAR(100),
    country               VARCHAR(100),
    subscribe_newsletter  BIT
);

```

## OrderSvc
docker run -d --name mysql-ordersvc -p 3308:3306 -e MYSQL_ROOT_PASSWORD=todo mysql

Connect to the database with:
`mysql --protocol=tcp -u root -p -P 3308`

Create db with:
`create database orderdb;`

And run the script to create `order` and `orderline` tables:
```sql
create table lineitem (
    id                  INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    order_id            INT             NOT NULL,
    name                VARCHAR(1000)   NOT NULL,
    price               DECIMAL(10,2)   NOT NULL,
    qty                 INT             NOT NULL,
    FOREIGN KEY (order_id)
        REFERENCES orders(id)
); 

create table orders (
    id                  INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    account_id          INT             NOT NULL,
    created_at          DATETIME        NOT NULL,
    last_modified       DATETIME        NOT NULL,
    currency            varchar(3)      NOT NULL,
    price               DECIMAL(10,2)   NOT NULL,
    tax                 DECIMAL(10,2)   NOT NULL,
    shipping            DECIMAL(10,2)   NOT NULL,
    total_price         DECIMAL(10,2)   NOT NULL,
    status              TINYINT         NOT NULL
);
```

## NewsletterSvc
docker run -d --name mongo-nlsvc -p 32768:27017 mongo

## NotificationSvc
docker run -d --name mysql-notificationsvc -p 3306:3306 -e MYSQL_ROOT_PASSWORD=todo mysql



# Seeding the databases

### Seeding Product data
Connect to the catalot mongodb  instance with:
mongo mongodb://localhost:32769

And run:
```js
use catalog
db.Products.insertMany([
    { Slug: "xbx-123", Name: "Xbox One", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "ps4-456", Name: "PS4", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "ns-789", Name: "Nintendo Switch", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "ps5-753;", Name: "PS5", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 4 } ,
    { Slug: "xbxx-951", Name: "Xbox X Series", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 4 } ,
    { Slug: "wiiu-789", Name: "wii U", Price: 300, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "xbxc-123", Name: "Xbox One Controller", Price: 59, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "ps4c-456", Name: "PS4 Controller", Price: 59, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "nsc-789", Name: "Nintendo Switch Controller", Price: 59, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "ps5c-753;", Name: "PS5 Controller", Price: 69, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 4 } ,
    { Slug: "xbxc-951", Name: "Xbox X Series Controller", Price: 69, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 4 } ,
    { Slug: "wiiuc-789", Name: "wii U Controller", Price: 49, Currency: "CAD", Description: "TODO", CategoryId: "games", CategoryName: "Games", Rating: 5 } ,
    { Slug: "fdr-951", Name: "Fender Stratocaster", Price: 800, Currency: "CAD", Description: "TODO", CategoryId: "musical-instruments", CategoryName: "Musical Instruments", Rating: 5 } ,
    { Slug: "gib-789", Name: "Gibson Les Paul", Price: 1500, Currency: "CAD", Description: "TODO", CategoryId: "musical-instruments", CategoryName: "Musical Instruments", Rating: 3 } ,
    { Slug: "gljb-789", Name: "Geddy Lee's Jazz Bass", Price: 1500, Currency: "CAD", Description: "TODO", CategoryId: "musical-instruments", CategoryName: "Musical Instruments", Rating: 4 } ,
    { Slug: "pb-951", Name: "Paddle Boarding", Price: 500, Currency: "CAD", Description: "TODO", CategoryId: "sports", CategoryName: "Sports", Rating: 3 } ,
    { Slug: "jjk-789", Name: "Jiu-Jitsu Kimono", Price: 150, Currency: "CAD", Description: "TODO", CategoryId: "sports", CategoryName: "Sports", Rating: 3 } ,
    { Slug: "sb-789", Name: "Soccer Ball", Price: 15, Currency: "CAD", Description: "TODO", CategoryId: "sports", CategoryName: "Sports", Rating: 3 } ,
    { Slug: "goia-789", Name: "Go in Action", Price: 45, Currency: "CAD", Description: "TODO", CategoryId: "books", CategoryName: "Books", Rating: 3 } ,
    { Slug: "alg-4445", Name: "Algorithms", Price: 45, Currency: "CAD", Description: " by Robert Sedgewick & Kevin Wayne TODO", CategoryId: "books", CategoryName: "Books", Rating: 3 } ,
    { Slug: "cd-789", Name: "Continuous Delivery", Price: 45, Currency: "CAD", Description: "by Jez Humble & David Farley", CategoryId: "books", CategoryName: "Books", Rating: 3 } ,
    { Slug: "rpi-789", Name: "Raspberry Pi", Price: 45, Currency: "CAD", Description: "TODO", CategoryId: "computers", CategoryName: "Computers", Rating: 3 } ,
    { Slug: "pppro-789", Name: "Pine Book Pro ARM", Price: 45, Currency: "CAD", Description: "TODO", CategoryId: "computers", CategoryName: "Computers", Rating: 3 } ,
    { Slug: "pppro-789", Name: "Sony Bravia 55", Price: 450, Currency: "CAD", Description: "TODO", CategoryId: "tvs", CategoryName: "TVs", Rating: 4 } ,
    { Slug: "pppro-789", Name: "Toshiba Netflix 55", Price: 450, Currency: "CAD", Description: "TODO", CategoryId: "tvs", CategoryName: "TVs", Rating: 4 } ,
    { Slug: "ggc-789", Name: "Grass Curter", Price: 40, Currency: "CAD", Description: "TODO", CategoryId: "home", CategoryName: "Home & Garden", Rating: 4 } ,
    { Slug: "bbqc-789", Name: "BBQ Coal", Price: 40, Currency: "CAD", Description: "TODO", CategoryId: "home", CategoryName: "Home & Garden", Rating: 4 } ,
    { Slug: "seab-789", Name: "Sony Earbuddy", Price: 40, Currency: "CAD", Description: "TODO", CategoryId: "headphones-audio", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "bstd3-789", Name: "Beats Studio3 Wireless Headphones, Matte Black", Price: 400, Currency: "CAD", Description: "TODO", CategoryId: "headphones-audio", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "smb-506041", Name: "Sennheiser MB Pro 1 (506041)", Price: 166, Currency: "CAD", Description: "TODO", CategoryId: "headphones-audio", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "gcd-5041", Name: "Gaming Computer Desk with Storage for Controller, Headphone & Speaker - Black", Price: 159.71, Currency: "CAD", Description: "TODO", CategoryId: "headphones-audio", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "iphone-753", Name: "iPhone 7", Price: 400, Currency: "CAD", Description: "TODO", CategoryId: "phones", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "iphone-X11", Name: "iPhone X", Price: 999, Currency: "CAD", Description: "TODO", CategoryId: "phones", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "iphone-X11-case1", Name: "iPhone X Case", Price: 9.99, Currency: "CAD", Description: "TODO", CategoryId: "phones", CategoryName: "Headphones & Audio", Rating: 2 },
    { Slug: "sg-11", Name: "Samsung Galaxy 11", Price: 600, Currency: "CAD", Description: "TODO", CategoryId: "phones", CategoryName: "Headphones & Audio", Rating: 4 },
    { Slug: "sg-20", Name: "Samsung Galaxy 20", Price: 800, Currency: "CAD", Description: "TODO", CategoryId: "phones", CategoryName: "Headphones & Audio", Rating: 4 }
]);

// inserts categories
db.Categories.insertMany([
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

### Seeding the Notification datbase
Connect to the database with:
`mysql --protocol=tcp -u root -p`

Create a database and table:
```sql
create database notificationdb;
use notificationdb;
CREATE TABLE Notifications (
    id          INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(1000)   NOT NULL,
    email       VARCHAR(300)    NOT NULL,
    created_at  DATETIME        NOT NULL,
    type        char(1)         NOT NULL
);

-- create our user
create user 'svc'

```

## Changing Configuration
todo add:
    * configure volumes?
    * configure networks
    * configure GMAIL smtp

Ex: passing config to dotnet on the command line:
dotnet run DbSettings:ConnStr="mongodb://brunobruno:12345"

With env varialbes:
todo


## Running the Services
todo :: add how to run 




## Testing
Frontend: http://localhost:21400
Catalog: http://localhost:21401
Newsletter: http://localhost:21402/signup





# References
* Create a web API with ASP.NET Core and MongoDB | https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-3.1&tabs=visual-studio
* Google Accounts - Sign in using App Passwords | https://support.google.com/accounts/answer/185833
