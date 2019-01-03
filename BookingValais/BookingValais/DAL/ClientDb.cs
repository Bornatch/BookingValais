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
    public class ClientDb
    {
        //get login
        public static Client GetClient(string surname, string name, string password)
        {
            Client client = new Client();

            string connectionString = ConfigurationManager.ConnectionStrings["HotelValais"].ConnectionString;
            try
            {
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    string query = "SELECT * FROM Client Where Name = @Name AND Surname = @Surname AND Password = @Password";

                    SqlCommand cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("Surname", surname);
                    cmd.Parameters.AddWithValue("Name", name);
                    cmd.Parameters.AddWithValue("Password", password);

                    cn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            client.Idclient = (int)dr["IdClient"];
                            client.Name = (string)dr["Name"];
                            client.Surname = (string)dr["Surname"];
                            client.Password = (string)dr["Password"];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return client;
        }
    }
}
