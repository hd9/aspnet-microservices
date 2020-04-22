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
    * RabbitMQ

Start by pulling the images with:
docker pull mongo:latest
docker pull rabbitmq:latest


## Running the dependencies
We'll need to run RabbitMQ and two instances of MongoDB.

Run RabbitMQ with:
docker run --name r1 -it -h rh -p 5672:5672 rabbitmq

Run MongoDbs as:
docker run -d --name m2 -p 32768:27017 mongo                                        # for NewsletterSvc
docker run -d --name m2 -p 32769:27017 mongo                                        # for CatalogSvc

Run MySql:
docker run -d --name sql1 -p 3306:3306 -e MYSQL_ROOT_PASSWORD=hilden mysql          # for NotificationsSvc


## Seeding the databases

### Seeding Product data
Connect to the catalot mongodb  instance with:
mongo mongodb://localhost:32769

And run:
```js
use catalog
db.products.insertMany([
    { Name: "Xbox X", Description: "The Xbox Series X is an upcoming video game console from Microsoft. Announced December 12, 2019, it is the successor to the Xbox One and the fourth console in the Xbox family of consoles.", Price: 500, Currency: "USD" },
    { Name: "Playstation 5", Description: "The PlayStation 5 is an upcoming home video game console developed by Sony Interactive Entertainment. Announced as the successor to the PlayStation 4 in 2019, its launch is scheduled for late 2020.", Price: 500, Currency: "USD" },
    { Name: "Nintendo Switch", Description: "The Nintendo Switch is a video game console developed by Nintendo, released worldwide in most regions on March 3, 2017. It is a hybrid console that can be used as a home console and portable device.", Price: 300, Currency: "USD" },
    { Name: "Playstation 4", Description: "The PlayStation 4 is an eighth-generation home video game console developed by Sony Interactive Entertainment. Announced as the successor to the PlayStation 3 in February 2013, it was launched on November 15 in North America, November 29 in Europe, South America and Australia, and on February 22, 2014 in Japan.", Price: 300, Currency: "USD" },
    { Name: "Xbox One", Description: "The Xbox One is an eighth-generation home video game console developed by Microsoft. Announced in May 2013, it is the successor to Xbox 360 and the third console in the Xbox series of video game consoles.", Price: 300, Currency: "USD" },
    { Name: "Wii U", Description: "The Wii U is a home video game console developed by Nintendo as the successor to the Wii. Released in late 2012, it is the first eighth-generation video game console and competed with Microsoft's Xbox One and Sony's PlayStation 4. The Wii U is the first Nintendo console to support HD graphics", Price: 200, Currency: "USD" },
]);


### Seeding Datbase
Connect to the database with:
mysql --protocol=tcp -u root -p

Create a database and table:
create database hildenco;
use hildenco;
CREATE TABLE notifications (
    id          INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(1000)   NOT NULL,
    email       VARCHAR(300)    NOT NULL,
    created_at  DATETIME        NOT NULL,
    type        char(1)         NOT NULL
);

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