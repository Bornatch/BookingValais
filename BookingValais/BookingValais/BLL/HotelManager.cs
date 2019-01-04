﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DAL;
using DTO;
namespace BLL
{
    public class HotelManager
    {

        //static HttpClient client = new HttpClient();
        static String baseUri = "http://localhost:3749/api/";

        public static Hotel GetHotel(int idHotel)
        {
            Hotel h = new Hotel();
            String uri = baseUri + "Hotels/"+idHotel;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                h = JsonConvert.DeserializeObject<Hotel>(response.Result);
            }
            return h;
        }

        public static List<Hotel> GetHotels()
        {
            List<Hotel> hotels;
            String uri = baseUri + "Hotels";
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                hotels = JsonConvert.DeserializeObject<List<Hotel>>(response.Result);
            }

            return hotels;

        }

            public static List<Hotel> GetAvailableHotels(string dateStart, string dateEnd, string location, int persons)
        {
            //based on paramaters, this method will return a list of all possible rooms
            List<Hotel> results = new List<Hotel>();

            String uri = baseUri + "Hotels/GetAvailableHotels/" + dateStart + "/" + dateEnd + "/" + location + "/" + persons;
            using (HttpClient httpClient = new HttpClient())
            {
                Task<String> response = httpClient.GetStringAsync(uri);
                results = JsonConvert.DeserializeObject<List<Hotel>>(response.Result);
            }

            return results;

            //fills list with all results from research
            //for (int i = 0; i < HotelDb.GetAvailableHotels(dateStart, dateEnd, location, persons).Count; i++)

            //results = GetHotels();
        
            //return results;
        }

    public static List<Hotel> GetAvailableHotelsAdvanced(DateTime dateStart, DateTime dateEnd, String location,
                            Boolean hasWifi, Boolean hasParking, int category, int persons, Boolean hasTV, Boolean hasHairDryer)
    {
        //based on paramaters, this method will return a list of all possible rooms
        List<Hotel> results = new List<Hotel>();

        //fills list with all results from research
        for (int i = 0; i < HotelDb.GetAvailableHotelsAdvanced(dateStart, dateEnd, location, hasWifi, hasParking, category, persons, true, true).Count; i++)
        {
            results.Add(
            HotelDb.GetAvailableHotelsAdvanced(dateStart, dateEnd, location, hasWifi, hasParking, category, persons, true, true)[i]
            );
        }

        return results;
     }
        
    }
}
