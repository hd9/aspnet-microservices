-- Payment database init script 
create database if not exists paymentdb;
use paymentdb;

create table if not exists payment (
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

create table if not exists payment_request (
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