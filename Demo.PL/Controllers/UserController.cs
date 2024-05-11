using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        //inject from user manager
        public UserController(UserManager<ApplicationUser> userManager,IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchInput)
        {
            var users=Enumerable.Empty<UserViewModel>();
            if (string.IsNullOrEmpty(SearchInput))
            {
                users =await _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FName = U.FirstName,
                    LName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToListAsync();

            }
            else
            {
                users = await _userManager.Users.Where(U=>U.Email.ToLower()
                                                .Contains(SearchInput.ToLower()))
                                                .Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FName = U.FirstName,
                    LName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToListAsync();

            }

            return View(users);
           
        }

        public async Task<IActionResult> Details(string? id,string ViewName="Details") 
        {
            if (id is null)
                return BadRequest();
            var userFromDb = await _userManager.FindByIdAsync(id);
            if (userFromDb is null)
                return NotFound();

            var user = new UserViewModel()
            {
                Id = userFromDb.Id,
                FName = userFromDb.FirstName,
                LName = userFromDb.LastName,
                Email = userFromDb.Email,
                Roles = _userManager.GetRolesAsync(userFromDb).Result
            };
            return View(ViewName,user);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserViewModel model)
        {
            if(id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                var userFromDb =await _userManager.FindByIdAsync(id);
                if(userFromDb is null)
                    return NotFound();
                userFromDb.FirstName = model.FName;
                userFromDb.LastName=model.LName;
                await _userManager.UpdateAsync(userFromDb);
                return RedirectToAction(nameof(Index));
            
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                var userFromDb = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(userFromDb);
                return RedirectToAction(nameof(Index));

            }
            return View(model);
        }


    }
}
