using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Resort_Application.ViewModels;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Application.Services.Interface;
using White.Lagoon.Domain.Entities;
using White.Lagoon.infrastructure.Data;

namespace Resort_Application.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;

        public VillaNumberController(IVillaNumberService villaNumberService, IVillaService villaService)
        {
            _villaNumberService = villaNumberService;
            _villaService = villaService;
        }
        public IActionResult Index()
        {
            var VillaNumbers = _villaNumberService.GetAllVillaNumbers();
            return View(VillaNumbers);
        }
        public IActionResult Create()
        {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _villaService.GetAllVillas().Select(u =>new SelectListItem
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

            bool roomNumberExists = _villaNumberService.CheckVillaNumberExists(Obj.VillaNumber.Villa_number);
            if (ModelState.IsValid && !roomNumberExists)
            {
              _villaNumberService.CreateVillaNumber(Obj.VillaNumber);
                TempData["success"] = "The Villa number has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The Villa Number already exists.";
            }
            Obj.VillaList = _villaService.GetAllVillas().Select(u=> new SelectListItem
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
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _villaNumberService.GetVillaNumberById(villaNumberId)
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
              _villaNumberService.UpdateVillaNumber(villaNumberVM.VillaNumber);
                TempData["success"] = "The Villa number has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            villaNumberVM.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
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
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _villaNumberService.GetVillaNumberById(villaNumberId)
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
            VillaNumber? objFromDb = _villaNumberService.GetVillaNumberById(villaNumberVM.VillaNumber.Villa_number);

            if (objFromDb is not null)
            {
                 _villaNumberService.DeleteVillaNumber(objFromDb.Villa_number);
                TempData["success"] = "The Villa number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa number could not be deleted ";
            return View();
        }

    }
}
