using GamerInfo.Data;
using GamerInfo.Models;
using System;

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
    }
}
