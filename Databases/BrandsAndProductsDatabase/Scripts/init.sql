CREATE DATABASE fairwear_brand_and_product_database;
    
\c fairwear_brand_and_product_database;

DROP TABLE IF EXISTS brands;
DROP TABLE IF EXISTS products;

CREATE TABLE brands (
  id SERIAL PRIMARY KEY,
  name VARCHAR NOT NULL,
  country VARCHAR NOT NULL,
  environment_rating INTEGER NOT NULL,
  people_rating INTEGER NOT NULL,
  animal_rating INTEGER NOT NULL,
  rating_description VARCHAR NOT NULL,
  categories VARCHAR[] NOT NULL,
  ranges VARCHAR[] NOT NULL
);

CREATE TABLE products (
  id SERIAL PRIMARY KEY,
  upc_code VARCHAR NOT NULL,
  name VARCHAR NOT NULL,
  category VARCHAR,
  ranges VARCHAR[],
  brand_id INTEGER NOT NULL REFERENCES brands(id)
);