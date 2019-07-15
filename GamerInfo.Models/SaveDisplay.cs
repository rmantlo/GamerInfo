using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerInfo.Models
{
    public class SaveDisplay
    {
        public int SaveID { get; set; }
        public string SaveTitle { get; set; }
        public string SaveInformation { get; set; }
        public decimal? Hours { get; set; }
        public int GameID { get; set; }
        [UIHint("Starred")]
        public bool IsCurrent { get; set; }
    }
}
