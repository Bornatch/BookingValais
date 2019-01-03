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

            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Room Where IdRoom = @IdRoom";

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

            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Room.IdRoom, Room.Description, Room.Type, Room.Price, Room.HasTV, Room.HasHairDryer " +
                                        "FROM Room " +
                                        "WHERE Room.IdHotel = @idHotel AND " +
                                        "Room.IdRoom NOT IN( " +
                                            "SELECT DISTINCT Room.IdRoom " +
                                            "FROM Room " +
                                            "INNER JOIN RoomReservation ON Room.IdRoom = RoomReservation.IdRoom " +
                                            "INNER JOIN Reservation ON RoomReservation.IdReservation = Reservation.IdReservation " +
                                            "INNER JOIN Hotel ON Room.IdHotel = Hotel.IdHotel " +
                                            "WHERE(@dateStart <= Reservation.DateEnd) " +
                                            "AND(@dateEnd > Reservation.DateStart) " +
                                            "AND Room.IdHotel = @idHotel) " +
                                        "ORDER BY Room.Price;";

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

            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
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

            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT COUNT(*) FROM Room WHERE IdHotel = @idHotel;";

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