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
`docker run -d -h hildenco --name rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:management-alpine`

On the command above we essentially exposed 2 ports
from the containers to our localhost:
    * 15672: Rabbitmq's management interface.
             Can be accessed at: http://localhost:15672/.
             Login with guest/guest
    * 5672: this is what our services will use to
            intercommunicate


## CatalogSvc
Holds catalog and product information.

Running CatalogSvc docker image:
`docker run -d --name mongo-catalog -p 32769:27017 mongo`

Seeding Product data:

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

// inserts categories
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
Run the order database with:
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
Run the recommendation db with:
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
