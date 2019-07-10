using GamerInfo.Data;
using GamerInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerInfo.Services
{
    public class LibraryService
    {
        private readonly Guid _userId;

        public LibraryService(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<Game> GetGameList()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Games.Where(e => e.OwnerID == _userId).Select(e => new Game
                {
                    GameID = e.GameID,
                    Name = e.Name,
                    Summary = e.Summary,
                    CoverID = e.CoverID,
                    AgeRating = e.AgeRating,
                    ReleaseDate = e.ReleaseDate,
                    Genre = e.Genre,
                    UserComments = e.UserComments,
                });
                return query.ToArray();
            }
        }

        public bool CreateNewGame(Game game)
        {
            var entity = new GameData
            {
                OwnerID = _userId,
                Name = game.Name,
                Summary = game.Summary,
                CoverID = game.CoverID,
                AgeRating = game.AgeRating,
                ReleaseDate = game.ReleaseDate,
                Genre = game.Genre,
                UserComments = game.UserComments
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Games.Add(entity);
                ctx.SaveChanges();
                entity.GameID = entity.ID;
                return ctx.SaveChanges() == 1;
            }
        }

        public Game GetGameDetails(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Games.Single(e => e.OwnerID == _userId && e.GameID == id);
                return new Game
                {
                    GameID = entity.GameID,
                    Name = entity.Name,
                    Summary = entity.Summary,
                    CoverID = entity.CoverID,
                    AgeRating = entity.AgeRating,
                    ReleaseDate = entity.ReleaseDate,
                    Genre = entity.Genre,
                    UserComments = entity.UserComments
                };
            }
        }

        public bool UpdateGame(Game newGame)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var editEntity = ctx.Games.Single(e => e.OwnerID == _userId && e.GameID == newGame.GameID);
                editEntity.Name = newGame.Name;
                editEntity.CoverID = newGame.CoverID;
                editEntity.AgeRating = newGame.AgeRating;
                editEntity.ReleaseDate = newGame.ReleaseDate;
                editEntity.Genre = newGame.Genre;
                editEntity.Summary = newGame.Summary;
                editEntity.UserComments = newGame.UserComments;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool RemoveGame(int id)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var removedGame = ctx.Games.Single(e => e.OwnerID == _userId && e.GameID == id);
                ctx.Games.Remove(removedGame);
                return ctx.SaveChanges() == 1;
            };
        }
    }
}
