using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using DTO;

namespace BLL
{
    public class ReservationManager
    {
        public static void DeleteReservation(int idReservation)
        {
            ReservationDb.DeleteReservation(idReservation);
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