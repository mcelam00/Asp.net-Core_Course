using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Api.Models;

namespace WebAPI.Api.Controllers
{
    [Route("api/[controller]")] //este es el enrutamiento común a todas las manejadoras, sustituyendo [controller] por el nombre de la clase menos el sufijo Controller, es decir TodoItems -> api/todoitems porque es no sensible a mayúsculas. Si se especifica una ruta concreta para un método se anexaría detrás
    [ApiController]//Se marca la clase con éste atributo que indica que el controller responde a las web API requests habilitando comportamientos específicos
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context; //también se inyecta el contexto de la base de datos en el controller porque se usa en cada uno de los metodos CRUD (Create, Read, Update, Delete) en el controller

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }



        //READ
        //Tenemos dos manejadoras para las peticiones GET, cada una para un enrutamiento distinto
        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync(); //de la base de datos de todoItems conviertelos a una lista asincronamente y la retorna (la lista)

        }
        //Al usar una in-memory db cuando se para la app y se rearranca se ha perdido todo lo que estaba almacenado de antes y hay que mandar post de nuevo. Es como la RAM con el pc, lo almacena temporal hasta que la paras.

        // GET: api/TodoItems/5
        [HttpGet("{id}")] //esto representa una incognita, una variable que se rellena con lo que quiera que venga en la URL detrás de api/todoitems y después se pasa al método la variable
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id); //de la base de datos de todoItems, busca asíncronamente el objeto todoitem que tiene (de clave primaria) la id que llega como parámetro en el final de la URL

            if (todoItem == null) //si se encontró la entidad (el objeto) con ese id lo retorna, de lo contrario retorna null
            {
                return NotFound(); //La llamada a este metodo retorna un 404 not found (esta es la idea de programar en backend a mas alto nivel de abstraccion que con el XMLHttpRequest)
            }

            return todoItem;
            //ASP.NET Core automatically serializes the object to JSON and writes the JSON into the body of the response message. The response code for this return type is 200 OK, assuming there are no unhandled exceptions. Unhandled exceptions are translated into 5xx errors.

        }




        //UPDATE
        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem) //recibe el id del objeto a actualizar en la url y el objeto nuevo COMPLETO con el que actualizamos al antiguo en el cuerpo del mensaje.
        {
            if (id != todoItem.Id) //si la id del objeto que mandamos en el cuerpo y la id que indicamos en la url no son la misma, entonces falla
            {
                return BadRequest(); //se retorna un 400 
            }

            _context.Entry(todoItem).State = EntityState.Modified; //""se marca la página como sucia""
            //obtenemos una entrada a la entidad, la actualizamos y marcamos su estado a modificado
            try
            {
                await _context.SaveChangesAsync(); //""Se escribe la página"" Se detectan los cambios en entidades y se guardan a la bd asincronamente.
            }
            catch (DbUpdateConcurrencyException) //si se ha producido algun cambio en la bd conforme estabamos modificando en memoria a la vez y al volver a grabar, mas entidades no coinciden ocurre esta exception
            {
                if (!TodoItemExists(id)) //durante ese tiempo han borrado en la base de datos la entidad que estoy actualizando en memoria o bien si trato de actualizar y grabar una entidad que no existe previamente (no se ha creado con un post antes)
                {
                    return NotFound(); //retornamos 404
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); //La respuesta correcta por defecto es el 204 
        }





        //CREATE
        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem) //async and await son para asincronía. Me llega como parámetrp un objeto de la clase TodoItem que es el JSON que mandamos en el cuerpo del request
        {
            _context.TodoItems.Add(todoItem);   //El contexto de la bd de TodoItems añadimos el todoItem que llega
            await _context.SaveChangesAsync();  //Damos el Save para que se haga efectivo el cambio pero con un await para que no bloquee la ejecución

            /*return CreatedAtAction("PutTodoItem", new { id = todoItem.Id }, todoItem);*/
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);

            //CreatedAtAction(String, Object, Object) está sobrecargado y toma como parámetros:
            //actionName: El nombre de la accion a usar para generar la URL. Se usa el nameof keyword para no hardcodearlo en la llamada. Es la ruta que vamos a anexar al final para componer la URI, Si ponemos PutTodoItem se usará la ruta definida en esta misma clase para el metodo put con ese mismo nombre de metodo y si ponemos el del get se usará la ruta que corresponde a la manejadora de las peticiones get
            //routeValues: Los datos de enrutamiento a usar para generar la URL (el id final que se pone para que sea URI)
            //value: El contenido a formatear en el entity body (el objeto resultado)

            //Retorna un objeto de la clase CreatedAtActionResult (https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.createdatactionresult?view=aspnetcore-5.0)
            //Retorna también un 201 de estado si es exitosos (el estandar para el POST cuando crea un nuevo recurso en el servidor)
            //Un header de localizacion en la response, siendo el Identificador de Recurso Uniforme (URI) del nuevo To-Do-Item creado.
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id); //de la base de datos de todoItems, busca asíncronamente el objeto todoitem que tiene (de clave primaria) la id que llega como parámetro en el final de la URL
            if (todoItem == null) //si se encontró la entidad (el objeto) con ese id lo BORRA, de lo contrario retorna null
            {
                return NotFound();//404 not found
            }

            _context.TodoItems.Remove(todoItem); //como sé que está, la marco como entidad en estado para borrar
            await _context.SaveChangesAsync(); //actualizo a que se haga efectivo el borrado en la base de datos

            return NoContent(); //204 no content
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id); //valida si algun todoitem cumple una condición (en otras palabras, si existe un e tal que su e.id sea el id que viene como parámetro)
        }
    }
}
