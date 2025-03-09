using Company.G01.BLL.Respositories;
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
    }
}
