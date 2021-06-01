using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Models
{
    public class TypeOfEclipseResponse
    {
        public string Name { get; set; }
        public string Info { get; set; }
        public string Url { get; set; }
    }
    public class EclipseItems
    {
        public List<TypeOfEclipseResponse> lunarEclipse { get; set; }
    }
}
