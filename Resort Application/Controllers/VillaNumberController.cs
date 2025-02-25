using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resort_Application.ViewModels;
using White.Lagoon.Domain.Entities;
using White.Lagoon.infrastructure.Data;

namespace Resort_Application.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var VillaNumbers = _db.VillaNumbers.Include(u=>u.Villa).ToList();
            return View(VillaNumbers);
        }
        public IActionResult Create()
        {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u =>new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };  
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM Obj)
        {
            //ModelState.Remove("Villa");

            bool roomNumberExists = _db.VillaNumbers.Any(u=>u.Villa_number == Obj.VillaNumber.Villa_number);
            if (ModelState.IsValid && !roomNumberExists)
            {
                _db.VillaNumbers.Add(Obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The Villa number has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The Villa Number already exists.";
            }
            Obj.VillaList = _db.Villas.ToList().Select(u=> new SelectListItem
            {
                Text =u.Name,
                Value = u.Id.ToString()
            });
            return View(Obj);
        }


        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u=> u.Villa_number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
          
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The Villa number has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            villaNumberVM.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(villaNumberVM);     
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFromDb = _db.VillaNumbers
             .FirstOrDefault(u => u.Villa_number == villaNumberVM.VillaNumber.Villa_number);

            if (objFromDb is not null)
            {
                _db.VillaNumbers.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "The Villa number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa number could not be deleted ";
            return View();
        }

    }
}
