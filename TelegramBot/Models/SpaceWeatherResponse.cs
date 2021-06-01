using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Models
{
    public class SpaceWeatherResponse
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public string IssueTime { get; set; }
        public string MessageBody { get; set; }
        public string FullName { get; set; }
    }
    public class SpaceWeather
    {
        public List<SpaceWeatherResponse> SpaceWeatherData { get; set; }
    }

}
