using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly REEService _reeservice;


        public ValuesController(REEService myservice)
        {
            this._reeservice = myservice;
        }


        [HttpGet]
        public async void Get()
        {
            string resultado = await _reeservice.GetREEData("2021-11-24");
            Console.WriteLine(resultado);
        }


        













        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        










        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

       








        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        










        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
