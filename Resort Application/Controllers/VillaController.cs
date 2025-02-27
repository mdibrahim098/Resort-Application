﻿using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using White.Lagoon.Application.Common.Interfaces;
using White.Lagoon.Domain.Entities;
using White.Lagoon.infrastructure.Data;

namespace Resort_Application.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
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
                if (Obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");
                    using (var fileStream = new FileStream(Path.Combine(imagePath,fileName),FileMode.Create))
                           Obj.Image.CopyTo(fileStream);

                           Obj.ImageUrl = @"\images\VillaImage\" + fileName;

                }
                else
                {
                    Obj.ImageUrl = "https://placehold.co/600x400";
                }

                _unitOfWork.Villa.Add(Obj);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);
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

                if (Obj.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(Obj.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");
                    if (!string.IsNullOrEmpty(Obj.ImageUrl))
                    {
                        var OldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, Obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(OldImagePath))
                        {
                            System.IO.File.Delete(OldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                        Obj.Image.CopyTo(fileStream);

                    Obj.ImageUrl = @"\images\VillaImage\" + fileName;

                }
                _unitOfWork.Villa.Update(Obj);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.Id == villaId);   
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa Obj)
        {
            Villa? objFromDb = _unitOfWork.Villa.Get(u=> u.Id == Obj.Id);
            if (objFromDb is not null)
            {

                if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                {
                    var OldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(OldImagePath))
                    {
                        System.IO.File.Delete(OldImagePath);
                    }
                }
                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The Villa has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be deleted ";
            return View();
        }

    }
}
