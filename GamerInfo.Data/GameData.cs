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
        public int ID { get; set; }
        [Required]
        public Guid OwnerID { get; set; }
        public int GameID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public string CoverID { get; set; }
        [Required]
        public string AgeRating { get; set; }
        [Required]
        public DateTime? ReleaseDate { get; set; }
        [Required]
        public string Genre { get; set; }
        
        public string UserComments { get; set; }
    }
}
