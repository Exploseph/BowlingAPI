using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace BowlingJackpotModel
{
    public class Jackpot
    {
        public Decimal JackpotBalance { get; set; }
    }
    
    [MetadataType(typeof(UserRequestMetaData))]
    public partial class User
    {        
    }

    public class UserRequestMetaData
    {        
        [Required]
        public string Name { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Ticket> Tickets { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public virtual ICollection<WeeklyPlay> WeeklyPlays { get; set; }
    }


    [MetadataType(typeof(TicketRequestMetaData))]
    public partial class Ticket
    {
    }
    public class TicketRequestMetaData
    {
        [Required]
        public int User_Id { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public virtual User User { get; set; }
        [XmlIgnore]
        [IgnoreDataMember]
        public virtual TicketAmount TicketAmount { get; set; }
    }

    [MetadataType(typeof(TicketAmountRequestMetaData))]
    public partial class TicketAmount
    {
    }

    public class TicketAmountRequestMetaData
    {
        [Required]
        [Range(0.0, 100)]
        public decimal Ticket_Amount { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }

    [MetadataType(typeof(WeeklyPlayRequestMetaData))]
    public partial class WeeklyPlay
    {
    }
    public class WeeklyPlayRequestMetaData
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [Range(0, 10)]
        public int Pins { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public virtual User User { get; set; }
    }
}
