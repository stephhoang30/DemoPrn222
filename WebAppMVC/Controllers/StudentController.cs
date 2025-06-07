using Microsoft.AspNetCore.Mvc;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    public class StudentController : Controller
    {
        [HttpGet]
        public IActionResult Index(Student stu, string sortField, string sortOrder)
        {
            // transfer data from Controller to View

            // c1: use ViewBag
            ViewBag.message = "Add successfully Student:";
            if (!CheckExist(data, stu))
            {
                ViewBag.message = "Delete successfully Student";
            }

            if (stuOld != null)
            {
                ViewBag.message = "Update successfully Student";
                stuOld = null;
            }

            ViewBag.student = stu;

            // sort
            List<Student> sortedList = data;

            if (!string.IsNullOrEmpty(sortField) && !string.IsNullOrEmpty(sortOrder))
            {
                switch (sortField)
                {
                    case "Code":
                        sortedList = (sortOrder == "ASC") ? 
                            data.OrderBy(s => s.Code).ToList() 
                            : data.OrderByDescending(s => s.Code).ToList();
                        break;
                    case "Name":
                        sortedList = (sortOrder == "ASC") ? 
                            data.OrderBy(s => s.Name).ToList() 
                            : data.OrderByDescending(s => s.Name).ToList();
                        break;
                    case "Mark":
                        sortedList = (sortOrder == "ASC") ? 
                            data.OrderBy(s => s.Mark).ToList() 
                            : data.OrderByDescending(s => s.Mark).ToList();
                        break;
                }
            }

            ViewBag.sortField = sortField;
            ViewBag.sortOrder = sortOrder;

            // c2: use ViewData - optional

            // c3: use Model
            return View(sortedList);
        }

        private bool CheckExist(List<Student> data, Student stu)
        {
            var result = data.FirstOrDefault(item => item.Code == stu.Code);
            return result != null;

            //var result = data.Where(item => item.Code == stu.Code).ToList();
            //return result.Count > 0;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Student stu)
        {
            if (ModelState.IsValid) // if transfer student successfully
            {
                data.Add(stu);
                return RedirectToAction("Index", stu);
            }
            return View();
        }

        // Create a list Student
        public static List<Student> data = new List<Student>()
        {
            new Student("SV01", "Nguyen Van A", 10),
            new Student("SV02", "Nguyen Van B", 7),
            new Student("SV03", "Nguyen Van C", 5)
        };

        [HttpGet]
        public IActionResult Delete(string code)
        {
            // find student need to delete
            Student stu = data.FirstOrDefault(x => x.Code == code);
            data.Remove(stu);
            return RedirectToAction("Index", stu);
        }

        [HttpGet] 
        public IActionResult Update(string code)
        {
            // find student need to update
            Student stu = data.FirstOrDefault(x => x.Code == code);
            return View(stu);
        }

        static Student stuOld;
        [HttpPost]
        public IActionResult Update(Student stu)
        {
            // find student need to update

            // save data of updated student to stuOld
            stuOld = data.FirstOrDefault(x => x.Code == stu.Code);

            // assign new value to stuOld
            stuOld.Name = stu.Name;
            stuOld.Mark = stu.Mark;

            // redirect to Index view
            return RedirectToAction("Index", stu);
        }
    }
}
