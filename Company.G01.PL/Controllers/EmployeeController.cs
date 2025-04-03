using System.Threading.Tasks;
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

        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                 employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else 
            {
                 employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
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
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
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
                    await _unitOfWork.EmployeeRepository.AddAsync(employee);
                    var count = await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Details(int? id, string viewName = "Details") 
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            
            if (employee is null) return NotFound(new { StatusCode = 404 , Message = $"Employee with Id : {id} is not found " });

            var dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id) 
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee with Id : {id} is not found " });
            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model , string viewName = "Edit")
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
                var count = await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Delete(employee);
                var count = await _unitOfWork.CompleteAsync();
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
