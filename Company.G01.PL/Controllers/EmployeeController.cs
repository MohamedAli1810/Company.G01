using AutoMapper;
using Company.G01.BLL.Interfaces;
using Company.G01.DAL.Models;
using Company.G01.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Company.G01.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper 
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees = _employeeRepository.GetAll();
            }
            else 
            {
                 employees = _employeeRepository.GetByName(SearchInput);
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
            var departments = _departmentRepository.GetAll();
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
                    var employee = _mapper.Map<Employee>(model);
                    var count = _employeeRepository.Add(employee);
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
            var employee = _employeeRepository.Get(id.Value);
            var departments = _departmentRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404 , Message = $"Employee with Id : {id} is not found " });
            return View(viewName, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id) 
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee with Id : {id} is not found " });
            //var employeeDto = new CreateEmployeeDto()
            //{
            //    Name = employee.Name,
            //    Age = employee.Age,
            //    Address = employee.Address,
            //    CreateAt = employee.CreateAt,
            //    HiringDate = employee.HiringDate,
            //    Email = employee.Email,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,
            //    Phone = employee.Phone,
            //    Salary = employee.Salary,
            //    Department = employee.Department
            //};
            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id , CreateEmployeeDto model) 
        {
            if (ModelState.IsValid) 
            {
                //if (id != model.Id) return BadRequest();
                //var employee = new Employee()
                //{
                //    Id = id,
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
                //    DepartmentId = model.DepartmentId
                //};
                var employee = _mapper.Map<Employee>(model); ;
                var count = _employeeRepository.Update(employee);
                if (count > 0) 
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int? id, Employee model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest();
                var count = _employeeRepository.Delete(model);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
