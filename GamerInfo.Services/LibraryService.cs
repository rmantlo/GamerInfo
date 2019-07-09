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
            using(var ctx = new ApplicationDbContext())
            {
                var query = ctx.Games.Where(e => e.OwnerID == _userId).Select(e => new Game
                {
                    GameID = e.GameID,
                    Name = e.Name,
                    Summary = e.Summary,
                    CoverID = e.CoverID,
                    AgeRating = e.AgeRating,
                    ReleaseDate = e.ReleaseDate,
                    Genre = e.Genre
                });
                return query.ToArray();
            }
        }

        public bool CreateNewGame(Game game)
        {
            var entity = new GameData
            {
                OwnerID = _userId,
                GameID = game.GameID,
                Name = game.Name,
                Summary = game.Summary,
                CoverID = game.CoverID,
                AgeRating = game.AgeRating,
                ReleaseDate = game.ReleaseDate,
                Genre = game.Genre
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Games.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
