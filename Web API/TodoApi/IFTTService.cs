using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace TodoApi
{
    public class IFTTService
    {

        private HttpClient _httpClient;

        public IFTTService(HttpClient httpclient)
        {
            _httpClient = httpclient;
        }

        public async void TurnPlugOn()
        {
            string URL = "https://maker.ifttt.com/trigger/Enchufar/with/key/dje2e2aNtKamVk_VWp90t9";

            await _httpClient.GetAsync(URL);

        }

        public async void TurnPlugOff()
        {

            string URL = "https://maker.ifttt.com/trigger/Apagar/with/key/dje2e2aNtKamVk_VWp90t9";

            await _httpClient.GetAsync(URL);

        }
    }
    }
