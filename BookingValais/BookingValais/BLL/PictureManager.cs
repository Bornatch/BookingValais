using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DAL;
using DTO;

namespace BLL
{
    public class PictureManager
    {
        public static List<string> GetAllPicturesURL(int idHotel)
        {
            return PictureDb.GetAllPicturesURL(idHotel);
        }
        
        public static string GetPicturesURL(int idHotel)
        {
            //show only one picture when presented with the list of hotels (after search)
            return PictureDb.GetPicturesURL(idHotel);
        }
    }
}