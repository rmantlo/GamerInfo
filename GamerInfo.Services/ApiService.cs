using GamerInfo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GamerInfo.Services
{
    public class ApiService
    {
        private string URL = "https://api-v3.igdb.com/";
        private string Key = "4e0ed404bf691e52cb4cedf37ee1551d";

        public ApiService() { }

        public object GetApiGames()
        {
            using (var client = new WebClient())
            {
                //release_dates, age_ratings, genres
                client.Headers.Add("user-key", Key);
                string data = "fields name,popularity,cover,summary; sort popularity desc;";
                string result = client.UploadString(URL + "games/", data).Replace("\n", "");
                List<object> obj = JsonConvert.DeserializeObject<List<object>>(result);
                List<ApiDisplay> whattheheck = null;


                foreach (var item in obj)
                {
                    var stringIt = item.ToString();
                    var okay = JsonConvert.DeserializeObject<Dictionary<string, string>>(stringIt);
                    string id;
                    bool forFun = okay.TryGetValue("id", out id);
                    char[] plsdearlord = id.ToCharArray();
                    string whatever = new string(plsdearlord);
                    //int idAsNumberHopefully = whatever;
                    
                    
                    ApiDisplay browseApi = new ApiDisplay
                    {
                        GameID = whatever,
                        Name = okay["name"],
                        Summary = okay["summary"],
                    };
                }

                return obj;

            };
        }
    }
}
