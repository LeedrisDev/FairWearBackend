FROM postgres:15.2-alpine

ENV POSTGRES_USER=fairwear
ENV POSTGRES_PASSWORD=fairwear
ENV POSTGRES_DB=fairwear_brand_and_product_database

COPY Databases/BrandsAndProductsDatabase/Scripts/*.sql /docker-entrypoint-initdb.d/