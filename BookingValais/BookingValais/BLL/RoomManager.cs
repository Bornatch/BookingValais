using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DAL;
using DTO;


namespace BLL
{
    public class RoomManager
    {

        static String baseUri = "http://localhost:3749/api/";

        public static List<Room> GetAllRoomsByListId(List<int> idRoomsList)
        {

            String uri = "";

            List<Room> results = new List<Room>();

            foreach(int idRoom in idRoomsList)
            {
                uri = baseUri + "Rooms/GetRoomById/" + idRoom;
                using (HttpClient httpClient = new HttpClient())
                {
                    Task<String> response = httpClient.GetStringAsync(uri);
                    results.Add(JsonConvert.DeserializeObject<Room>(response.Result));
                }
            }
            
            return results;
        }

        public static List<Room> GetRooms(int idHotel, string dateStart, string dateEnd)
        {
            List<Room> results = new List<Room>();

            String uri = baseUri + "Rooms/GetRooms/" + idHotel + "/" + dateStart + "/" + dateEnd;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                results = JsonConvert.DeserializeObject<List<Room>>(response.Result);
            }

            return results;


        }
    }
}