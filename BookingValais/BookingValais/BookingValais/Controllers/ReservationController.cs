using DTO;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;

namespace BookingValais.Controllers
{
    public class ReservationController : Controller
    {
        // GET: Reservation
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ReserveRoom(int numberRooms, string dateStart, string dateEnd, string[] selectedRoom)
        {
            if(selectedRoom == null)
            {
                ViewData["Error"] = "Please select a room";
                return null;
            }
            if (Session["IdClient"] == null)
            {
                ViewData["Error"] = "You need to login in order to book a room.";
                return View("../Login/Login");
            }
            else
            {
                List<int> idRoomsToBook = new List<int>();

                decimal totalPrice = 0;
                for (int i = 0; i < selectedRoom.Length; i++)
                {
                    //retrieve id Room that is left of slash and price that is right of slash
                    int index = selectedRoom[i].IndexOf("/");
                    string idRoom = (index > 0 ? selectedRoom[i].Substring(0, index) : "");
                    string price = selectedRoom[i].Substring(index + 1);

                    idRoomsToBook.Add(Convert.ToInt32(idRoom));
                    totalPrice = totalPrice + Convert.ToDecimal(price);
                }

                ViewData["dateStart"] = dateStart;
                ViewData["dateEnd"] = dateEnd;

                //RoomManager.GetAllRoomsByListId(idRoomsToBook)
                ViewData["listRoomsToBook"] = BLL.RoomManager.GetAllRoomsByListId(idRoomsToBook);
                ViewData["totalPrice"] = totalPrice;

                return View("Confirmation");
            }

        }

        public ActionResult FinalizeReservation(string dateStart, string dateEnd, string totalPrice, string[] selectedRooms)
        {
            /*
             * 1) add dateStart, dateEnd, totalPrice and idClient in table Reservation
             * 2) retrieve last idReservation and save it
             * 3) loop through listOfRooms and each time call BLL.UpdateRoomReservation to insert idRoom and IdReservation
             * */
            int[] arrayIdRooms = new int[selectedRooms.Length];

            //extract all idrooms selected and put them in an array
            for (int i = 0; i < selectedRooms.Length; i++)
            {
                arrayIdRooms[i] = Convert.ToInt32(selectedRooms[i].ToString());
            }

            Client client = (Client)Session["IdClient"];

            //convert strings to appropriate formats
            DateTime dateStartUsable = Convert.ToDateTime(dateStart.ToString());
            DateTime dateEndUsable = Convert.ToDateTime(dateEnd.ToString());
            decimal totalPriceUsable = Convert.ToDecimal(totalPrice.ToString());

            //step 1
            ReservationManager.AddNewReservation(client.Idclient, dateStartUsable, dateEndUsable, totalPriceUsable);

            //step 2
            int idReservation = ReservationManager.GetLastIdReservation();

            //step 3
            for (int i = 0; i < arrayIdRooms.Length; i++)
            {
                ReservationManager.AddNewRoomReservation(arrayIdRooms[i], idReservation);
            }
            return View("Final");
        }

        public ActionResult Edit()
        {
            Client client = new Client();
            client = (Client)Session["IdClient"];
            int idClient = client.Idclient;

            ViewData["listReservations"] = ReservationManager.GetAllReservations(idClient);
            return View("Edit");
        }

        [HttpPost]
        public ActionResult Delete(string[] selectedReservation)
        {
            int idReservation = 0;
            int nbOfReservations = selectedReservation.Length;

            for (int i = 0; i < nbOfReservations; i++)
            {
                idReservation = Convert.ToInt32(selectedReservation[i].ToString());

                ReservationManager.DeleteRoomReservation(idReservation);

                ReservationManager.DeleteReservation(idReservation);
            }
            return View("DeleteOk");
        }

    }

    
}