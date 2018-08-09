using ChatProject.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatProject.BL.Models
{
    public class Message : IEntity<long>
    {
        public long Id { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
        public string TextMessage { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        //public MessageStatus MessageStatus { get; set; } = MessageStatus.New;
    }
}
