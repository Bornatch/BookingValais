using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAL
{
    public class ReservationDb
    {
        public static void DeleteReservation(int idReservation)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Reservations WHERE idReservation = @idReservation";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("idReservation", idReservation);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void DeleteRoomReservation(int idReservation)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM RoomReservations WHERE Reservation_IdReservation = @idReservation";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("idReservation", idReservation);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void AddNewReservation(int idClient, DateTime dateStart, DateTime dateEnd, decimal totalPrice)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Reservations (idClient, DateStart, DateEnd, TotalPrice) " +
                                    "VALUES(@idClient, @DateStart, @DateEnd, @TotalPrice)";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("idClient", idClient);
                    cmd.Parameters.AddWithValue("DateStart", dateStart);
                    cmd.Parameters.AddWithValue("DateEnd", dateEnd);
                    cmd.Parameters.AddWithValue("TotalPrice", totalPrice);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void AddNewRoomReservation(int idRoom, int idReservation)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO RoomReservations (Room_IdRoom, Reservation_IdReservation) " +
                                    "VALUES(@IdRoom, @IdReservation)";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("IdRoom", idRoom);
                    cmd.Parameters.AddWithValue("IdReservation", idReservation);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Reservation> GetAllReservations(int idClient)
        {
            List<Reservation> results = new List<Reservation>();

            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT DISTINCT Reservations.DateStart, Reservations.DateEnd, Reservatiosn.idReservation, Reservations.TotalPrice, " +
                        "(SELECT Name FROM Hotels WHERE IdHotel = " +
                        "(SELECT IdHotel From Rooms WHERE Rooms.IdRoom = RoomReservations.Room_IdRoom)) As Hotels " +
                        "FROM Reservations " + 
                        "INNER JOIN RoomReservations ON Reservations.idReservation = RoomReservations.Reservation_IdReservation " +
                        "INNER JOIN Rooms ON RoomReservations.Room_IdRoom = Rooms.IdRoom " +
                        "WHERE Reservations.idClient = @IdClient AND Reservations.DateStart >= @Today;";

                    SqlCommand cmd = new SqlCommand(query, cn);

                    //only show reservations that are after today (you cant delete reservations AFTER you stayed at the hotel)
                    DateTime today = DateTime.Now;
                    cmd.Parameters.AddWithValue("IdClient", idClient);
                    cmd.Parameters.AddWithValue("Today", today);

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Reservation reservation = new Reservation();
                            reservation.IdReservation = (int)dr["idReservation"];
                            reservation.IdClient = idClient;
                            reservation.TotalPrice = (decimal)dr["TotalPrice"];
                            reservation.DateStart = (DateTime)dr["DateStart"];
                            reservation.DateEnd = (DateTime)dr["DateEnd"];
                            reservation.hotelName = (string)dr[4];

                            results.Add(reservation);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return results;
        }


        //used in order to delete the appropriate reservation
        public static int GetLastIdReservation()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;
            int result = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT TOP 1 * FROM Reservations ORDER BY idReservation DESC";

                    SqlCommand cmd = new SqlCommand(query, cn);

                    cn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result = (int)dr["idReservation"];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }
    }
}
