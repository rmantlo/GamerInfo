using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerInfo.Models
{
    public class SaveDisplay
    {
        public string SaveTitle { get; set; }
        public string SaveInformation { get; set; }
        public decimal? Hours { get; set; }
        public int GameID { get; set; }
        public bool IsCurrent { get; set; }
    }
}
