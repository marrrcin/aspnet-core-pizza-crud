using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using PizzaCRUD.Data;
using PizzaCRUD.Models;

namespace PizzaCRUD.Controllers
{
    public class IngridientsController : Controller
    {
        protected PizzaDbContext Db;

        public IngridientsController(PizzaDbContext context)
        {
            Db = context;    
        }

        // GET: Ingridients
        public async Task<IActionResult> Index()
        {
            return View(await Db.Ingridients.ToListAsync());
        }

        // GET: Ingridients/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Ingridient ingridient = await Db.Ingridients.SingleAsync(m => m.Id == id);
            if (ingridient == null)
            {
                return HttpNotFound();
            }

            return View(ingridient);
        }

        // GET: Ingridients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ingridients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingridient ingridient)
        {
            ingridient.Name = (ingridient.Name ?? "").Trim();
            if (string.IsNullOrEmpty(ingridient.Name))
            {
                ModelState.AddModelError("", "Nazwa nie może być pusta!");
            }

            if (ModelState.IsValid)
            {
                if (!Db.Ingridients.Any(igr => igr.Name == ingridient.Name))
                {
                    ingridient.Id = Guid.NewGuid();
                    Db.Ingridients.Add(ingridient);
                    await Db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Taki składnik już istnieje!");
                }
                
            }
            return View(ingridient);
        }

        // GET: Ingridients/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Ingridient ingridient = await Db.Ingridients.SingleAsync(m => m.Id == id);
            if (ingridient == null)
            {
                return HttpNotFound();
            }
            return View(ingridient);
        }

        // POST: Ingridients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Ingridient ingridient)
        {
            if (Db.Ingridients.Any(igr => igr.Name == ingridient.Name))
            {
                ModelState.AddModelError("","Taki składnik już istnieje!");;
            }

            ingridient.Name = (ingridient.Name ?? "").Trim();
            if (string.IsNullOrEmpty(ingridient.Name))
            {
                ModelState.AddModelError("", "Nazwa nie może być pusta!");
            }

            if (ModelState.IsValid)
            {
                Db.Update(ingridient);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(ingridient);
        }

        // GET: Ingridients/Delete/5
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(Guid? id,string error="")
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Ingridient ingridient = await Db.Ingridients.SingleAsync(m => m.Id == id);
            if (ingridient == null)
            {
                return HttpNotFound();
            }

            if (error != "")
            {
                ViewBag.Error = "Składnik używany przez co najmniej jedną pizzę!";
            }
            return View(ingridient);
        }

        // POST: Ingridients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (Db.PizzaIngridients.Any(pi => pi.IngridientId == id))
            {
                return RedirectToAction("Delete", new {id = id, error = "yes"});
            }

            var ingridient = await Db.Ingridients.SingleAsync(m => m.Id == id);
            Db.Ingridients.Remove(ingridient);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
