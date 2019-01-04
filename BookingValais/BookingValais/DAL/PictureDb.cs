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
    public class PictureDb
    {
        public static List<string> GetAllPicturesURL(int idHotel)
        {
            List<string> results = new List<string>();
            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT DISTINCT Url FROM Pictures WHERE Pictures.Room_IdRoom IN" +
                                     "(SELECT IdRoom FROM Rooms WHERE Rooms.IdHotel = @idHotel);";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("idHotel", idHotel);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        //we only need the URL to show images in the View
                        while (dr.Read())
                        {
                            results.Add((string)dr["Url"]);
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

        public static string GetPicturesURL(int idHotel)
        {
            string result = "";

            string connectionString = ConfigurationManager.ConnectionStrings["HotelContext-20190102113238"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    //get the first image of the query
                    string query = "SELECT TOP 1 Url, IdPicture, IdPicture FROM Pictures WHERE Pictures.Room_IdRoom IN" +
                                     "(SELECT IdRoom FROM Rooms WHERE Rooms.IdHotel = @idHotel)" +
                                     "ORDER BY Pictures.IdPicture ASC;";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("idHotel", idHotel);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        //we only need the URL to show images in the view
                        while (dr.Read())
                        {
                            result = (string)dr["Url"];
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