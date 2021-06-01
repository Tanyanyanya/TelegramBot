using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Models
{
    public class PicOfDayModel
    {
        public string Date { get; set; }
        public string Explanation { get; set; }
        public string Hdurl { get; set; }
        public string Title { get; set; }
    }
}
