\c brand_and_product_db;

create table if not exists brands
(
    id                 bigserial,
    name               varchar             not null,
    country            varchar             not null,
    environment_rating integer             not null,
    people_rating      integer             not null,
    animal_rating      integer             not null,
    rating_description varchar             not null,
    categories         character varying[] not null,
    ranges             character varying[] not null,
    primary key (id),
    unique (name),
    constraint check_environment_rating
        check ((environment_rating >= 0) AND (environment_rating <= 5)),
    constraint check_people_rating
        check ((people_rating >= 0) AND (people_rating <= 5)),
    constraint check_animal_rating
        check ((animal_rating >= 0) AND (animal_rating <= 5))
);

create table if not exists products
(
    id       bigserial,
    upc_code varchar not null,
    name     varchar not null,
    color    varchar(50),
    category varchar not null,
    ranges   character varying[],
    brand_id bigint  not null,
    primary key (id),
    foreign key (brand_id) references brands
        on delete cascade
);

create table if not exists integration_events
(
    id         bigserial,
    event  varchar not null,
    data  varchar not null,
    primary key (id)
);
