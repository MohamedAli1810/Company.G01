using AutoMapper;
using Company.G01.BLL.Interfaces;
using Company.G01.DAL.Models;
using Company.G01.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;


namespace Company.G01.PL.Controllers
{
    // MVC Controller
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRespository;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentRepository departmentRespository , IMapper mapper)
        {
            _departmentRespository = departmentRespository;
            _mapper = mapper;
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
                //var department = new Department() 
                //{
                //    Code = model.Code,
                //    Name = model.Name,
                //    CreateAt = model.CreateAt
                //};
                var department = _mapper.Map<Department>(model);
                var count = _departmentRespository.Add(department);
                if (count > 0) 
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")

        {
            var department = _departmentRespository.Get(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            if (department is null) return NotFound(new { StatusCode = 404 ,  Message = $"Department with Id :{id} is not found" });
            return View(viewName,department);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var department = _departmentRespository.Get(id.Value);
            if (department == null)
            {
                return NotFound();
            }
            if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department with Id :{id} is not found" });
            var departmentDto = new CreateDepartmentDto() 
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt
            };
            return View(departmentDto);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //if (id != department.Id) return BadRequest(); //400
        //        //var department = new Department()
        //        //{
        //        //    Id = id,
        //        //    Code = model.Code,
        //        //    Name = model.Name,
        //        //    CreateAt = model.CreateAt
        //        //};
        //        var department = _mapper.Map<Department>(model);
        //        var count = _departmentRespository.Update(department);

        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }

        //    return View(model);

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingDepartment = _departmentRespository.Get(id);
            if (existingDepartment == null)
            {
                return NotFound(new { StatusCode = 404, Message = $"Department with Id {id} not found" });
            }

            // Map only the fields that need updating
            _mapper.Map(model, existingDepartment);

            var count = _departmentRespository.Update(existingDepartment);
            if (count > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Failed to update department.");
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto model )
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department() 
        //        {
        //           Id = id,
        //           Name = model.Name,
        //           Code = model.Code,
        //           CreateAt = model.CreateAt,
        //        };
        //        var count = _departmentRespository.Update(department);

        //        if (count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }

        //    return View(model);

        //}

        [HttpGet]
        public IActionResult? Delete(int? id)
        {
            //var department = _departmentRespository.Get(id);
            //if (department == null)
            //{
            //    return NotFound();
            //}
            //if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department with Id :{id} is not found" });
            return Details(id , "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int? id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest(); //400     
                var count = _departmentRespository.Delete(department);

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(department);

        }
    }
}
