using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;
using Newtonsoft.Json;

namespace BLL
{
    public class ReservationManager
    {
        static String baseUri = "http://localhost:3749/api/";

        public static void DeleteReservation(int id)
            //id = idReservation
        {
            string uri = baseUri + "Reservations/DeleteReservation/" + id;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> response = httpClient.GetAsync(uri);
            }

        }

        public static void DeleteRoomReservation(int id)
                //id = idRoomReservation
        {
            string uri = baseUri + "RoomReservations/DeleteRoomReservation/" + id;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> response = httpClient.GetAsync(uri);
            }
        }

        public static void AddNewReservation(int idClient, DateTime dateStart, DateTime dateEnd, decimal totalPrice)
        {
            string dateStartText = dateStart.ToString("dd-MM-yyyy");
            string dateEndText = dateEnd.ToString("dd-MM-yyyy");
            string totalPricText = Convert.ToString(totalPrice);

            string uri = baseUri + "Reservations/AddNewReservation/" + idClient + "/" + dateStartText + "/" + dateEndText + "/" + totalPricText;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> response = httpClient.GetAsync(uri);
            }
        }

        public static void AddNewRoomReservation(int idRoom, int idReservation)
        {
            string uri = baseUri + "RoomReservations/AddNewRoomReservation/" + idRoom + "/" + idReservation;
            RoomReservation roomReservation = new RoomReservation();

            using (HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> response = httpClient.GetAsync(uri);
            }
        }

        public static List<Reservation> GetAllReservations(int id)
        {
            //id = idClient
            List<Reservation> results = new List<Reservation>();
            string uri = baseUri + "Reservations/GetAllReservations/" + id;

            using (HttpClient httpClient = new HttpClient())
            {

                Task<String> response = httpClient.GetStringAsync(uri);
                results = JsonConvert.DeserializeObject<List<Reservation>>(response.Result);
            }

            return results;
        }

        

        public static int GetLastIdReservation()
        {
            //GetLastIdReservation
            int result = 0;
            string uri = baseUri + "Reservations/GetLastIdReservation";

            using (HttpClient httpClient = new HttpClient())
            {

                Task<String> response = httpClient.GetStringAsync(uri);
                result = JsonConvert.DeserializeObject<int>(response.Result);
            }

            return result;
        }
    }
}