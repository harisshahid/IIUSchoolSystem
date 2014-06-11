using System;

namespace IIUSchoolSystem.Models
{
    public class ErrorLogModel
    {
        public long Id { get; set; }
        public string ShortMessage { get; set; }
        public string FullMessage { get; set; }
        public string IpAddress { get; set; }
        public long UserId { get; set; }
        public string PageUrl { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}