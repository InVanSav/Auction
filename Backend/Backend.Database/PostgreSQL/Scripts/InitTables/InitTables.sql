CREATE TABLE "Users"
(
    "id"       uuid PRIMARY KEY NOT NULL,
    "name"     varchar(35)      NOT NULL,
    "email"    varchar(50)      NOT NULL,
    "password" varchar(300)     NOT NULL
);

CREATE TABLE "Auctions"
(
    "id"          uuid PRIMARY KEY NOT NULL,
    "name"        varchar(35)      NOT NULL,
    "description" varchar(300)     NOT NULL,
    "dateStart"   timestamp DEFAULT now(),
    "dateEnd"     timestamp DEFAULT now(),
    "authorId"    uuid             NOT NULL,
    "state"       integer,
    CONSTRAINT fk_author FOREIGN KEY ("authorId") REFERENCES "Users" ("id")
);

CREATE TABLE "Lots"
(
    "id"          uuid PRIMARY KEY NOT NULL,
    "name"        varchar(35)      NOT NULL,
    "description" varchar(300)     NOT NULL,
    "auctionId"   uuid             NOT NULL,
    "startPrice"  decimal          NOT NULL,
    "buyoutPrice" decimal DEFAULT 0,
    "betStep"     decimal          NOT NULL,
    "state"       integer DEFAULT 0,
    CONSTRAINT fk_auction FOREIGN KEY ("auctionId") REFERENCES "Auctions" ("id")
);

CREATE TABLE "Bets"
(
    "id"       uuid PRIMARY KEY NOT NULL,
    "value"    decimal          NOT NULL,
    "lotId"    uuid             NOT NULL,
    "userId"   uuid             NOT NULL,
    "dateTime" timestamp,
    CONSTRAINT fk_lot FOREIGN KEY ("lotId") REFERENCES "Lots" ("id"),
    CONSTRAINT fk_user FOREIGN KEY ("userId") REFERENCES "Users" ("id")
);

CREATE TABLE "Images"
(
    "id"    uuid PRIMARY KEY NOT NULL,
    "lotId" uuid             NOT NULL,
    "path"  varchar(500)     NOT NULL,
    CONSTRAINT fk_lot FOREIGN KEY ("lotId") REFERENCES "Lots" ("id")
); 