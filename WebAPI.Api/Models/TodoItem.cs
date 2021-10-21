using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Api.Models
{
    public class TodoItem
    {
        //Esto es una clase que representa los datos que la aplicacion va a tratar. En éste caso será un TodoItem (Una tarea pendiente) porque la app trata todoItems (tareas pendientes).

        public long Id { get; set; } 
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        //Así pues, una tarea pendiente va a tener un Id de tipo long, un nombre de tipo string y un boolean que representa si está completada o no.

    }
}
