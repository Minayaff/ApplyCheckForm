using System;

namespace ApplyCheckESosial.Models
{
    public class ESosialServiceData
    {
        public string Number { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string ReceptMethod { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsClosed { get; set; }
        public string Closed { get { return IsClosed ? "Bağlıdır" : "Açıqdır"; } }
        public string Status { get; set; }
        public string ExecutorDepartment { get; set; }
        public string ExecutorFullName { get; set; }

    }
}
