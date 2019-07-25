using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CountryClubMVC.Models
{
    public class Message
    {
        [Key]
        public int Message_ID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Time { get; set; }
        public string TimeUpdated { get; set; }
        public string TimeDeleted { get; set; }
        public MessageStaus Status { get; set; } //Read or Unread

    }

    public enum MessageStaus
    {
        Read,
        Unread
    }
}