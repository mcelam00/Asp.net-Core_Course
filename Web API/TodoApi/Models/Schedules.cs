using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class Schedules
    {
        public long Id { get; set; }
        public Schedule[] schedules { get; set; }
}
}
