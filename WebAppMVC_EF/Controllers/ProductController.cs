using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebAppMVC_EF.Models;

namespace WebAppMVC_EF.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index(int? filCate)
        {
            ViewBag.categories = MySaleDbContext.INS.Categories.ToList();

            var lsProducts = MySaleDbContext.INS.Products.ToList();

            if (filCate != null)
            {
                lsProducts = MySaleDbContext.INS.Products
                    .Where(x => x.CategoryId == filCate)
                    .ToList();
            }

            ViewBag.filCate = filCate;
            ViewBag.lsProducts = lsProducts;

            return View();
        }
    }
}
