using System;
using DTO;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class HotelDb
    {
        public static Hotel GetHotel(int idHotel)
        {
            Hotel result = new Hotel();

            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Hotel WHERE IdHotel = @idHotel";
                    
                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("idHotel", idHotel);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            result.IdHotel = (int)dr["IdHotel"];
                            result.Name = (string)dr["Name"];
                            result.Description = (string)dr["Description"];
                            result.Location = (string)dr["Location"];
                            result.Category = (int)dr["Category"];
                            result.HasWifi = (Boolean)dr["HasWifi"];
                            result.HasParking = (Boolean)dr["HasParking"];
                            result.Phone = (string)dr["Phone"];
                            result.Email = (string)dr["Email"];
                            result.Website = (string)dr["Website"];
                        }
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }

            return result;
        }

        public static List<Hotel> GetAvailableHotelsAdvanced(DateTime dateStart, DateTime dateEnd ,string location,
                                    Boolean hasWifi, Boolean hasParking, int category, int persons, Boolean hasTV, Boolean hasHairDryer)
        {
        /*get all hotels not reserved in these dates */
            List<Hotel> results = new List<Hotel>();
            //the datestart is set at midnight, we had seconds in order to do accurate comparisions in the query
            dateStart = dateStart.AddSeconds(86399);
            dateEnd = dateEnd.AddSeconds(86399);

            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT DISTINCT Hotel.IdHotel, Name, Hotel.Description, Location, Category, HasWifi, HasParking, Phone, Email, Website FROM Hotel "
                        + "INNER JOIN Room ON Room.IdHotel = Hotel.IdHotel " +
                        "WHERE Room.IdHotel IN( " +
                        "SELECT Room.IdHotel " +
                        "FROM Room " +
                        "WHERE IdRoom NOT IN( " +
                        "SELECT Room.IdRoom FROM Room " +
                        "INNER JOIN RoomReservation ON Room.IdRoom = RoomReservation.IdRoom " +
                        "INNER JOIN Reservation ON RoomReservation.IdReservation = Reservation.idReservation " +
                        "INNER JOIN Hotel ON Room.IdHotel = Hotel.IdHotel " +
                        "WHERE(@DateStart <= Reservation.DateEnd) AND (@DateEnd >= Reservation.DateStart) AND Room.Type != @Persons ";

                    //only tests this criteria if the checkbox has been ticked
                    if (hasTV == true)
                        query = query + "AND Room.HasTV = @HasTV ";
                    if (hasHairDryer == true)
                        query = query + "AND Room.HasHairDryer = @HasHairDryer";

                    query = query + "))" +
                        "AND " +
                        "Hotel.IdHotel IN( " +
                        "SELECT Hotel.IdHotel FROM Hotel " +
                        "WHERE " +
                        "Hotel.Location = @Location AND ";

                    //only test this criteria if the checkbox has been ticked
                    if (hasParking == true)
                        query = query + "Hotel.HasParking = @HasParking AND ";
                    if (hasWifi == true)
                        query = query + "Hotel.HasWifi = @HasWifi AND ";

                    query = query + "Hotel.Category >= @Category);";


                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("DateStart", dateStart);
                    cmd.Parameters.AddWithValue("DateEnd", dateEnd);
                    cmd.Parameters.AddWithValue("Location", location);
                    cmd.Parameters.AddWithValue("HasWifi", hasWifi);
                    cmd.Parameters.AddWithValue("HasParking", hasParking);
                    cmd.Parameters.AddWithValue("Category", category);
                    cmd.Parameters.AddWithValue("Persons", persons);
                    cmd.Parameters.AddWithValue("HasTV", hasTV);
                    cmd.Parameters.AddWithValue("HasHairDryer", hasHairDryer);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            //only if query gives results
                            if(dr.HasRows == true)
                            {
                                Hotel hotel = new Hotel();

                                hotel.IdHotel = (int)dr["IdHotel"];
                                hotel.Name = (string)dr["Name"];
                                hotel.Description = (string)dr["Description"];
                                hotel.Location = (string)dr["Location"];
                                hotel.Category = (int)dr["Category"];
                                hotel.HasWifi = (Boolean)dr["HasWifi"];
                                hotel.HasParking = (Boolean)dr["HasParking"];
                                hotel.Phone = (string)dr["Phone"];
                                hotel.Email = (string)dr["Email"];
                                hotel.Website = (string)dr["Website"];

                                results.Add(hotel);
                            }
                            
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

        public static List<Hotel> GetAvailableHotels(DateTime dateStart, DateTime dateEnd, string location, int persons)         
        {
            /*get all hotels not reserved in these dates */
            List<Hotel> results = new List<Hotel>();

            //the datestart is set at midnight, we had seconds in order to do accurate comparisions in the query
            dateStart = dateStart.AddSeconds(86399);
            dateEnd = dateEnd.AddSeconds(86399);

            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT DISTINCT Hotel.IdHotel, Name, Hotel.Description, Location, Category, HasWifi, HasParking, Phone, Email, Website FROM Hotel "
                        + "INNER JOIN Room ON Room.IdHotel = Hotel.IdHotel " +
                        "WHERE Room.IdHotel IN( " +
                        "SELECT Room.IdHotel " +
                        "FROM Room " +
                        "WHERE IdRoom NOT IN( " +
                        "SELECT Room.IdRoom FROM Room " +
                        "INNER JOIN RoomReservation ON Room.IdRoom = RoomReservation.IdRoom " +
                        "INNER JOIN Reservation ON RoomReservation.IdReservation = Reservation.idReservation " +
                        "INNER JOIN Hotel ON Room.IdHotel = Hotel.IdHotel " +
                        "WHERE(@DateStart <= Reservation.DateEnd) AND(@DateEnd >= Reservation.DateStart))" +
                        "AND " +
                        "Hotel.IdHotel IN( " +
                        "SELECT IdHotel FROM Hotel " +
                        "WHERE " +
                        "Hotel.Location = @Location) " +
                        "GROUP BY Room.IdHotel, Room.IdRoom);";

                    SqlCommand cmd = new SqlCommand(query, cn);

                    cmd.Parameters.AddWithValue("DateStart", dateStart);
                    cmd.Parameters.AddWithValue("DateEnd", dateEnd);
                    cmd.Parameters.AddWithValue("Location", location);
                    cmd.Parameters.AddWithValue("Persons", persons);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //only if query has results
                            if (dr.HasRows == true)
                            {
                                Hotel hotel = new Hotel();

                                hotel.IdHotel = (int)dr["IdHotel"];
                                hotel.Name = (string)dr["Name"];
                                hotel.Description = (string)dr["Description"];
                                hotel.Location = (string)dr["Location"];
                                hotel.Category = (int)dr["Category"];
                                hotel.HasWifi = (Boolean)dr["HasWifi"];
                                hotel.HasParking = (Boolean)dr["HasParking"];
                                hotel.Phone = (string)dr["Phone"];
                                hotel.Email = (string)dr["Email"];
                                hotel.Website = (string)dr["Website"];

                                results.Add(hotel);
                            }

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
    }
}
