using Microsoft.AspNetCore.Mvc;
using WebAppMVC_EF.Models;

namespace WebAppMVC_EF.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index(string sortBy, string sortOrder)
        {
            var customers = MySaleDbContext.INS.Customers.AsQueryable();

            if (sortBy == "Name")
            {
                if (sortOrder == "DESC")
                {
                    customers = customers.OrderByDescending(c => c.CustomerName);
                }
                else
                {
                    customers = customers.OrderBy(c => c.CustomerName);
                }
            }

            ViewBag.lsCustomers = customers.ToList();
            return View();
        }
    }
}
