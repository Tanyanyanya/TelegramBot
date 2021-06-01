using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Models;

namespace TelegramBot.Commands
{
    public class AstroClient
    {
        private static string apiAddress = "https://astroapi0106.azurewebsites.net";


        public static async Task<List<SpaceWeatherResponse>> GetSpaceWeatherTypes()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiAddress);
            var res = await client.GetAsync("astroapi/weather/types");
            res.EnsureSuccessStatusCode();


            if (res.ToString() == "NoContent")
            {
                return null;
            }

            string responseBody = await res.Content.ReadAsStringAsync();

            if (responseBody == null || responseBody == "")
            {
                return null;
            }

            var data = JsonConvert.DeserializeObject<List<SpaceWeatherResponse>>(responseBody);

            return data;

        }

        public static async Task<SpaceWeatherResponse> GetNextWeatherByType(string type)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiAddress);
            using (HttpResponseMessage response = await client.GetAsync($"astroapi/weather/{type}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<SpaceWeatherResponse>(content);
                    if (result == null || result.Name == null)
                    {
                        return null;
                    }

                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }



        public static async Task<List<TypeOfEclipseResponse>> GetEclipseTypes()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiAddress);
            var res = await client.GetAsync("astroapi/eclipse/lunar_eclipse_types");
            res.EnsureSuccessStatusCode();


            if (res.ToString() == "NoContent")
            {
                return null;
            }

            string responseBody = await res.Content.ReadAsStringAsync();

            if (responseBody == null || responseBody == "")
            {
                return null;
            }

            var data = JsonConvert.DeserializeObject<List<TypeOfEclipseResponse>>(responseBody);

            return data;

        }


        public static async Task<EclipseInCityResponse> GetEclipseInCity(string City)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiAddress);
            using (HttpResponseMessage response = await client.GetAsync($"astroapi/eclipse/{City}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<EclipseInCityResponse>(content);
                    if (result == null || result.City == null)
                    {
                        return null;
                    }

                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }



        public static async Task<AsteroidsResponse> GetTranslate(string id)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiAddress);
            using (HttpResponseMessage response = await client.GetAsync($"astroapi/asteroids/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<AsteroidsResponse>(content);
                    if (result == null || result.id == null)
                    {
                        return null;
                    }

                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


        public static async Task<List<MarsExplorationModel>> GetMarsExplorInfoFromDb()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiAddress);
            var res = await client.GetAsync("astroapi/Mars_exploration");
            res.EnsureSuccessStatusCode();


            if (res.ToString() == "NoContent")
            {
                return null;
            }

            string responseBody = await res.Content.ReadAsStringAsync();

            if (responseBody == null || responseBody == "")
            {
                return null;
            }

            var data = JsonConvert.DeserializeObject<List<MarsExplorationModel>>(responseBody);

            return data;

        }





        public static async Task<List<AsteroidsResponse>> GetAsteroidsFromDb()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiAddress);
            var res = await client.GetAsync("astroapi/asteroids");
            res.EnsureSuccessStatusCode();


            if (res.ToString() == "NoContent")
            {
                return null;
            }

            string responseBody = await res.Content.ReadAsStringAsync();

            if (responseBody == null || responseBody == "")
            {
                return null;
            }

            var user = JsonConvert.DeserializeObject<List<AsteroidsResponse>>(responseBody);

            return user;

        }



        public static async Task<List<AsteroidsResponse>> GetAsteroids()
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(apiAddress);
            var result = await client.GetAsync("astroapi/asteroids");

            result.EnsureSuccessStatusCode();
            var content = result.Content.ReadAsStringAsync().Result;

            var asteroids = JsonConvert.DeserializeObject<Items>(content);

            return asteroids.asteroidsData;

        }

        public static async Task<PicOfDayModel> GetPicOfDay()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiAddress);
            var result = await client.GetAsync("astroapi/PictureOfDay");
            result.EnsureSuccessStatusCode();
            var content = result.Content.ReadAsStringAsync().Result;
            var picture = JsonConvert.DeserializeObject<PicOfDayModel>(content);


            return picture;
        }
        public static async Task<SpacePhotoModel> GetPhotoByTag(string tag)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(apiAddress);
            using (HttpResponseMessage response = await client.GetAsync($"astroapi/search_photo/{tag}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<SpacePhotoModel>(content);
                    if (result == null || result.Tag == null)
                    {
                        return null;
                    }

                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
