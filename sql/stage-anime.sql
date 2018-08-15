drop table if exists anime_staging;
create table anime_staging(
    anime_id integer,
    name varchar(100),
    genres varchar(256),
    type varchar(10),
    episodes varchar(7),
    rating numeric(4,2),
    members integer);
\copy anime_staging from '/src/data/anime.csv' CSV HEADER
