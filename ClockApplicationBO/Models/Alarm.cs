using System;
using System.Collections.Generic;

namespace ClockApplicationBO.Models
{
    public partial class Alarm
    {
        public int AlarmId { get; set; }
        public string AlarmName { get; set; } = null!;
        public DateTime AlarmTime { get; set; }
        public bool Enabled { get; set; }
    }
}
