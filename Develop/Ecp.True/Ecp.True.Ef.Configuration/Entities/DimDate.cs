using System;
using System.Collections.Generic;

namespace Ecp.True.Ef.Configuration.Entities
{
    public partial class DimDate
    {
        public int DateKey { get; set; }
        public DateTime Date { get; set; }
        public byte Day { get; set; }
        public string DaySuffix { get; set; }
        public byte Weekday { get; set; }
        public string WeekDayName { get; set; }
        public string WeekDayNameShort { get; set; }
        public string WeekDayNameFirstLetter { get; set; }
        public byte DowinMonth { get; set; }
        public short DayOfYear { get; set; }
        public byte WeekOfMonth { get; set; }
        public byte WeekOfYear { get; set; }
        public byte Month { get; set; }
        public string MonthName { get; set; }
        public string MonthNameShort { get; set; }
        public string MonthNameFirstLetter { get; set; }
        public byte Quarter { get; set; }
        public string QuarterName { get; set; }
        public int Year { get; set; }
        public string Mmyyyy { get; set; }
        public string MonthYear { get; set; }
        public bool IsWeekend { get; set; }
        public bool IsHoliday { get; set; }
    }
}
