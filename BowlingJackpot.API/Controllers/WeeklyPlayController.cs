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
    public class WeeklyPlayController : ApiController
    {
        /// <summary>
        /// Gets all weekly jackpot plays
        /// </summary>
        /// <returns>List of class WeeklyPlay</returns>        
        public List<WeeklyPlay> GetWeeklyPlays()
        {
            using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
            {
                return rep.GetAllWeeklyPlays();
            }
        }

        /// <summary>
        /// Picks the weekly bowler to roll for the jackpot. Returns the current weekly play if a bowler is already selected.
        /// </summary>
        /// <returns>WeeklyPlay</returns>          
        public WeeklyPlay PutWeeklyPlay()
        {
            using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
            {
                return rep.PickWeeklyPlayer();
            }
        }

        /// <summary>
        /// Updates a weekly play.
        /// </summary>
        /// <param name="weeklyPlay">WeeklyPlay to be updated</param>
        /// <returns>WeeklyPlay</returns>              
        public HttpResponseMessage PostWeeklyPlay(WeeklyPlay weeklyPlay)
        {
            if (ModelState.IsValid)
            {
                using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
                {
                    var result = rep.UpdateWeeklyPlay(weeklyPlay);
                    var statusCode = HttpStatusCode.Created;
                    if (result == null)
                        statusCode = HttpStatusCode.Forbidden;
                    return this.Request.CreateResponse<WeeklyPlay>(statusCode, result);
                }
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
    }
}
