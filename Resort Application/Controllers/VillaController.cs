using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Domain.Entities;
using White.Lagoon.infrastructure.Data;

namespace Resort_Application.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository _VillaRepo;

        public VillaController(IVillaRepository villaRepo)
        {
            _VillaRepo = villaRepo;
        }
        public IActionResult Index()
        {
            var villas = _VillaRepo.GetAll();
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa Obj)
        {
            if (Obj.Name== Obj.Description)
            {
                ModelState.AddModelError("Name", "The description cannot exactly match the name.");
            }
            if (ModelState.IsValid)
            {
                _VillaRepo.Add(Obj);
                _VillaRepo.Save();
                TempData["success"] = "The Villa has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        public IActionResult Update(int villaId)
        {
            Villa? obj = _VillaRepo.Get(u => u.Id == villaId);
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
                _VillaRepo.Update(Obj);
                _VillaRepo.Save();
                TempData["success"] = "The Villa has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _VillaRepo.Get(u => u.Id == villaId);   
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa Obj)
        {
            Villa? objFromDb = _VillaRepo.Get(u=> u.Id == Obj.Id);
            if (objFromDb is not null)
            {
                _VillaRepo.Remove(objFromDb);
                _VillaRepo.Save();
                TempData["success"] = "The Villa has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be deleted ";
            return View();
        }

    }
}
