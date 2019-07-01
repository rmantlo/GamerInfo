using GamerInfo.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerInfo.Data
{
    public class GameData
    {
        [Key]
        public int GameID { get; set; }
        [Required]
        public Guid OwnerID { get; set; }
        public Game GameList { get; set; }
        public string UserComments { get; set; }
    }
}
