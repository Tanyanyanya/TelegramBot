using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Models
{
    public class EclipseInCityResponse
    {
        public string City { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string Middle_ecl { get; set; }
        public string Par_ecl_beg { get; set; }
        public string Par_ecl_end { get; set; }
        public string Pen_ecl_beg { get; set; }
        public string Pen_ecl_end { get; set; }
        public string Tot_ecl_beg { get; set; }
        public string Tot_ecl_end { get; set; }
    }
}
