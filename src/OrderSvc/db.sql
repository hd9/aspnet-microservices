-- OrderSvc database init script 
create database if not exists orderdb;
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