CREATE TABLE "Users"
(
    "id"       uuid PRIMARY KEY NOT NULL,
    "name"     varchar(35)      NOT NULL,
    "email"    varchar(50)      NOT NULL,
    "password" varchar(300)     NOT NULL
);