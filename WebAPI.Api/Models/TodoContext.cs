using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace WebAPI.Api.Models
{
    public class TodoContext : DbContext
    {

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {

        }


        public DbSet<TodoItem> TodoItems { get; set; }


        //Ahora es necesario registrar éste servicio DB context en el contenedor de inyección de dependencias (a.k.a. Startup.cs)

    }
}
