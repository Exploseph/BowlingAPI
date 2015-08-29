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
    public class JackpotController : ApiController
    {
        /// <summary>
        /// Returns the current jackpot balance
        /// </summary>
        /// <returns>Jackpot</returns>
        public Jackpot GetJackpot()
        {
            using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
            {
                return rep.GetJackpotBalance();
            }
        }
    }
}
