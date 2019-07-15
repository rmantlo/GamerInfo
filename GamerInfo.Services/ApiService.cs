using GamerInfo.Data;
using GamerInfo.Models.ApiModels;
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
        private readonly string _userId;
        private readonly bool _isFamilyFriendly;
        public ApiService(Guid userId)
        {
            _userId = userId.ToString();
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Users.Single(e => e.Id == _userId);
                _isFamilyFriendly = entity.IsFamilyFriendly;
            }
        }

        //calls api and saves basic data in model
        public List<ApiDisplay> BrowseGames()
        {
            var epochTry = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            int epoch = Convert.ToInt32(epochTry);

            string str = $"fields name,popularity,cover,summary,genres,first_release_date,age_ratings; where first_release_date < {epoch}; sort popularity desc;";
            return GetApiGames(str);
        }
        public List<ApiDisplay> SearchResults()
        {
            return null;
        }
        private List<ApiDisplay> GetApiGames(string str)
        {
            List<ApiFirstCall> gameObject;

            using (var client = new WebClient())
            {
                //release_dates, age_ratings, genres
                client.BaseAddress = URL;
                client.Headers.Add("user-key", Key);

                string gameResult = client.UploadString(URL + "games/", str);
                List<ApiFirstCall> obj = JsonConvert.DeserializeObject<List<ApiFirstCall>>(gameResult);
                gameObject = obj;

                List<ApiDisplay> displayList = new List<ApiDisplay>();

                foreach (var item in obj)
                {
                    //covers
                    if (item.Cover != null)
                    {
                        int coverId = int.Parse(item.Cover);
                        string response = GetCoverInfo(coverId);
                        item.Cover = $"https://images.igdb.com/igdb/image/upload/t_cover_big/{response}.jpg";
                    }
                    else
                    {
                        item.Cover = "~Content/Assets/noImage.jpg";
                    }
                    //genres
                    List<string> listOfGenres = new List<string>();
                    foreach (var genre in item.Genres)
                    {
                        int genres = int.Parse(genre);
                        string gResponse = GetGenreInfo(genres);
                        listOfGenres.Add(gResponse);
                    }
                    item.Genres = null;
                    item.Genres = listOfGenres.ToArray();
                    //releasedates
                    if (item.First_release_date != null)
                    {
                        int release = int.Parse(item.First_release_date);
                        var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(release).ToShortDateString();

                        item.First_release_date = date.ToString();
                    }
                    else item.First_release_date = null;
                    //ageratings
                    if (item.Age_ratings != null)
                    {
                        int ageRatingId = int.Parse(item.Age_ratings[0]);
                        string aResponse = GetAgeRatingInfo(ageRatingId);
                        List<string> ratingList = new List<string>();
                        ratingList.Add(aResponse);
                        item.Age_ratings = ratingList.ToArray();
                    }
                    else
                    {
                        List<string> rate = new List<string> { "No Rating" };
                        item.Age_ratings = rate.ToArray();
                    }

                    string genreStr = "";
                    foreach (var genre in item.Genres)
                    {
                        genreStr = genreStr + " " + genre;
                    }
                    ApiDisplay model = new ApiDisplay
                    {
                        GameID = int.Parse(item.Id),
                        Name = item.Name,
                        Summary = item.Summary,
                        CoverID = item.Cover,
                        Genre = genreStr,
                        AgeRating = item.Age_ratings[0],
                        ReleaseDate = DateTime.Parse(item.First_release_date)
                    };
                    if (_isFamilyFriendly)
                    {
                        if (model.AgeRating != "AO" && model.AgeRating != "M" && model.AgeRating != "Rating Pending" && model.AgeRating != "18+" && model.AgeRating != "No Rating")
                        {
                            displayList.Add(model);
                        }

                        
                    } else
                    {
                        displayList.Add(model);
                    }
                }
                return displayList;
            };
        }

        //get cover info
        public string GetCoverInfo(int coverId)
        {
            using (var client = new WebClient())
            {
                client.BaseAddress = URL;
                client.Headers.Clear();
                client.Headers.Add("user-key", Key);
                string coverData = $"fields id,game,image_id; where id = {coverId};";
                string coverResult = client.UploadString(URL + "covers/", coverData);
                List<ApiCover> result = JsonConvert.DeserializeObject<List<ApiCover>>(coverResult);
                return result[0].Image_id;
            }
        }
        //get Genres info
        public string GetGenreInfo(int genreId)
        {
            using (var client = new WebClient())
            {
                client.BaseAddress = URL;
                client.Headers.Clear();
                client.Headers.Add("user-key", Key);
                string genreData = $"fields id,name; where id = {genreId};";
                string genreResult = client.UploadString(URL + "genres/", genreData);
                List<ApiGenre> result = JsonConvert.DeserializeObject<List<ApiGenre>>(genreResult);

                return result[0].Name;
            }
        }
        //get release date info
        public string GetReleaseDateInfo(int releaseId)
        {
            using (var client = new WebClient())
            {
                client.BaseAddress = URL;
                client.Headers.Clear();
                client.Headers.Add("user-key", Key);
                string releaseData = $"fields id,human; where id = {releaseId};";
                string releaseResult = client.UploadString(URL + "release_dates/", releaseData);
                List<ApiReleaseDates> listOfDates = JsonConvert.DeserializeObject<List<ApiReleaseDates>>(releaseResult);
                return listOfDates[0].Human;
            }
        }
        //get age rating information
        public string GetAgeRatingInfo(int ratingId)
        {
            using (var client = new WebClient())
            {
                client.BaseAddress = URL;
                client.Headers.Clear();
                client.Headers.Add("user-key", Key);
                string ratingData = $"fields id,rating; where id = {ratingId};";
                string result = client.UploadString(URL + "age_ratings/", ratingData);
                List<ApiAgeRating> response = JsonConvert.DeserializeObject<List<ApiAgeRating>>(result);
                if (response[0].Rating == "1")
                {
                    return "3+";
                }
                else if (response[0].Rating == "2")
                {
                    return "7+";
                }
                else if (response[0].Rating == "3")
                {
                    return "12+";
                }
                else if (response[0].Rating == "4")
                {
                    return "16+";
                }
                else if (response[0].Rating == "5")
                {
                    return "18+";
                }
                else if (response[0].Rating == "6")
                {
                    return "Rating Pending";
                }
                else if (response[0].Rating == "7")
                {
                    return "EC (Early Childhood)";
                }
                else if (response[0].Rating == "8")
                {
                    return "E";
                }
                else if (response[0].Rating == "9")
                {
                    return "E10";
                }
                else if (response[0].Rating == "10")
                {
                    return "T";
                }
                else if (response[0].Rating == "11")
                {
                    return "M";
                }
                else if (response[0].Rating == "12")
                {
                    return "AO";
                }
                else return null;
            }
        }

        //add to library (converts from apidisplay to GameData and saves)
        public bool AddGameToLibrary(ApiDisplay item)
        {
            Guid user = Guid.Parse(_userId);
            var entity = new GameData
            {
                OwnerID = user,
                GameID = item.GameID,
                Name = item.Name,
                Summary = item.Summary,
                CoverID = item.CoverID,
                AgeRating = item.AgeRating,
                ReleaseDate = item.ReleaseDate,
                Genre = item.Genre,
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Games.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }
    }
}
