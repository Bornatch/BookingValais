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

        public static List<string> GetPicturesHotel(int idHotel)
        {
            List<String> url = new List<string>();
            List<string> results = new List<string>();

            String uri = baseUri+ "GetPicturesHotel/"+idHotel;

            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                results = JsonConvert.DeserializeObject<List<string>>(response.Result);
            }

            return results;
        }
        
        public static List<string> GetPicturesURL(int id)
        {
            //show only one picture when presented with the list of hotels (after search)

            List<string> pictures;

            String uri = baseUri + "GetPicturesHotel/" + id;

            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                pictures = JsonConvert.DeserializeObject<List<string>>(response.Result);
            }

            return pictures;

        }
    }
}