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
        private string URL = "https://api-v3.igdb.com/";
        HttpClient call;
        public ApiService()
        {
            var call = new HttpClient();

            call.BaseAddress = new Uri(URL);
            call.DefaultRequestHeaders.Accept.Clear();
            //call.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var IsHeader = call.DefaultRequestHeaders.TryAddWithoutValidation("user-key", "4e0ed404bf691e52cb4cedf37ee1551d");
        }

        public async void GetApiGames()
        {
            HttpResponseMessage responseMessage = await call.GetAsync(URL);
            List<Game> games = new List<Game>();
        }
    }
}
