CREATE TABLE IF NOT EXISTS public."Books"
(
    "Id"          integer                NOT NULL GENERATED ALWAYS AS IDENTITY,
    "Name"        character varying(50)  NOT NULL,
    "Description" character varying(200) NOT NULL,
    "Author"      character varying(50)  NOT NULL,
    PRIMARY KEY ("Id")
);