using GamerInfo.Models;
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

        public void GetApiGames()
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("user-key", Key);
                string data = "fields name,popularity; sort popularity desc;";
                var result = client.UploadString(URL + "games/", data);
                
            }
        }
    }
}
