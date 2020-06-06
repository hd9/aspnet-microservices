-- NewsletterSvc database init script 
create database newsletterdb;
use newsletterdb;

CREATE TABLE newsletter (
    id          INT             NOT NULL AUTO_INCREMENT PRIMARY KEY,
    name        VARCHAR(1000)   NOT NULL,
    email       VARCHAR(300)    NOT NULL,
    created_at  DATETIME        NOT NULL
);