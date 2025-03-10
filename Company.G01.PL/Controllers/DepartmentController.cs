using Company.G01.BLL.Respositories;
using Company.G01.DAL.Models;
using Company.G01.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.G01.PL.Controllers
{
    // MVC Controller
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRespository _departmentRespository;

        public DepartmentController(IDepartmentRespository departmentRespository)
        {
            _departmentRespository = departmentRespository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentRespository.GetAll();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) 
            {
                var department = new Department() 
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt
                };
                var count = _departmentRespository.Add(department);
                if (count > 0) 
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var department = _departmentRespository.Get(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
    }
}
