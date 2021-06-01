using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot
{
    public class AsteroidsResponse
    {

            public string id { get; set; }
            public string magnitude { get; set; }
            public string name { get; set; }

    }

    public class Items
    {
        public List<AsteroidsResponse> asteroidsData { get; set; }
    }


}
