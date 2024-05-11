using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{

    [Authorize]

    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // private IEmployeeRepository _employeeRepository; //Null
        // private readonly IDepartmentRepository _departmentRepository;
        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper /*IEmployeeRepository employeeRepository*/
            /*IDepartmentRepository departmentRepository*/  )  // Ask CLR to create object from class Implements interFace IDepartmentRepository

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            // _departmentRepository = departmentRepository;
            //_employeeRepository = employeeRepository;
        }


        //   /Employee/Index

        public async Task<IActionResult> Index(string SearchInput)
        {
            var employees = Enumerable.Empty<Employee>();


            if (string.IsNullOrEmpty(SearchInput))
            {
                //  employees = _employeeRepository.GetAll();
                employees = await _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                //employees = _employeeRepository.GetByName(SearchInput.ToLower());

                employees = _unitOfWork.EmployeeRepository.GetByName(SearchInput.ToLower());
            }
            var result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            return View(result);


        }


        //View 's Dictionary : Transfer Extra Information from action to view (one way)(key,value)

        // 1 - ViewData : Property inherted from controller class,Dictionary

        //   ViewData["Message"] = "Hello ViewData";

        // 2 - ViewBag :  Property inherted from controller class,Dynamic

        //  ViewBag.Message = "Hello ViewBag";

        // 3 - ViewTemp : Property inherted from controller class,Like ViewData

        //Transfer information from request to another 


        //  /Employee/Create

        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["Departments"] =_unitOfWork.DepartmentRepository.GetAll();
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {

                model.ImageName = DocumentSetting.UploadFile(model.Image, "images");


                var employee = _mapper.Map<Employee>(model);

                //Manual Mapping
                //Employee employee = new Employee()
                //{
                //    Id = model.Id,
                //    Name = model.Name,
                //    HireDate = model.HireDate,
                //    Salary = model.Salary,
                //    Address = model.Address,
                //    PhoneNumber = model.PhoneNumber,
                //    DateOfCreation = model.DateOfCreation,
                //    DepartmentId = model.DepartmentId,
                //    Department = model.Department,
                //    IsDeleted = model.IsDeleted
                //};

                //  var Count = _employeeRepository.Add(employee);



                _unitOfWork.EmployeeRepository.Add(employee);

                var Count = await _unitOfWork.Complete();

                if (Count > 0)
                {
                    TempData["Message"] = "Employee Is Created";
                }
                else
                {
                    TempData["Message"] = "Employee Is Not Created";

                }
                return RedirectToAction(nameof(Index));

            }

            return View(model);
        }


        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
            {
            if (id is null)
                return BadRequest();  // Error 400



            //var employee = _employeeRepository.Get(id.Value);

            var employee = await _unitOfWork.EmployeeRepository.Get(id.Value);


            var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound();  // Error 404


            return View(ViewName, employeeViewModel);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {


            //  ViewData["Departments"] = _unitOfWork.DepartmentRepository.GetAll();

            return await Details(id, "Edit");

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel model)
        {
            if (id != model.Id)

                return BadRequest();//400


            if (model.ImageName is not null)
            {
                DocumentSetting.Delete(model.ImageName, "images");
            }

            model.ImageName = DocumentSetting.UploadFile(model.Image, "images");


            var employee = _mapper.Map<Employee>(model);


            if (ModelState.IsValid)
            {
                _unitOfWork.EmployeeRepository.Update(employee);

                int Count = await _unitOfWork.Complete();


                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }



            return View(model);

        }




        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {


            return await Details(id, "Delete");


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? id, EmployeeViewModel model)
        {

            if (id != model.Id)

                return BadRequest();//400

            var employee = _mapper.Map<Employee>(model);

            if (ModelState.IsValid)
            {
                _unitOfWork.EmployeeRepository.Delete(employee);

                int Count = await _unitOfWork.Complete();

                if (Count > 0)
                {

                    DocumentSetting.Delete(model.ImageName, "images");

                    return RedirectToAction(nameof(Index));
                }
            }


            return View(model);
        }


    }
}
