using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Application.Services.Implementation;
using White.Lagoon.Application.Services.Interface;
using White.Lagoon.Domain.Entities;
using White.Lagoon.infrastructure.Data;

namespace Resort_Application.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
       
        private readonly IVillaService _villaService;

        public VillaController(IVillaService villaService)
        {
            _villaService = villaService;
        }
        public IActionResult Index()
        {
            var villas = _villaService.GetAllVillas();
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
                _villaService.CreateVilla(Obj);
                 TempData["success"] = "The Villa has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _villaService.GetVillaById(villaId);
         
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
                _villaService.UpdateVilla(Obj);
                TempData["success"] = "The Villa has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _villaService.GetVillaById(villaId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa Obj)
        {
            bool deleted = _villaService.DeleteVilla(Obj.Id);

            if(deleted)
            {
                TempData["success"] = "The Villa has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the villa. ";
            }

            return View();
        }

    }
}
