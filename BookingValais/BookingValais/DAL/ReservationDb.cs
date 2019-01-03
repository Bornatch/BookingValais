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
            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Reservation WHERE idReservation = @idReservation";

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
            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM RoomReservation WHERE idReservation = @idReservation";

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
            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Reservation (idClient, DateStart, DateEnd, TotalPrice) " +
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
            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO RoomReservation (IdRoom, IdReservation) " +
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

            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT DISTINCT Reservation.DateStart, Reservation.DateEnd, Reservation.idReservation, Reservation.TotalPrice, " +
                        "(SELECT Name FROM Hotel WHERE IdHotel = " +
                        "(SELECT IdHotel From Room WHERE Room.IdRoom = RoomReservation.IdRoom)) As Hotel " +
                        "FROM Reservation " + 
                        "INNER JOIN RoomReservation ON Reservation.idReservation = RoomReservation.IdReservation " +
                        "INNER JOIN Room ON RoomReservation.IdRoom = Room.IdRoom " +
                        "WHERE Reservation.idClient = @IdClient AND Reservation.DateStart >= @Today;";

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
            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            int result = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT TOP 1 * FROM Reservation ORDER BY idReservation DESC";

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
