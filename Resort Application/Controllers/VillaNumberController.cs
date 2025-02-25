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
                return RedirectToAction("Index");
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


        public IActionResult Update(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villaId);
            //Villa? obj = _db.Villas.Find(villaId);
            //var VillaList = _db.Villas.Where(u => u.Price > 50 && u.Occupancy > 0);
            if (obj == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa Obj)
        {
            
            if (ModelState.IsValid && Obj.Id>0)
            {
                _db.Villas.Update(Obj);
                _db.SaveChanges();
                TempData["success"] = "The Villa has been updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _db.Villas.FirstOrDefault(u => u.Id == villaId);   
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa Obj)
        {
            Villa? objFromDb = _db.Villas.FirstOrDefault(u=> u.Id == Obj.Id);
            if (objFromDb is not null)
            {
                _db.Villas.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "The Villa has been deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Villa could not be deleted ";
            return View();
        }

    }
}
