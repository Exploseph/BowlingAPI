using BowlingJackpotModel;
using BowlingJackpotModel.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingJackpotModel
{
    public class Tests
    {
        #region Public Methods
        public static void InsertData()
        {
            DeleteAllData();

            using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
            {
                TicketAmount ticketAmount = rep.AddTicketAmount(new TicketAmount() { Ticket_Amount = 1 });

                foreach (var u in GetUsers())
                {
                    var user = rep.AddUser(u);
                    rep.AddTicket(new Ticket { Purchase_Date = DateTime.UtcNow, Ticket_Amount_Id = ticketAmount.Id, User_Id = user.Id });
                    rep.AddTicket(new Ticket { Purchase_Date = DateTime.UtcNow.AddDays(-7), Ticket_Amount_Id = ticketAmount.Id, User_Id = user.Id });
                    rep.AddTicket(new Ticket { Purchase_Date = DateTime.UtcNow.AddDays(-14), Ticket_Amount_Id = ticketAmount.Id, User_Id = user.Id });
                }

                rep.AddWeeklyPlay(new WeeklyPlay { Play_Date = DateTime.UtcNow.AddDays(-14), User_Id = GetUser(6).Id, Pins = 8, Payout_Amount = 1 });
                rep.AddWeeklyPlay(new WeeklyPlay { Play_Date = DateTime.UtcNow.AddDays(-7), User_Id = GetUser(2).Id, Pins = 8, Payout_Amount = (Decimal)1.9 });
            }
        }
        public static void DeleteAllData()
        {
            using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
            {
                rep.DeleteAllData();
            }
        }
        #endregion

        #region Private Methods
        private static TicketAmount currentTicketAmount;
        private static TicketAmount CurrentTicketAmount
        {
            get
            {
                if (currentTicketAmount == null)
                {
                    using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
                    {
                        currentTicketAmount = rep.GetCurrentTicketAmount();
                    }
                }
                return currentTicketAmount;

            }
        }
        private static User GetUser(int index)
        {
            using (BowlingJackpotRepository rep = new BowlingJackpotRepository())
            {
                return rep.GetAllUsers()[index];
            }
        }
        private static List<User> GetUsers()
        {
            return new List<User>()
            {
                new User { Id = 1, Name = "Joe"},
                new User { Id = 2, Name = "Tim"},
                new User { Id = 3, Name = "Mike"},
                new User { Id = 4, Name = "Ralph"},
                new User { Id = 5, Name = "Alec"},
                new User { Id = 6, Name = "Sandy"},
                new User { Id = 7, Name = "Cindy"},
                new User { Id = 8, Name = "Amanda"},
                new User { Id = 9, Name = "Kathy"},
                new User { Id = 10, Name = "Regina"},
            };
        }
        #endregion
    }
}
