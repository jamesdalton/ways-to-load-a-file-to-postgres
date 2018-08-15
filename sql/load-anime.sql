insert into type(
    name) 
(select distinct type 
   from anime_staging 
  where type is not null);

insert into anime (
    select anime_id, 
    name, 
    (select type.id 
       from type 
      where anime_staging.type = type.name),
    case 
      when episodes = 'Unknown' then null 
      else episodes::integer 
    end,
    rating,
    members
    from anime_staging);

insert into genre (
    name) 
(select distinct trim(genre) 
   from anime_staging, 
        lateral unnest(string_to_array(genres, ',')) genre);

insert into anime_genre(
    anime_id, 
    genre_id) 
(select distinct anime_id, 
        (select genre.id 
           from genre 
          where genre.name = trim(genre)) 
   from anime_staging, 
        lateral unnest(string_to_array(genres, ',')) genre);
