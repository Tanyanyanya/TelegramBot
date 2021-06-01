using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Models
{
    public class MarsExplorationModel
    {
        public string Date { get; set; }
        public string Description { get; set; }
    }
    public class Items
    {
        public List<MarsExplorationModel> marsData { get; set; }
    }
}
