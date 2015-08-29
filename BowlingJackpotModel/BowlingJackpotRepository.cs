using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BowlingJackpotModel;
using System.Configuration;

namespace BowlingJackpotModel.DAL
{
    public class BowlingJackpotRepository : IDisposable
    {
        #region Private Variables
        private BowlingJackpotEntities context;
        #endregion

        #region Private Properties
        private DateTime weekStartDate
        {
            get
            {
                var startDate = DateTime.UtcNow.StartOfWeek(DayOfWeek.Monday);
                return startDate.Add(GetLeagueStartTimeSpan());
            }
        }

        private DateTime weekEndDate
        {
            get
            {
                return weekStartDate.AddDays(7);
            }
        }
        #endregion

        #region Constructor
        public BowlingJackpotRepository()
        {
            this.context = new BowlingJackpotEntities();
            context.Configuration.ProxyCreationEnabled = false;
        }
        #endregion

        #region Public Methods
        public List<User> GetAllUsers()
        {
            return context.Users.ToList();
        }

        public User GetUser(int id)
        {
            return context.Users.Where(i => i.Id == id).FirstOrDefault();
        }

        public User AddUser(User user)
        {
            context.Users.Add(user);
            Save();
            return user;
        }

        public List<Ticket> GetAllTickets()
        {
            return context.Tickets.ToList();
        }

        public Ticket GetTicket(int id)
        {
            return context.Tickets.Where(i => i.Id == id).FirstOrDefault();
        }

        public Ticket AddTicket(Ticket ticket)
        {
            context.Tickets.Add(ticket);
            Save();
            return ticket;
        }

        public List<TicketAmount> GetTicketAmounts()
        {
            return context.TicketAmounts.ToList();
        }

        public TicketAmount GetCurrentTicketAmount()
        {
            return context.TicketAmounts.OrderByDescending(i => i.Id).First();
        }

        public TicketAmount AddTicketAmount(TicketAmount ticketAmount)
        {
            context.TicketAmounts.Add(ticketAmount);
            Save();
            return ticketAmount;
        }

        public WeeklyPlay AddWeeklyPlay(WeeklyPlay wp)
        {
            context.WeeklyPlays.Add(wp);
            Save();
            return wp;
        }

        public List<WeeklyPlay> GetAllWeeklyPlays()
        {
            return context.WeeklyPlays.ToList();
        }

        public WeeklyPlay PickWeeklyPlayer()
        {
            var currentWeeksPlay = GetCurrentWeeklyPlay();
            if (currentWeeksPlay == null)
            {
                var currentWeeklyTickets = context.Tickets.Where(i => i.Purchase_Date > weekStartDate && i.Purchase_Date < weekEndDate);

                if (currentWeeklyTickets.Any())
                {
                    var randomTicket = GetRandomTicket(currentWeeklyTickets);

                    var weeklyPlay = new WeeklyPlay()
                    {
                        Play_Date = DateTime.UtcNow,
                        User_Id = randomTicket.User_Id,
                        Pins = 0
                    };

                    context.WeeklyPlays.Add(weeklyPlay);
                    Save();
                    return weeklyPlay;
                }
                return null;
            }
            else
            {
                return currentWeeksPlay;
            }
        }

        public WeeklyPlay UpdateWeeklyPlay(WeeklyPlay wp)
        {
            var playToUpdate = context.WeeklyPlays.Where(i => i.Id == wp.Id).FirstOrDefault();
            playToUpdate.Pins = wp.Pins;

            if (playToUpdate != null)
            {
                //Strike Payout Full Amount
                if (playToUpdate.Pins == 10)
                    playToUpdate.Payout_Amount = GetJackpotSinceLastWin(playToUpdate).JackpotBalance;
                //Pay 10%
                else
                    playToUpdate.Payout_Amount = GetJackpotSinceLastWin(playToUpdate).JackpotBalance * (Decimal)0.1;

                Save();

                //Update Payouts After playToUpdate
                var currentWeeklyPlay = GetCurrentWeeklyPlay();
                var lastPlay = context.WeeklyPlays.OrderByDescending(i => i.Play_Date).FirstOrDefault();
                if ((currentWeeklyPlay == null && playToUpdate.Id != lastPlay.Id) || (currentWeeklyPlay != null && playToUpdate.Id != currentWeeklyPlay.Id))
                {
                    var payoutsAfterPlay = context.WeeklyPlays.Where(i => i.Play_Date > playToUpdate.Play_Date).OrderBy(i => i.Play_Date);
                    foreach (var p in payoutsAfterPlay)
                    {
                        if (p.Payout_Amount != null)
                        {
                            //Strike Payout Full Amount
                            if (p.Pins.Value == 10)
                                p.Payout_Amount = GetJackpotSinceLastWin(p).JackpotBalance;
                            //Pay 10%
                            else
                                p.Payout_Amount = GetJackpotSinceLastWin(p).JackpotBalance * (Decimal)0.1;
                        }
                    }
                }
            }
            Save();
            return playToUpdate;
        }

        public WeeklyPlay GetCurrentWeeklyPlay()
        {
            return context.WeeklyPlays.Where(i => i.Play_Date > weekStartDate && i.Play_Date < weekEndDate).FirstOrDefault();
        }

        public Jackpot GetJackpotSinceLastWin(WeeklyPlay wp)
        {
            var jackpot = new Jackpot();
            WeeklyPlay lastWin = GetLastWeeklyPlayWin(wp);
            if (lastWin == null)
            {
                var lastStartWeek = GetLastStartWeek(wp.Play_Date);
                jackpot.JackpotBalance = (Decimal)context.Tickets.Where(i => i.Purchase_Date < lastStartWeek).Sum(i => i.TicketAmount.Ticket_Amount) - GetPreviousNonWinningPayouts(wp);
            }
            else
            {
                DateTime lastStartWeek = GetLastStartWeek(lastWin.Play_Date);
                DateTime currentPlayWeek = GetLastStartWeek(wp.Play_Date);
                var de = (Decimal)context.Tickets.Where(i => i.Purchase_Date > lastStartWeek && i.Purchase_Date < currentPlayWeek).Sum(i => i.TicketAmount.Ticket_Amount);
                jackpot.JackpotBalance = de - GetPreviousNonWinningPayouts(wp);
            }

            return jackpot;
        }

        public Jackpot GetJackpotBalance()
        {
            var jackpot = new Jackpot();
            var ticketsSold = context.Tickets.Sum(i => i.TicketAmount.Ticket_Amount);
            var payouts = context.WeeklyPlays.Sum(i => i.Payout_Amount);
            if (payouts == null)
                payouts = 0;
            jackpot.JackpotBalance = (Decimal)(ticketsSold - payouts);
            return jackpot;
        }

        public void InsertTestData(List<Ticket> tickets, List<WeeklyPlay> weeklyPlays)
        {
            context.Tickets.AddRange(tickets);
            context.WeeklyPlays.AddRange(weeklyPlays);
            Save();
        }

        public void DeleteAllData()
        {
            context.Users.RemoveRange(context.Users);
            context.Tickets.RemoveRange(context.Tickets);
            context.TicketAmounts.RemoveRange(context.TicketAmounts);
            context.WeeklyPlays.RemoveRange(context.WeeklyPlays);
            Save();
        }
        #endregion

        #region Private Methods
        private WeeklyPlay GetLastWeeklyPlayWin(WeeklyPlay wp)
        {
            return context.WeeklyPlays.Where(i => i.Pins == 10 && i.Id != wp.Id && i.Play_Date < wp.Play_Date).OrderByDescending(j => j.Play_Date).FirstOrDefault();
        }

        private Decimal GetPreviousNonWinningPayouts(WeeklyPlay wp)
        {
            var lastWin = GetLastWeeklyPlayWin(wp);
            Decimal? payout;
            if (lastWin == null)
            {
                payout = context.WeeklyPlays.Where(i => i.Id != wp.Id && i.Play_Date < wp.Play_Date).Sum(i => i.Payout_Amount);
            }
            else
            {
                var lastWinStartDate = GetLastStartWeek(lastWin.Play_Date);
                var currentPlayStartDate = GetLastStartWeek(wp.Play_Date);
                payout = context.WeeklyPlays.Where(i => i.Id != wp.Id && i.Play_Date > lastWinStartDate && i.Play_Date < currentPlayStartDate).Sum(i => i.Payout_Amount);
            }

            if (payout.HasValue)
                return payout.Value;
            return (Decimal)0;
        }

        private void Save()
        {
            context.SaveChanges();
        }

        private static Ticket GetRandomTicket(IQueryable<Ticket> currentWeeklyTickets)
        {
            var r = new Random();
            int index = r.Next(currentWeeklyTickets.Count());
            var list = currentWeeklyTickets.ToList();
            var randomTicket = list.ElementAt(index);
            return randomTicket;
        }

        private TimeSpan GetLeagueStartTimeSpan()
        {
            return new TimeSpan(12, 0, 0);
        }

        private DateTime GetLastStartWeek(DateTime lastStart)
        {
            return lastStart.StartOfWeek(DayOfWeek.Monday).AddDays(7).Add(GetLeagueStartTimeSpan());
        }
        #endregion

        #region Dispose

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
