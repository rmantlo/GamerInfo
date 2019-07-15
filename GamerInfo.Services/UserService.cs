using GamerInfo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerInfo.Services
{
    public class UserService
    {
        private readonly string _userId;
        public UserService(string userId)
        {
            _userId = userId;
        }
        public bool UpdateUserFriendly(bool boolChange)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var editUser = ctx.Users.Single(e => e.Id == _userId);
                editUser.IsFamilyFriendly = boolChange;

                return ctx.SaveChanges() == 1;
            }
        }
        public bool UpdateThemeValue(int themeValue)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var edit = ctx.Users.Single(e => e.Id == _userId);
                edit.ThemeType = (TypeOfTheme)themeValue;

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
