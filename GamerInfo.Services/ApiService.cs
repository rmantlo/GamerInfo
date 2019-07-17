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
        private string Key = "71de76ca8da2d1fd5cfc5e23f7decfde";
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
        public List<ApiDisplay> SearchResults(string searchTerm)
        {
            string searchStr = $"fields game; search \"{searchTerm}\"; where game != null & popularity > 50;";
            List<int> resultIdList = new List<int>();
            using (var client = new WebClient())
            {
                client.BaseAddress = URL;
                client.Headers.Add("user-key", Key);
                string gameResult = client.UploadString(URL + "search/", searchStr);
                List<ApiSearch> obj = JsonConvert.DeserializeObject<List<ApiSearch>>(gameResult);

                foreach (ApiSearch item in obj)
                {
                    resultIdList.Add(item.Game);
                }
            }
            if (resultIdList.Count != 0)
            {
                string gameIdList = string.Join(",", resultIdList.ToArray());
                var epoch = Convert.ToInt32((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
                string pullSearchGames = $"fields name,popularity,cover,summary,genres,first_release_date,age_ratings; where first_release_date < {epoch} & id = ({gameIdList}); sort popularity desc; limit 10;";
                using (var client = new WebClient())
                {
                    client.BaseAddress = URL;
                    client.Headers.Add("user-key", Key);
                    string gameResult = client.UploadString(URL + "games/", pullSearchGames);
                    List<ApiFirstCall> obj = JsonConvert.DeserializeObject<List<ApiFirstCall>>(gameResult);
                    List<ApiDisplay> displaySearchList = new List<ApiDisplay>();

                    foreach (var item in obj)
                    {
                        ApiDisplay searchResult = new ApiDisplay
                        {
                            GameID = int.Parse(item.Id),
                            Name = item.Name,
                            Summary = item.Summary,
                        };
                        //covers
                        if (item.Cover != null)
                        {
                            string response = GetCoverInfo(item.Cover);
                            searchResult.CoverID = $"https://images.igdb.com/igdb/image/upload/t_cover_big/{response}.jpg";
                        }
                        else
                        {
                            searchResult.CoverID = "~Content/Assets/noImage.jpg";
                        }
                        //genres
                        List<string> listOfGenres = new List<string>();
                        if (item.Genres != null)
                        {
                            foreach (var genre in item.Genres)
                            {
                                listOfGenres.Add(genre);
                            }
                            string genreList = string.Join(",", listOfGenres.ToArray());
                            searchResult.Genre = GetGenreInfo(genreList);
                        }
                        else searchResult.Genre = "None";
                        //releasedates
                        if (item.First_release_date != null)
                        {
                            //int release = int.Parse(item.First_release_date);
                            var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(int.Parse(item.First_release_date));

                            searchResult.ReleaseDate = date;
                        }
                        else searchResult.ReleaseDate = null;
                        //ageratings
                        if (item.Age_ratings != null)
                        {
                            //int ageRatingId = int.Parse(item.Age_ratings[0]);
                            string aResponse = GetAgeRatingInfo(item.Age_ratings[0]);
                            //List<string> ratingList = new List<string>();
                            //ratingList.Add(aResponse);
                            //item.Age_ratings = ratingList.ToArray();
                            searchResult.AgeRating = aResponse;
                        }
                        else
                        {
                            searchResult.AgeRating = "No Rating";
                        }
                        if (_isFamilyFriendly)
                        {
                            if (searchResult.AgeRating != "AO" && searchResult.AgeRating != "M" && searchResult.AgeRating != "Rating Pending" && searchResult.AgeRating != "18+" && searchResult.AgeRating != "No Rating")
                            {
                                displaySearchList.Add(searchResult);
                            }
                        }
                        else
                        {
                            displaySearchList.Add(searchResult);
                        }
                    }
                    return displaySearchList;
                }
            }
            else return null;
        }

        public List<ApiDisplay> GetBrowseGames()
        {
            List<ApiFirstCall> gameObject;

            using (var client = new WebClient())
            {
                //release_dates, age_ratings, genres
                client.BaseAddress = URL;
                client.Headers.Add("user-key", Key);

                var epochTry = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
                int epoch = Convert.ToInt32(epochTry);

                string str = $"fields name,popularity,cover,summary,genres,first_release_date,age_ratings; where first_release_date < {epoch}; sort popularity desc; limit 10;";

                string gameResult = client.UploadString(URL + "games/", str);
                List<ApiFirstCall> obj = JsonConvert.DeserializeObject<List<ApiFirstCall>>(gameResult);
                gameObject = obj;

                List<ApiDisplay> displayList = new List<ApiDisplay>();

                foreach (var item in obj)
                {
                    ApiDisplay model = new ApiDisplay
                    {
                        GameID = int.Parse(item.Id),
                        Name = item.Name,
                        Summary = item.Summary
                    };

                    //covers
                    if (item.Cover != null)
                    {
                        string response = GetCoverInfo(item.Cover);
                        model.CoverID = $"https://images.igdb.com/igdb/image/upload/t_cover_big/{response}.jpg";

                    }
                    else
                    {
                        model.CoverID = "~Content/Assets/noImage.jpg";
                    }
                    //genres
                    List<string> listOfGenres = new List<string>();
                    if (item.Genres != null)
                    {
                        foreach (var genre in item.Genres)
                        {
                            listOfGenres.Add(genre);
                        }
                        string genreList = string.Join(",", listOfGenres.ToArray());
                        model.Genre = GetGenreInfo(genreList);
                    }
                    else model.Genre = "None";
                    //releasedates
                    if (item.First_release_date != null)
                    {
                        //int release = int.Parse(item.First_release_date);
                        var date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(int.Parse(item.First_release_date));

                        model.ReleaseDate = date;
                    }
                    else model.ReleaseDate = null;
                    //ageratings
                    if (item.Age_ratings != null)
                    {
                        string aResponse = GetAgeRatingInfo(item.Age_ratings[0]);
                        model.AgeRating = aResponse;
                    }
                    else
                    {
                        model.AgeRating = "No Rating";
                    }
                    if (_isFamilyFriendly)
                    {
                        if (model.AgeRating != "AO" && model.AgeRating != "M" && model.AgeRating != "Rating Pending" && model.AgeRating != "18+" && model.AgeRating != "No Rating")
                        {
                            displayList.Add(model);
                        }
                    }
                    else
                    {
                        displayList.Add(model);
                    }
                }
                return displayList;
            };
        }

        //get cover info
        public string GetCoverInfo(string coverId)
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
        public string GetGenreInfo(string genreId)
        {
            using (var client = new WebClient())
            {
                client.BaseAddress = URL;
                client.Headers.Clear();
                client.Headers.Add("user-key", Key);
                string genreResult = client.UploadString(URL + "genres/", $"fields id,name; where id = ({genreId});");
                List<ApiGenre> result = JsonConvert.DeserializeObject<List<ApiGenre>>(genreResult);
                List<string> genres = new List<string>();
                foreach (var item in result)
                {
                    genres.Add(item.Name);
                }
                return string.Join(", ", genres.ToArray());
            }
        }
        //get age rating information
        public string GetAgeRatingInfo(string ratingId)
        {
            using (var client = new WebClient())
            {
                client.BaseAddress = URL;
                client.Headers.Clear();
                client.Headers.Add("user-key", Key);
                //string ratingData = $"fields id,rating; where id = {ratingId};";
                string result = client.UploadString(URL + "age_ratings/", $"fields id,rating; where id = {ratingId};");
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
        //compare game to my library
        public bool CheckMyLibrary(ApiDisplay game)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var userId = Guid.Parse(_userId);
                var result = ctx.Games.SingleOrDefault(e => e.OwnerID == userId && e.GameID == game.GameID);
                if (result != null)
                {
                    return false;
                }
                else return true;
            }
        }
    }
}
