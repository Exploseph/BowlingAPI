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
    public class TicketAmountController : ApiController
    {
        /// <summary>
        /// Gets all Ticket Amounts
        /// </summary>
        /// <returns>TicketAmount</returns>
        public List<TicketAmount> GetTicketAmount()
        {
            using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
            {
                return rep.GetTicketAmounts();
            }
        }

        /// <summary>
        /// Adds a Ticket Amount.  This changes the Ticket Amount for all future Tickets.
        /// </summary>
        /// <param name="ticketAmount">Decimal.  Amount of all tickets going forward</param>
        /// <returns>TicketAmount</returns>
        public HttpResponseMessage PostTicketAmount(TicketAmount ticketAmount)
        {
            if (ModelState.IsValid)
            {
                using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
                {
                    rep.AddTicketAmount(ticketAmount);
                }
                return this.Request.CreateResponse<TicketAmount>(HttpStatusCode.Created, ticketAmount);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
    }
}
