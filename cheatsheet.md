# Cheatsheet
Here's some information to get started with the project.

## Debugging
* Open `src/AspNetMicroservices.sln` with Visual Studio 2019
* Configure SMTP Settings (see section above)
* Run the depencies with `docker-compose -f docker-compose.debug.yml up`
* From Visual Studio, run the project as debug (F5)

## Urls
By default, the microservices are configured to run at:
* **Web**: [http://localhost:8000](http://localhost:8000)
* **Catalog**: [http://localhost:8001](http://localhost:8001)
* **Newsletter**: [http://localhost:8002](http://localhost:8002)
* **Order**: [http://localhost:8003](http://localhost:8003)
* **Account**: [http://localhost:8004](http://localhost:8004)
* **Recommendation**: [http://localhost:8005](http://localhost:8005)
* **Notification**: [http://localhost:8006](v)
* **Payment**: [http://localhost:8007](http://localhost:8007)
* **Shipping**: [http://localhost:8008](http://localhost:8008)

And the management tools are available on:
* **Grafana**: [http://localhost:3000/](http://localhost:3000/)
* **MySQL Admin**: [http://localhost:8010/](http://localhost:8010/)
* **Mongo Express**: [http://localhost:8011/](http://localhost:8011/)
* **RabbitMQ dashboard**: [http://localhost:8012/](http://localhost:8012/)
* **Redis Commander**: [http://localhost:8013/](http://localhost:8013/)
* **The ELK Stack (Experimental)**: [http://localhost:5601/app/kibana#/home](http://localhost:5601/app/kibana#/home).

## Databases
Accessing the databases is also trivial. The simplest way to 
reach them out is by using `docker-compose` and
accessing them by their internal hostnames.
Since the hostnames are configured to match their respective
services, it should be straightforward to access them. 
For example, for `OrderSvc` is `order-db`, for `AccountSvc`, it's
`account-db`, and so on. For the full reference, check the
`src/docker-compose.yml` file.

### Accessing the MySQL databases with MySQL Admin
You can access the MySQL databases using MySQL Admin
(available at http://localhost:8010/), specifying the server
name (eg. `catalog-db`), and entering username: `root`,
password: `todo`. 
For example, to access the database for `OrderSvc`, open Adminer
and enter:
* **Server**: `order-db`
* **Username**: `root`
* **Password**: `todo`

### Accessing MongoDB with MongoExpress
You can access the MongoDB database using Mongo Express available at
http://localhost:8011/. No username or password is required.

### Accessing from code or via the command line
However, if you're looking accessing them via the commandline 
(or from code), here are the default urls. Please notice that 
you should bind these ports when running the container, else 
you won't be able to access them from the host (your machine):
* **catalog-db**: mongodb://localhost:3301
* **newsletter-db**: mysql://localhost:3302
* **order-db**: mysql://localhost:3303
* **account-db**: mysql://localhost:3304
* **recommendation-db**: mysql://localhost:3305
* **notification-db**: mysql://localhost:3306
* **payment-db**: mysql://localhost:3307
* **shipping-db**: mysql://localhost:3308

# Commands
The main commands to run are:
```s
# running just the backend dependencies (MySQL, MongoDB, Redis, RabbitMQ, etc)
docker-compose -f docker-compose.debug.yml up

# running the containers (recommended)
docker-compose up                           # start all the services in the foreground
docker-compose up -d                        # start all the services in the background
docker-compose down                         # stop and remove all the services
docker-compose up <service-name>            # start <service-name> and its dependencies. Ex: docker-compose up shipping

# building the images
docker-compose build

# running the instances individually
docker run --name web            -p 8000:80 web
docker run --name catalog        -p 8001:80 catalog
docker run --name newsletter     -p 8002:80 newsletter
docker run --name order          -p 8003:80 order
docker run --name account        -p 8004:80 account
docker run --name recommendation -p 8005:80 recommendation
docker run --name notification   -p 8006:80 notification
docker run --name payment        -p 8007:80 payment
docker run --name shipping       -p 8008:80 shipping
```
