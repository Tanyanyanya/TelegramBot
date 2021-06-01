using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TelegramBot
{
    public static class ApiSetting
    {
        public static string Base = $"https://localhost:44393/";

        public static HttpClient AstroApiClient { get; set; }

        public static void InitializeClient()
        {
            AstroApiClient = new HttpClient();
            AstroApiClient.BaseAddress = new Uri("https://localhost:44393/");
        }

    }
}
