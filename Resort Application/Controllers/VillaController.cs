using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using White.Lagoon.Domain.Entities;
using White.Lagoon.infrastructure.Data;

namespace Resort_Application.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
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
                _db.Villas.Add(Obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
       


    }
}
