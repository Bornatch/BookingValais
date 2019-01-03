using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DTO;
using DAL;

namespace BLL
{
    public class RoomManager
    {
        public static List<Room> GetAllRoomsByListId(List<int> idRooms)
        {
            List<Room> results = new List<Room>();
            results = RoomDb.GetAllRoomsByListId(idRooms);

            return results;
        }

        public static List<Room> GetRooms(int idHotel, DateTime dateStart, DateTime dateEnd)
        {
            List<Room> results = new List<Room>();

            for(int i = 0; i < RoomDb.GetRooms(idHotel, dateStart, dateEnd).Count; i++)
            {
                //loop through results of method to fill list
                results.Add(RoomDb.GetRooms(idHotel, dateStart, dateEnd)[i]);
            }

        return results;
     
        }
    }
}