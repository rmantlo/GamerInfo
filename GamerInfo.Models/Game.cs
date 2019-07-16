using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerInfo.Models
{
    public class Game
    {
        public int GameID { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string CoverID { get; set; }
        public string AgeRating { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ReleaseDate { get; set; }
        public string Genre { get; set; }
        public string UserComments { get; set; }
        public IEnumerable<SaveDisplay> SaveDisplay { get; set; }
    }
}
