using System.Threading.Tasks;
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
        //private readonly IDepartmentRepository _departmentRespository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(
            //IDepartmentRepository departmentRespository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            //_departmentRespository = departmentRespository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
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
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0) 
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")

        {
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            if (department is null) return NotFound(new { StatusCode = 404 ,  Message = $"Department with Id :{id} is not found" });
            return View(viewName,department);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
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
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existingDepartment = await _unitOfWork.DepartmentRepository.GetAsync(id);
            if (existingDepartment == null)
            {
                return NotFound(new { StatusCode = 404, Message = $"Department with Id {id} not found" });
            }

            // Map only the fields that need updating
            _mapper.Map(model, existingDepartment);

            _unitOfWork.DepartmentRepository.Update(existingDepartment);
            var count = await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult?> Delete(int? id)
        {
            //var department = _departmentRespository.Get(id);
            //if (department == null)
            //{
            //    return NotFound();
            //}
            //if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department with Id :{id} is not found" });
            return await Details(id , "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest(); //400     
                _unitOfWork.DepartmentRepository.Delete(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(department);

        }
    }
}
