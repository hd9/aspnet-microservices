-- Recommendation database init script 
create database if not exists recommendationdb;
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
('g-ps4-456' , 'Playstation 4', 'PS4 is Sony\'s last gen console and one of the most sold consoles of all time', 0, sysdate(), sysdate()),
('g-ps4c-456', 'PS4 Controller', 'The PS4 Controller is the best companion for your PS4 console.', 0, sysdate(), sysdate()),
('g-ps4-fo4' , 'Fallout 4 (PS4)', 'Fallout 4 (PS4) is probably the best Lara Croft ever made!', 0, sysdate(), sysdate()),
('g-ps4-dsl3', 'Dark Souls 3 (PS4)', 'Featuring a variety of awe-inspiring locales (some grotesque, some majestic), the same finely-tuned combat fans had come to expect, and a new emphasis on speed and versatility inspired by FromSoftware\'s own Bloodborne', 0, sysdate(), sysdate()),
('g-ps4-ff15', 'Final Fantasy 15 (PS4)', 'Four adrenaline-driven teens embark on the journey of a lifetime, but this ain’t no Road Trip.', 0, sysdate(), sysdate()),

-- xbox stuff
('g-xbx1-123', 'Xbox one', 'Xbox one is Microsoft\'s last gen console and full of 5-start exclusives.', 0, sysdate(), sysdate()),
('g-xbxc-123', 'Xbox One Controller', 'The Xbox One Controller is the best companion for your Xbox One console.', 0, sysdate(), sysdate()),
('g-xbx1-d00m', 'Doom (Xbox One)', 'The DOOM Eternal Deluxe Edition includes: Year One Pass: Get access to two campaign expansions for the critically-acclaimed DOOM Eternal. An imbalance of power in the heavens requires the true ruler of this universe to rise and set things right.', 0, sysdate(), sysdate()),
('g-xbx1-fo4', 'Fallout 4 (Xbox One)', '', 0, sysdate(), sysdate()),
('g-xbx1-rmk', 'Resident Evil 2 Remake (Xbox One)', 'Capcom\'s remake of survival horror classic Resident Evil 2 has gone down a storm with fans and newcomers alike, and it\'s no surprise.', 0, sysdate(), sysdate());

-- insert some recommendations to start
insert into recommendation (product_slug, related_slug, hits, last_update)
values
-- ps4 recomms
('g-ps4-456', 'g-ps4c-456', 1000, sysdate()),
('g-ps4-456', 'g-ps4-fo4',  100, sysdate()),
('g-ps4-456', 'g-ps4-dsl3', 100, sysdate()),
('g-ps4-456', 'g-ps4-ff15', 100, sysdate()),
('g-ps4c-456', 'g-ps4-456', 100, sysdate()),
('g-ps4-fo4',  'g-ps4-456', 100, sysdate()),
('g-ps4-dsl3', 'g-ps4-456', 100, sysdate()),
('g-ps4-ff15', 'g-ps4-456', 100, sysdate()),

-- xbox recomms
('g-xbx1-123', 'g-xbxc-123', 10, sysdate()),
('g-xbx1-123', 'g-xbx1-d00m', 10, sysdate()),
('g-xbx1-123', 'g-xbx1-fo4', 10, sysdate()),
('g-xbx1-123', 'g-xbx1-rmk', 100, sysdate()),
('g-xbxc-123', 'g-xbx1-123', 100, sysdate()),
('g-xbx1-d00m','g-xbx1-123', 100, sysdate()),
('g-xbx1-fo4', 'g-xbx1-123', 100, sysdate()),
('g-xbx1-rmk', 'g-xbx1-123', 100, sysdate());
