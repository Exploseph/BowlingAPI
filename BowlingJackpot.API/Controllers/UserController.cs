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
    public class UserController : ApiController
    {
        /// <summary>
        /// Returns all users
        /// </summary>
        /// <returns>List of class User</returns>
        public List<User> GetAllUsers()
        {
            using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
            {
                return rep.GetAllUsers();
            }
        }
        /// <summary>
        /// Returns a user by ID
        /// </summary>
        /// <param name="id">ID of the User</param>
        /// <returns>User</returns>
        public User GetUserByID(int id)
        {
            using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
            {
                return rep.GetUser(id);
            }
        }
        /// <summary>
        /// Inserts a user
        /// </summary>
        /// <param name="user">User to insert</param>
        /// <returns>User</returns>
        public HttpResponseMessage PostUser(User user)
        {
            if (ModelState.IsValid)
            {
                using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
                {
                    rep.AddUser(user);
                }
                return this.Request.CreateResponse<User>(HttpStatusCode.Created, user);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }
    }
}
