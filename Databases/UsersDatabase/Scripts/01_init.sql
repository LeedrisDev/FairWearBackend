\c users_db;

create table if not exists users
(
    id                   bigserial,
    username             varchar not null,
    email                varchar not null,
    phone                varchar not null,
    language_preferences varchar default 'English':: character varying,
    theme                varchar default 'System default':: character varying,
    firebase_id          varchar not null,
    constraint check_theme
        check (theme in ('System default', 'Light', 'Dark')),
    primary key (id),
    unique (username),
    unique (firebase_id)
);

create index users_firebase_id_index
    on users (firebase_id);

create unique index users_id_index
    on users (id);

create table if not exists products
(
    id     bigint not null,
    name   varchar not null,
    rating integer not null,
    primary key (id)
);

create table if not exists brands
(
    id     bigint not null,
    name   varchar not null,
    rating integer not null,
    primary key (id)
);

create table if not exists user_product_history
(
    id          bigserial,
    user_id     bigint not null,
    product_id  bigint not null,
    timestamp   date default CURRENT_DATE,
    primary key (id),
    foreign key (user_id) references users
        on delete cascade,
    foreign key (product_id) references products
        on delete cascade
);

create table user_brand_history
(
    id          bigserial,
    user_id     bigint not null,
    brand_id    bigint not null,
    timestamp   date default CURRENT_DATE,
    primary key (id),
    foreign key (user_id) references users
        on delete cascade,
    foreign key (brand_id) references brands
        on delete cascade
);
