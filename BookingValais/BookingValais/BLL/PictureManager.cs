using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using Newtonsoft.Json;

namespace BLL
{
    public class PictureManager
    {
        //static HttpClient client = new HttpClient();
        static String baseUri = "http://localhost:3749/api/Pictures/";

        public static List<Picture> getPictures()
        {
            List<Picture> results = new List<Picture>();
            String uri = baseUri+"/Pictures";
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                results = JsonConvert.DeserializeObject<List<Picture>>(response.Result);
            }

            return results;
        }

        public static List<string> GetAllPicturesURL(int idHotel)
        {
            List<String> url = new List<string>();
            List<Picture> results = new List<Picture>();

            String uri = baseUri+ "Hotels/"+idHotel+"/pictures";

            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                results = JsonConvert.DeserializeObject<List<Picture>>(response.Result);
            }

            foreach(Picture p in results)
            {
                url.Add(p.Url);
            }

            return url;
        }
        
        public static string GetPicturesURL(int id)
        {
            //show only one picture when presented with the list of hotels (after search)

            String pictureUrl;

            String uri = baseUri + "GetPictures/" + id;


            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                pictureUrl = response.Result;
                Console.WriteLine(pictureUrl);

                //parse url to get the correct format
                pictureUrl = pictureUrl.Substring(0, pictureUrl.Length - 2);
                pictureUrl = pictureUrl.Substring(pictureUrl.IndexOf("http"));
            }

            return pictureUrl;

        }
    }
}