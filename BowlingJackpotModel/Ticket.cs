//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BowlingJackpotModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ticket
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public System.DateTime Purchase_Date { get; set; }
        public int Ticket_Amount_Id { get; set; }
    
        public virtual User User { get; set; }
        public virtual TicketAmount TicketAmount { get; set; }
    }
}
