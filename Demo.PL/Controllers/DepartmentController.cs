using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        // Action : Public Non-Static Methods
        //private IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork ,IMapper mapper /*DepartmentRepository departmentRepository*/)
        {
            // _departmentRepository = departmentRepository;   //Ask CLR  Create Object from DepartmentRepository  

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        // Action
        // /Department/Index
        public async Task<IActionResult> Index(string SearchInput)
        {

            //var departments = Enumerable.Empty<Department>();


            //if (string.IsNullOrEmpty(SearchInput))
            //{
            //    departments = await _unitOfWork.DepartmentRepository.GetAll();
            //}
            //else
            //{
            //    departments = _unitOfWork.DepartmentRepository.GetByName(SearchInput.ToLower());
            //}
            //var result = _mapper.Map<IEnumerable<DepartmentViewModel>>(departments);

            //return View(result);


            var departments = Enumerable.Empty<Department>();
            //var departments = await _unitOfWork.DepartmentRepository.GetAll();
            if(string.IsNullOrEmpty(SearchInput))
            {
                departments = await _unitOfWork.DepartmentRepository.GetAll();
            }
            else
            {
                departments = _unitOfWork.DepartmentRepository.GetByName(SearchInput.ToLower());
            }

            return View(departments);

        }

        // Department/Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {



                 _unitOfWork.DepartmentRepository.Add(model);

                var Count = await _unitOfWork.Complete();


                if (Count > 0)
                {
                    TempData["Message"] = "Department Is Created";
                }
                else
                {
                    TempData["Message"] = "Department Is Not Created";

                }
                return RedirectToAction(nameof(Index));

            }

            return View(model);
        }





        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();  // Error 400
            }

            var department = await _unitOfWork.DepartmentRepository.Get(id.Value);




            if (department is null)
            {
                return NotFound();  // Error 404
            }

            return View(ViewName, department);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {


            return await Details(id, "Edit");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, Department model)
        {
            if (id != model.Id)
                return BadRequest();  //400



            if (ModelState.IsValid)   //Server Side Validation
            {
                _unitOfWork.DepartmentRepository.Update(model);

                var count = await _unitOfWork.Complete();

                if (count > 0)
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
        public async Task<IActionResult> Delete([FromRoute] int? id, Department model)
        {
            if (id != model.Id)
                return BadRequest();    //400

            if (ModelState.IsValid)    //Server Side Validation
            {
                _unitOfWork.DepartmentRepository.Delete(model);

                var count = await _unitOfWork.Complete();

                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

    }

}
