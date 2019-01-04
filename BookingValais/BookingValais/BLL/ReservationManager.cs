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
        static String baseUri = "http://localhost:3749/api/Reservations/";

        public static void DeleteReservation(int id)
            //id = idReservation
        {
            string uri = baseUri + "DeleteReservation/" + id;
            Reservation reservation = null;
            using (HttpClient httpClient = new HttpClient())
            {

                Task<String> response = httpClient.GetStringAsync(uri);
                reservation = JsonConvert.DeserializeObject<Reservation>(response.Result);
            }

        }

        public static void DeleteRoomReservation(int idReservation)
        {
            ReservationDb.DeleteRoomReservation(idReservation);
        }

        public static void AddNewRoomReservation(int idRoom, int idReservation)
        {
            ReservationDb.AddNewRoomReservation(idRoom, idReservation);
        }

        public static List<Reservation> GetAllReservations(int idClient)
        {
            return ReservationDb.GetAllReservations(idClient);
        }

            public static void AddNewReservation(int idClient, DateTime dateStart, DateTime dateEnd, decimal totalPrice)
        {
            ReservationDb.AddNewReservation(idClient, dateStart, dateEnd, totalPrice);
        }

        public static int GetLastIdReservation()
        {
            return ReservationDb.GetLastIdReservation();
        }
    }
}