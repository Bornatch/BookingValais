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
            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT DISTINCT Url FROM Picture WHERE Picture.IdRoom IN" +
                                     "(SELECT IdRoom FROM Room WHERE Room.IdHotel = @idHotel);";

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

            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    //get the first image of the query
                    string query = "SELECT TOP 1 Url, IdPicture, IdPicture FROM Picture WHERE Picture.IdRoom IN" +
                                     "(SELECT IdRoom FROM Room WHERE Room.IdHotel = @idHotel)" +
                                     "ORDER BY Picture.IdPicture ASC;";

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