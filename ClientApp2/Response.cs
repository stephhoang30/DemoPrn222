using ServerApp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp2
{
    public class Response
    {
        public List<string> listCateNames { get; set; }
        public List<Product> listProducts { get; set; }

    }
}
