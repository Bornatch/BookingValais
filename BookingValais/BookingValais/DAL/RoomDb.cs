using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DTO;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    public class RoomDb
    {
        public static List<Room> GetAllRoomsByListId(List<int> idRooms)
        {
            List<Room> ListOfRooms = new List<Room>();

            for(int i = 0; i<idRooms.Count;i++)
            {
                ListOfRooms.Add(GetRoomById(idRooms[i]));
            }

            return ListOfRooms;
        }
        public static Room GetRoomById(int idRoom)
        {
            Room room = new Room();

            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM RoRoomsom Where IdRoom = @IdRoom";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("IdRoom", idRoom);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            room.IdRoom = (int)dr["IdRoom"];
                            room.Number = (int)dr["Number"];
                            room.HasHairDryer = (Boolean)dr["HasHairDryer"];
                            room.HasTV = (Boolean)dr["HasTV"];
                            room.IdHotel = (int)dr["IdHotel"];
                            room.Price = (decimal)dr["Price"];
                            room.Description = (string)dr["Description"];
                            room.Type = (int)dr["Type"];
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            return room;
        }
        public static List<Room> GetRooms(int idHotel, DateTime dateStart, DateTime dateEnd)
        {
            List<Room> results = new List<Room>();

            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Rooms.IdRoom, Rooms.Description, Rooms.Type, Rooms.Price, Rooms.HasTV, Rooms.HasHairDryer " +
                                        "FROM Rooms " +
                                        "WHERE Rooms.IdHotel = @idHotel AND " +
                                        "Rooms.IdRoom NOT IN( " +
                                            "SELECT DISTINCT Rooms.IdRoom " +
                                            "FROM Rooms " +
                                            "INNER JOIN RoomReservations ON Rooms.IdRoom = RoomReservations.Room_IdRoom " +
                                            "INNER JOIN Reservations ON RoomReservations.Reservation_IdReservation = Reservations.IdReservation " +
                                            "INNER JOIN Hotels ON Rooms.IdHotel = Hotels.IdHotel " +
                                            "WHERE(@dateStart <= Reservations.DateEnd) " +
                                            "AND(@dateEnd > Reservations.DateStart) " +
                                            "AND Rooms.IdHotel = @idHotel) " +
                                        "ORDER BY Rooms.Price;";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("idHotel", idHotel);
                    cmd.Parameters.AddWithValue("dateStart", dateStart);
                    cmd.Parameters.AddWithValue("dateEnd", dateEnd);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        int totalNumberOfAvailableRooms = CountAvailableRooms(idHotel, dateStart, dateEnd, query);

                        while (dr.Read())
                        {
                            decimal totalNumberRooms = CountAllRooms(idHotel);
                            Room room = new Room();

                            room.IdRoom = (int)dr["IdRoom"];
                            room.Description = (string)dr["Description"];
                            room.Type = (int)dr["Type"];

                            //count number of nights
                            //source : https://stackoverflow.com/questions/33345344/calculate-number-of-nights-between-2-datetimes

                            var frm = dateStart < dateEnd ? dateStart : dateEnd;
                            var to = dateStart < dateEnd ? dateEnd : dateStart;
                            int totalDays = (int)(to - frm).TotalDays;
                            

                            //if booked over 70%, increase price by 20%
                            if (totalNumberOfAvailableRooms <= totalNumberRooms*30/100)
                            {
                                room.Price = (decimal)dr["Price"] * 120 / 100 * totalDays;
                            }
                            else
                            {
                                room.Price = (decimal)dr["Price"] * totalDays;
                            }
                            room.HasTV = (Boolean)dr["HasTV"];
                            room.HasHairDryer = (Boolean)dr["HasHairDryer"];
                            room.IdHotel = idHotel;

                            results.Add(room);
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

        public static int CountAvailableRooms(int idHotel, DateTime dateStart, DateTime dateEnd, string query)
        {
            int result = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("idHotel", idHotel);
                    cmd.Parameters.AddWithValue("dateStart", dateStart);
                    cmd.Parameters.AddWithValue("dateEnd", dateEnd);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            result++;
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


        public static int CountAllRooms(int idHotel)
        {
            int result = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM Rooms WHERE IdHotel = @idHotel;";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("idHotel", idHotel);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            result = (int)dr[0];
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