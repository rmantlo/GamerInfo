using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerInfo.Models.ApiModels
{
    public class ApiFirstCall
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Popularity { get; set; }
        public string Summary { get; set; }
        public string Cover { get; set; }
        public string[] Release_dates { get; set; }
        public string[] Age_ratings { get; set; }
        public string[] Genres { get; set; }
    }
}
