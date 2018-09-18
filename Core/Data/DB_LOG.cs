
using System;

namespace Core.Data
{
    public class DB_LOG
    {
        public int Id { get; set; }
        public DateTime LOG_DATE_TIME { get; set; }
        public string LOG_THREAD { get; set; }
        public string LOG_LEVEL { get; set; }
        public string LOG_USER { get; set; }
        public string LOG_MESSAGE { get; set; }
        public string TYPE { get; set; }
    }
}