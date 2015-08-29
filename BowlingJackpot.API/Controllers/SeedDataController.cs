using BowlingJackpotModel;
using BowlingJackpotModel.DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BowlingJackpot.API.Controllers
{
    public class SeedDataController : ApiController
    {
        /// <summary>
        /// Inserts test data, currently Enabled (webconfig AllowInsertDeleteOfTestData=true)
        /// </summary>
        /// <returns>Boolean</returns>  
        public Boolean PutAllData()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AllowInsertDeleteOfTestData"].ToString()))
            {
                Tests.InsertData();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Deletes all data, currently Enabled (webconfig AllowInsertDeleteOfTestData=true)
        /// </summary>
        /// <returns>Boolean</returns>        
        public Boolean DeleteAllData()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AllowInsertDeleteOfTestData"].ToString()))
            {
                Tests.DeleteAllData();
                return true;
            }
            return false;
        }
    }
}
