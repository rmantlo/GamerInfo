using GamerInfo.Data;
using GamerInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GamerInfo.Services
{
    public class SaveService
    {
        private readonly Guid _userId;

        public SaveService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateSave(SaveCreate saveCreate)
        {
            var entity = new SaveData
            {
                SaveTitle = saveCreate.SaveTitle,
                SaveInformation = saveCreate.SaveInformation,
                Hours = saveCreate.Hours,
                OwnerID = _userId,
                GameID = saveCreate.GameID
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Saves.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<SaveDisplay> GetSaveInfoByGame(int gameId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var result = ctx.Saves.Where(e => e.OwnerID == _userId).Select(e => new SaveDisplay
                {
                    SaveTitle = e.SaveTitle,
                    SaveInformation = e.SaveInformation,
                    Hours = e.Hours,
                    GameID = e.GameID,
                    IsCurrent = e.IsCurrentSave
                });
                return result.ToArray();
            }
        }
    }
}
