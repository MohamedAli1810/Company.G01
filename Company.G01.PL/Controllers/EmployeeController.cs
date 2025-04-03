using AutoMapper;
using Company.G01.BLL.Interfaces;
using Company.G01.DAL.Models;
using Company.G01.PL.Dtos;
using Company.G01.PL.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Company.G01.PL.Controllers
{
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper 
            )
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            else 
            {
                 employees = _unitOfWork.EmployeeRepository.GetByName(SearchInput);
            }

                // Dictionary
                // 1. ViewData
                //ViewData["Message"] = "Hello From ViewData";
                // 2. ViewBag
                //ViewBag.Message = "Hello From ViewBag";
                // 3. TempData


                return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    //var employee = new Employee()
                    //{
                    //    Name = model.Name,
                    //    Age = model.Age,
                    //    Address = model.Address,
                    //    CreateAt = model.CreateAt,
                    //    HiringDate = model.HiringDate,
                    //    Email = model.Email,
                    //    IsActive = model.IsActive,
                    //    IsDeleted = model.IsDeleted,
                    //    Phone = model.Phone,
                    //    Salary = model.Salary,
                    //    DepartmentId = model.DepartmentId,
                    //};

                    if (model.Image is not null) 
                    {
                       model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                    }

                    var employee = _mapper.Map<Employee>(model);
                    _unitOfWork.EmployeeRepository.Add(employee);
                    var count = _unitOfWork.Complete();
                    if (count > 0)
                    {
                        TempData["Message"] = "Employee is created";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("",ex.Message);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details") 
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            
            if (employee is null) return NotFound(new { StatusCode = 404 , Message = $"Employee with Id : {id} is not found " });

            var dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(viewName, dto);
        }

        [HttpGet]
        public IActionResult Edit(int? id) 
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee with Id : {id} is not found " });
            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model , string viewName = "Edit")
        {
            if (ModelState.IsValid)
            {
                if (model.ImageName is not null && model.Image is not null) 
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }

                if(model.Image is not null) 
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                var employee = _mapper.Map<Employee>(model); 
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Update(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(viewName,model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    var existingEmployee = _employeeRepository.Get(id);
        //    if (existingEmployee == null)
        //    {
        //        return NotFound(new { StatusCode = 404, Message = $"Employee with Id {id} not found" });
        //    }

        //    // Map only the fields that need updating
        //    _mapper.Map(model, existingEmployee);

        //    var count = _employeeRepository.Update(existingEmployee);
        //    if (count > 0)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ModelState.AddModelError("", "Failed to update employee.");
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Delete(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    if (model.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "images");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
