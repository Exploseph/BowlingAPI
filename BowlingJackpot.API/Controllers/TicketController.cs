using BowlingJackpotModel;
using BowlingJackpotModel.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BowlingJackpot.API.Controllers
{
    public class TicketController : ApiController
    {
        /// <summary>
        /// Returns all tickets purchased.
        /// </summary>
        /// <returns>List of class Ticket</returns>
        public List<Ticket> GetAllTickets()
        {
            using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
            {
                return rep.GetAllTickets();
            }
        }
        /// <summary>
        /// Returns ticket by ID
        /// </summary>
        /// <param name="id">ID of the Ticket</param>
        /// <returns>Ticket</returns>
        public Ticket GetTicketByID(int id)
        {
            using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
            {
                return rep.GetTicket(id);
            }
        }
        /// <summary>
        /// Inserts a ticket
        /// </summary>
        /// <param name="ticket">The Ticket to be added</param>
        /// <returns>Ticket</returns>
        public HttpResponseMessage PostTicket(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
                {
                    ticket.TicketAmount = rep.GetCurrentTicketAmount();
                    ticket.Purchase_Date = DateTime.UtcNow;
                    rep.AddTicket(ticket);
                }
                return this.Request.CreateResponse<Ticket>(HttpStatusCode.Created, ticket);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
    }
}
