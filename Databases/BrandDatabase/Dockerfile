FROM postgres:15.2-alpine

ENV POSTGRES_USER ${POSTGRES_USER}
ENV POSTGRES_PASSWORD ${POSTGRES_PASSWORD}
ENV POSTGRES_DB ${POSTGRES_BRAND_DATABASE}

COPY Scripts/*.sql /docker-entrypoint-initdb.d/

RUN addgroup -S pgsql-group && adduser -S pgsql-user -G pgsql-group
USER pgsql-user