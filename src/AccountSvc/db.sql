-- AccountSvc database init script 
create database if not exists accountdb;
use accountdb;

CREATE TABLE if not exists account (
    id                    INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name                  VARCHAR(1000)   NOT NULL,
    email                 VARCHAR(300)    NOT NULL,
    password              VARCHAR(1000)   NOT NULL,
    salt                  VARCHAR(100)    NOT NULL,
    created_at            DATETIME        NOT NULL,
    last_updated          DATETIME        NOT NULL,
    subscribe_newsletter  BIT
);

CREATE TABLE if not exists address (
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

CREATE TABLE if not exists payment_info (
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

CREATE TABLE if not exists event_type (
    id                    TINYINT         NOT NULL PRIMARY KEY,
    name                  VARCHAR(1000)   NULL
);

CREATE TABLE if not exists account_history (
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