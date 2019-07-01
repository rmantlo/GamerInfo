using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerInfo.Data
{
    public class SaveData
    {
        [Key]
        public int SaveID { get; set; }
        [Required]
        public Guid OwnerID { get; set; }
        [Required]
        public int GameID { get; set; }
        [Required]
        public string SaveTitle { get; set; }
        public string SaveInformation { get; set; }
        public decimal? Hours { get; set; }
        [DefaultValue("false")]
        public bool IsCurrentSave { get; set; }

    }
}
