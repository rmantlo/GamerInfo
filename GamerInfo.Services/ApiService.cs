using GamerInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GamerInfo.Services
{
    public class ApiService
    {
        public bool CallAllGames()
        {
            List<Game> ApiGames = null;
            using (var call = new HttpClient())
            {
                call.BaseAddress = new Uri("http//:api-v3.igdb.com/");
            }
            return false;
        }
    }
}
