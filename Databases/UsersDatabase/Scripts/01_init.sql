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

create table if not exists user_product_history
(
    id          bigserial,
    user_id     bigint not null,
    product_id  bigint not null,
    timestamp   timestamp with time zone default CURRENT_TIMESTAMP,
    primary key (id),
    foreign key (user_id) references users
        on delete cascade,
    foreign key (product_id) references products
        on delete cascade
);

create table if not exists user_experience
(
    id      bigserial,
    user_id bigint not null,
    score   bigint    default 0,
    level   integer   default 0,
    todos   integer[] default '{0,0,0}',
    primary key (id),
    foreign key (user_id) references users
        on delete cascade,
    unique (user_id)
);