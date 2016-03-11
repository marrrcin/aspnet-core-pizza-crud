using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.Data.Entity;
using PizzaCRUD.Data;
using PizzaCRUD.Models;

namespace PizzaCRUD.Controllers
{
    public class PizzasController : Controller
    {
        protected PizzaDbContext Db;

        public PizzasController(PizzaDbContext db)
        {
            Db = db;
        }

        // GET: Pizzas
        public async Task<IActionResult> Index()
        {
            return View(await Db.Pizzas.ToListAsync());
        }

        // GET: Pizzas/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Pizza pizza = await Db.Pizzas.Include(p=>p.Ingridients).SingleAsync(m => m.Id == id);
            if (pizza == null)
            {
                return HttpNotFound();
            }
            SelectUsedIngridients(pizza);
            return View(pizza);
        }

        // GET: Pizzas/Create
        public IActionResult Create()
        {
            ViewBag.ActionType = "Create";
            EnsureMandatoryIngridients();
            var pizza = new Pizza
            {
                PriceLarge = (decimal)34.99,
                PriceMedium = (decimal)24.99,
                PriceSmall = (decimal)14.99
            };
            SelectAllIngridients(pizza);
            return View("CreateEdit",pizza);
        }

        public IActionResult Search(string query)
        {
            return View("Index", Db.Pizzas.Where(pizza => pizza.Name.StartsWith(query)));
        }

        protected virtual void EnsureMandatoryIngridients()
        {
            if (Db.Ingridients.Count(ig => MandatoryIngridients.Ingridients.Contains(ig.Name)) != MandatoryIngridients.Ingridients.Length)
            {
                MandatoryIngridients.Ingridients.ToList().ForEach(mi =>
                {
                    try
                    {
                        Db.Ingridients.Add(new Ingridient {Name = mi});
                        Db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        //seed only
                    }
                });
            }
        }

        // POST: Pizzas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pizza pizza)
        {
            ViewBag.ActionType = "Create";
            if (ModelState.IsValid)
            {
                if (!Db.Pizzas.Any(p => p.Name == pizza.Name))
                {
                    await AddPizza(pizza);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("","Taka pizza już istnieje");
                }
                
            }
            SelectAllIngridients(pizza);
            return View("CreateEdit",pizza);
        }

        protected virtual void SelectAllIngridients(Pizza pizza)
        {
            pizza.UsedIngridients =
                Db.Ingridients.ToArray()
                    .ToDictionary(ig => ig.Name, ig => MandatoryIngridients.Ingridients.Contains(ig.Name) || (pizza.UsedIngridients != null && pizza.UsedIngridients.ContainsKey(ig.Name)));
        }

        protected virtual async Task AddPizza(Pizza pizza)
        {
            pizza.Id = Guid.NewGuid();
            Db.Pizzas.Add(pizza);
            AddPizzaIngridients(pizza);
            await Db.SaveChangesAsync();
        }

        protected virtual void AddPizzaIngridients(Pizza pizza)
        {
            var ingridients = new List<Ingridient>();
            MandatoryIngridients.Ingridients.ToList()
                .ForEach(igr => ingridients.Add(Db.Ingridients.FirstOrDefault(ingr => ingr.Name == igr)));
            pizza.UsedIngridients.Where(igr => !MandatoryIngridients.Ingridients.Contains(igr.Key)).ToList().ForEach(ingr =>
            {
                if (ingr.Value)
                {
                    ingridients.Add(Db.Ingridients.FirstOrDefault(i => i.Name == ingr.Key));
                }
            });

            ingridients.Select(ingr => new PizzaIngridient {IngridientId = ingr.Id, PizzaId = pizza.Id})
                .ToList()
                .ForEach(ingr => Db.PizzaIngridients.Add(ingr));
        }

        // GET: Pizzas/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            ViewBag.ActionType = "Edit";
            if (id == null)
            {
                return HttpNotFound();
            }

            var pizza = await Db.Pizzas.Include(p=>p.Ingridients).SingleAsync(m => m.Id == id);
            if (pizza == null)
            {
                return HttpNotFound();
            }
            SelectUsedIngridients(pizza);
            return View("CreateEdit", pizza);
        }

        protected virtual void SelectUsedIngridients(Pizza pizza)
        {
            pizza.UsedIngridients = Db.Ingridients.ToArray()
                .ToDictionary(igr => igr.Name, igr => pizza.Ingridients.Any(i => i.IngridientId == igr.Id));
        }

        // POST: Pizzas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Pizza pizza)
        {
            ViewBag.ActionType = "Edit";
            if (ModelState.IsValid)
            {
                Db.PizzaIngridients.Where(pi=>pi.PizzaId == pizza.Id).ToList().ForEach(pi=> Db.PizzaIngridients.Remove(pi));
                await Db.SaveChangesAsync();
                AddPizzaIngridients(pizza);
                Db.Update(pizza);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            pizza.UsedIngridients = Db.Ingridients.ToArray().ToDictionary(igr => igr.Name, igr => Db.Pizzas.Include(p=>p.Ingridients).Single(p=>p.Id == pizza.Id).Ingridients.Any(i => i.IngridientId == igr.Id));
            return View("CreateEdit", pizza);
        }

        // GET: Pizzas/Delete/5
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            Pizza pizza = await Db.Pizzas.Include(p => p.Ingridients).SingleAsync(m => m.Id == id);
            if (pizza == null)
            {
                return HttpNotFound();
            }
            SelectUsedIngridients(pizza);
            return View(pizza);
        }

        // POST: Pizzas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            Pizza pizza = await Db.Pizzas.SingleAsync(m => m.Id == id);
            Db.Pizzas.Remove(pizza);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
