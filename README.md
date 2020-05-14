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



# Configuring the microservices

## RabbitMQ
Run RabbitMQ with:
`docker run --name rabbitmq -d -p 5672:5672 rabbitmq`
## CatalogSvc
Holds catalog and product information.

### Running CatalogSvc docker image:
`docker run -d --name m-cat -p 32769:27017 mongo`

### Seeding Product data:
Connect to the catalog / mongodb instance with:
`mongo mongodb://localhost:32769`

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
    { Slug: "peaa-789", Name: "Patterns of Enterprise Application Architecture", Price: 45, Currency: "CAD", Description: "The practice of enterprise application development has benefited from the emergence of many new enabling technologies. Multi-tiered object-oriented platforms, such as Java and .NET, have become commonplace. These new tools and technologies are capable of building powerful applications, but they are not easily implemented. Common failures in enterprise applications often occur because their developers do not understand the architectural lessons that experienced object developers have learned.", CategoryId: "books", CategoryName: "Books", Rating: 3 } ,
    { Slug: "goia-789", Name: "Go in Action", Price: 45, Currency: "CAD", Description: "TODO", CategoryId: "books", CategoryName: "Books", Rating: 3 } ,
    { Slug: "alg-4445", Name: "Algorithms", Price: 45, Currency: "CAD", Description: " by Robert Sedgewick & Kevin Wayne TODO", CategoryId: "books", CategoryName: "Books", Rating: 4 } ,
    { Slug: "cd-789", Name: "Continuous Delivery", Price: 45, Currency: "CAD", Description: "by Jez Humble & David Farley", CategoryId: "books", CategoryName: "Books", Rating: 5 } ,
    { Slug: "dpero-1515", Name: "Design Patterns: Elements of Reusable Object-Oriented Software", Price: 35, Currency: "CAD", Description: "The Design of Everyday Things is a best-selling book by cognitive scientist and usability engineer Donald Norman about how design serves as the communication between object and user, and how to optimize that conduit of communication in order to make the experience of using the object pleasurable.", CategoryId: "books", CategoryName: "Books", Rating: 5 } ,
    { Slug: "dedt-131", Name: "The Design of Everyday Things", Price: 35, Currency: "CAD", Description: "Addison-Wesley Professional, 1994", CategoryId: "books", CategoryName: "Books", Rating: 5 } ,
    { Slug: "caw-1773", Name: "Coders at Work", Price: 35, Currency: "CAD", Description: "This is a who's who in the programming world - a fascinating look at how some of the best in the world do their work. Patterned after the best selling Founders at Work, the book represents two years of interviews with some of the top programmers of our times.", CategoryId: "books", CategoryName: "Books", Rating: 5 } ,
    { Slug: "tmmm-189", Name: "The Mythical Man-Month", Price: 35, Currency: "CAD", Description: "Few books on software project management have been as influential and timeless as The Mythical Man-Month. With a blend of software engineering facts and thought-provoking opinions, Fred Brooks offers insight for anyone managing complex projects. These essays draw from his experience as project manager for the IBM System/360 computer family and then for OS/360, its massive software system. Now, 20 years after the initial publication of his book, Brooks has revisited his original ideas and added new thoughts and advice, both for readers already familiar with his work and for readers discovering it for the first time.", CategoryId: "books", CategoryName: "Books", Rating: 5 } ,
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

## AccountSvc
Run the MySql image for the account service:
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
Run the order database with:
`docker run -d --name mysql-ordersvc -p 3308:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

Connect to the database with:
`mysql --protocol=tcp -u root -ptodo -P 3308`

Create db with:
`create database orderdb;`

And run the script to create the tables:
```sql
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

create table lineitem (
    id                  INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    order_id            INT             NOT NULL,
    name                VARCHAR(1000)   NOT NULL,
    price               DECIMAL(10,2)   NOT NULL,
    qty                 INT             NOT NULL,
    FOREIGN KEY (order_id)
        REFERENCES orders(id)
); 

CREATE TABLE payment_info (
    id                    INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    order_id              INT             NOT NULL,
    status                TINYINT         NOT NULL,
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
    status                TINYINT         NOT NULL,
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
```

## NewsletterSvc
Run the Newsletter db with:
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


## NotificationSvc
Run the Notification db with:
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

## PaymentSvc
Run the payment db with:
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
    created_at          DATETIME        NOT NULL,
    last_modified       DATETIME        NOT NULL,
    currency            VARCHAR(3)      NOT NULL,
    amount              DECIMAL(10,2)   NOT NULL,
    status              TINYINT         NOT NULL
);

create table log (
    id                  INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    pmt_id              INT             NOT NULL,
    created_at          DATETIME        NOT NULL,
    FOREIGN KEY (pmt_id)
        REFERENCES payment(id)
); 
```

## RecommendationSvc
Run the recommendation db with:
`docker run -d --name mysql-recommendationsvc -p 3310:3306 -e MYSQL_ROOT_PASSWORD=todo mysql`

Connect to the database with:
`mysql --protocol=tcp -u root -ptodo -P 3310`

Create the db with:
`create database recommendationdb;`

And run the script to create the tables:
```sql
use recommendationdb;

create table recommendation (
    id                  INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    account_id          INT             NOT NULL,
    order_id            INT             NOT NULL,
    created_at          DATETIME        NOT NULL
);

create table log (
    id                  INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    recomm_id           INT             NOT NULL,
    created_at          DATETIME        NOT NULL,
    FOREIGN KEY (recomm_id)
        REFERENCES recommendation(id)
);
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
