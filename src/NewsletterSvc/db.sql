-- NewsletterSvc database init script 
create database if not exists newsletterdb;
use newsletterdb;

CREATE TABLE if not exists newsletter (
    id          INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(1000)   NOT NULL,
    email       VARCHAR(300)    NOT NULL,
    created_at  DATETIME        NOT NULL
);