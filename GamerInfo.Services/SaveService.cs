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
                var result = ctx.Saves.Where(e => e.OwnerID == _userId && e.GameID == gameId).Select(e => new SaveDisplay
                {
                    SaveTitle = e.SaveTitle,
                    SaveInformation = e.SaveInformation,
                    Hours = e.Hours,
                    GameID = e.GameID,
                    IsCurrent = e.IsCurrentSave,
                    SaveID = e.SaveID
                });
                return result.ToArray();
            }
        }

        public SaveDisplay getSaveById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Saves.Single(e => e.OwnerID == _userId && e.SaveID == id);
                return new SaveDisplay
                {
                    SaveTitle = entity.SaveTitle,
                    SaveInformation = entity.SaveInformation,
                    Hours = entity.Hours,
                    GameID = entity.GameID,
                    IsCurrent = entity.IsCurrentSave,
                    SaveID = entity.SaveID
                };
            }
        }

        public bool UpdateSaveFile(SaveDisplay save)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var oldEntity = ctx.Saves.Single(e => e.OwnerID == _userId && e.SaveID == save.SaveID);
                oldEntity.SaveTitle = save.SaveTitle;
                oldEntity.SaveInformation = save.SaveInformation;
                oldEntity.Hours = save.Hours;
                oldEntity.IsCurrentSave = save.IsCurrent;

                return ctx.SaveChanges() == 1;
            }
        }
        public bool RemoveSaveFileById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Saves.Single(e => e.OwnerID == _userId && e.SaveID == id);
                ctx.Saves.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }
        public bool RemoveAllSavesByGameId(int gameId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entities = ctx.Saves.Where(e => e.OwnerID == _userId && e.GameID == gameId);
                foreach (var item in entities)
                {
                    ctx.Saves.Remove(item);
                }
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
