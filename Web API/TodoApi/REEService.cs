using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace TodoApi
{
       public class REEService 
    {
        private  HttpClient _httpClient;

        public REEService(HttpClient httpclient)
        {
            _httpClient = httpclient;
        }

        public async Task<string> GetREEData(string fecha)
        {
            string URL = "https://api.esios.ree.es/archives/70/download_json?locale=es&date="+fecha;

            var request = new HttpRequestMessage(HttpMethod.Get, URL); //creo la request con el método que usaré que es GET y la URL
            //request.Headers.Add("Accept", "application/json;application/vnd.esios-api-v1+json");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Host", "api.esios.ree.es");
            request.Headers.Add("Authorization", "Token token=16388be3b19d1f03112ab963f5b0660f07abfe1d616859d5e8f032074b25093d");
            request.Headers.Add("Cookie", "");



            var response = await _httpClient.SendAsync(request);

            //var response = await _httpClient.GetAsync(URL); otra opción


            return await response.Content.ReadAsStringAsync();



        }






         
    }
}
