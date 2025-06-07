using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp2
{
    public class Request
    {
        public string Action { get; set; } // Add, Update, Delete
        public string ObjectName { get; set; }
        public object? Data { get; set; }
    }
}
