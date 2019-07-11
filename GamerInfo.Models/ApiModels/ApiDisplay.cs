using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerInfo.Models.ApiModels
{
    public class ApiDisplay
    {
        public int GameID { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string CoverID { get; set; }
        public string AgeRating { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Genre { get; set; }
    }
}
