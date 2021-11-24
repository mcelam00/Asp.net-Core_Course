using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Scripting.Hosting;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly REEService _reeservice;
        private readonly IFTTService _IFTTservice;

        public TodoItemsController(TodoContext context, REEService myserviceREE, IFTTService myserviceIFTTT) //inyecto por dependencia el serivicio Http REE
        {
            _context = context;
            this._reeservice = myserviceREE; //Se lo mappeo al atributo de la clase para poder usarlo y hacer llamadas desde los endpoints
            this._IFTTservice = myserviceIFTTT;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }



        [HttpGet("REE")]
        public async void GetREEData()
        {
            Console.WriteLine("DENTRO");
            string resultado = await _reeservice.GetREEData("2021-11-24");
            Console.WriteLine(resultado);
        }


        [HttpGet("IFTTT/On")]
        public void GetTurnOn()
        {
            _IFTTservice.TurnPlugOn();
        }


        [HttpGet("IFTTT/Off")]
        public void GetTurnOff()
        {
            _IFTTservice.TurnPlugOff();
        }


        [HttpGet("PythonMonitor")]
        public void GetConsumption()
        {
            var path = @"C:\Users\mario\Desktop\API\examples\electricity.py";
            var path1 = @"C:\Users\mario\Desktop\Hello.py";
            ScriptRuntime py = Python.CreateRuntime();
            dynamic programaExterno = py.UseFile(path1);

           



        }

    }
}
