namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var gamesDto = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);

            int tagsAdded = 0;
            int devsAdded = 0;
            int gamesAdded = 0;


            StringBuilder sb = new StringBuilder();

            var games = new List<Game>();
            ;
            foreach (var gameDto in gamesDto)
            {
                ;
                if (IsValid(gameDto) &&
                    !String.IsNullOrEmpty(gameDto.Name) &&
                    gameDto.Tags.Length > 0)
                {
                    Developer developer = GetDeveloper(context, gameDto.Developer);

                    Genre genre = GetGenre(context, gameDto.Genre);

                    //var tags = gameDto.Tags.Select(t => GetTag(context, t)).ToArray();

                    var tags = new List<Tag>();
                    foreach (var tagDto in gameDto.Tags)
                    {
                        Tag tag = GetTag(context, tagDto);
                        tags.Add(tag);
                    }

                    var game = new Game
                    {
                        Name = gameDto.Name,
                        Price = gameDto.Price,
                        ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Developer = developer,
                        Genre = genre,
                        GameTags = tags.Select(t => new GameTag
                        {
                            Tag = t,
                            
                        }).ToList()
                    };

                    games.Add(game);
                    gamesAdded++;
                    ;
                    sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
                    ;
                }
                else
                {
                    sb.AppendLine("Invalid Data");
                }
            }

            context.Games.AddRange(games);

            context.SaveChanges();
            ;
            return sb.ToString();
        }

        private static Tag GetTag(VaporStoreDbContext context, string tagDto)
        {
            Tag tag = context.Tags.FirstOrDefault(t => t.Name == tagDto);

            if (tag == null)
            {
                tag = new Tag
                {
                    Name = tagDto
                };

                context.Tags.Add(tag);
                context.SaveChanges();
            }

            return tag;
        }

        private static Genre GetGenre(VaporStoreDbContext context, string genreName)
        {
            Genre genre = context.Genres.FirstOrDefault(g => g.Name == genreName);

            if (genre == null)
            {
                genre = new Genre
                {
                    Name = genreName
                };
                context.Genres.Add(genre);
                context.SaveChanges();
            }

            return genre;
        }

        private static Developer GetDeveloper(VaporStoreDbContext context, string developerName)
        {
            Developer developer = context.Developers.FirstOrDefault(d => d.Name == developerName);

            if (developer == null)
            {
                developer = new Developer
                {
                    Name = developerName
                };

                context.Developers.Add(developer);
                context.SaveChanges();
            }

            return developer;
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }

    }
}