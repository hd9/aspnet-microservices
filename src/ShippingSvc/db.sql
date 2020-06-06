-- Shipping Database
create database if not exists shippingdb;
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
