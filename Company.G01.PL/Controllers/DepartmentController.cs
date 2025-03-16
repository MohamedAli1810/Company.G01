﻿using Company.G01.BLL.Respositories;
using Company.G01.DAL.Models;
using Company.G01.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
=======
>>>>>>> 9c76c71d506f51e1d04347519ad3d8b412a0aca4

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
<<<<<<< HEAD
        public IActionResult Details(int? id, string viewName = "Details")
=======
        public IActionResult Details(int id)
>>>>>>> 9c76c71d506f51e1d04347519ad3d8b412a0aca4
        {
            var department = _departmentRespository.Get(id);
            if (department == null)
            {
                return NotFound();
            }
<<<<<<< HEAD
            if (department is null) return NotFound(new { StatusCode = 404 ,  Message = $"Department with Id :{id} is not found" });
            return View(viewName,department);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //var department = _departmentRespository.Get(id);
            //if (department == null)
            //{
            //    return NotFound();
            //}
            //if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department with Id :{id} is not found" });
            return Details(id , "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest(); //400     
                var count = _departmentRespository.Update(department);

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(department);

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
        public IActionResult Delete(int? id)
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
        public IActionResult Delete([FromRoute] int id, Department department)
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

=======
            return View(department);
>>>>>>> 9c76c71d506f51e1d04347519ad3d8b412a0aca4
        }
    }
}
