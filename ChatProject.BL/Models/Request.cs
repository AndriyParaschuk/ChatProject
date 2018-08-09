using ChatProject.BL.Interfaces;
using ChatProject.BL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChatProject.BL
{
    public class Request : IEntity<long>
    {
        public long Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string FromId { get; set; }
        public string ToId { get; set; }
        public virtual User From { get; set; }
        public virtual User To { get; set; }

        [EnumDataType(typeof(RequestStatus))]
        public RequestStatus Status { get; set; } = RequestStatus.New;
    }
}
