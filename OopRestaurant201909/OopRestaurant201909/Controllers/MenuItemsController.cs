using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OopRestaurant201909;
using OopRestaurant201909.Models;

namespace OopRestaurant201909.Controllers
{
    public class MenuItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Ez a fuggveny felelos a model betolteseert minden megjelenito (get) action eseten
        /// </summary>
        /// <param name="id">Menuitem azonosito, lehet null is.</param>
        /// <param name="op">Muvelet: Read vagy New</param>
        /// <returns></returns>
        private MenuItem ReadOrNewMenuItem(int? id, ReadOrNewOperation op)
        {
            MenuItem menuItem;

            switch (op)
            {
                case ReadOrNewOperation.Read:
                    //1. Adatok betoltese az adatbazisbol (Model)                    
                    menuItem = db.MenuItems.Find(id);

                    if (menuItem == null)
                    {
                        return null;
                    }

                    //Ahhoz, hogy legyen, be kell toltenunk a menuItem Category property-jet,
                    //amit magatol az EF nem tolt be
                    db.Entry(menuItem)
                        .Reference(x => x.Category)
                        .Load();
                    break;
                case ReadOrNewOperation.New:
                    //1. Adat (model) peldanyositasa
                    menuItem = new MenuItem();
                    break;
                default:
                    throw new Exception($"Erre nem keszultnk fel: {op}");                    
            }

            //2. Megjelentesi adatok feltoltese (ViewModel)
            //Hogy be tudjuk allitani a lenyilot, ezert megadjuk az aktualis Category azonositojat
            if (menuItem.Category != null)
            {
                menuItem.CategoryId = menuItem.Category.Id;
            }
            
            //Lekuldjuk a Categories adatbazistabla tartalmat (db.Categories.ToList()),
            //megadjuk, hogy melyik mezo azonositja a sort, es adja azt az erteket, ami a a vegeredmeny (Id)
            //megadjuk, hogy a lenyilo egyes soraiba melyik property (oszlop) ertekei keruljenek. (Name)
            LoadAssignableCategories(menuItem);

            return menuItem;            
        }

        private void CreateUpdateOrDeleteMenuItem(MenuItem menuItem, CreateUpdateOrDeleteOperation op)
        {
            switch (op)
            {
                case CreateUpdateOrDeleteOperation.Create:
                    var categoryCreate = db.Categories.Find(menuItem.CategoryId);

                    //if (category == null)
                    //{//Ha nincs ilyen kategoria, akkor nem lehet mit tenni, visszakuldom az adatokat modositasra 
                    //    LoadAssignableCategories(menuItem);
                    //    return View(menuItem);
                    //}

                    db.MenuItems.Attach(menuItem);

                    //Mivel ez egy teljesen uj elem, ami meg nem volt az adatbazisban (ellentetben az Edit-tel), 
                    //ezert nem tudunk property-t tolteni, mert meg nincs honnan.
                    //Ezert ebben az estben az alabbi sor nem kell (es InvalidOperationsException-t is dobna).
                    //db.Entry(menuItem).Reference(x => x.Category).Load();

                    menuItem.Category = categoryCreate;
                    return;

                case CreateUpdateOrDeleteOperation.Update:
                    var categoryUpdate = db.Categories.Find(menuItem.CategoryId);

                    //if (category == null)
                    //{//Ha nincs ilyen kategoria, akkor nem lehet mit tenni, visszakuldom az adatokat modositasra 
                    //    LoadAssignableCategories(menuItem);
                    //    return View(menuItem);
                    //}

                    //A html formrol jovo adatokat bemutatjuk az adatbazisnak
                    db.MenuItems.Attach(menuItem);

                    //az adatbazissal kapocsolatos dolgok eleresehez kell az entry
                    var entry = db.Entry(menuItem);

                    //ennek segitsegevel betoltjuk a Category tabla adatait a menuItem.Category property-be
                    entry.Reference(x => x.Category).Load();

                    //majd felulirjuk azzal, ami bejott a html formon
                    menuItem.Category = categoryUpdate;
                    entry.State = EntityState.Modified; //Csak, ha ez be van allitva, akkor menti a modositasokat az EF
                    return;

                case CreateUpdateOrDeleteOperation.Delete:
                    db.MenuItems.Remove(menuItem);
                    return;
                default:
                    throw new Exception($"Erre nem vagyunk felkeszulve: {op}");
            }
        }

        private void LoadAssignableCategories(MenuItem menuItem)
        {
            menuItem.AssignableCategories = new SelectList(db.Categories.OrderBy(x => x.Name).ToList(), "id", "Name");
        }

        // GET: MenuItems
        public ActionResult Index()
        {
            //Az Include nelkul nem tolti be a MenuItem-be a Category erteket, mert az egy masik tablaban van
            return View(db.MenuItems.Include(x => x.Category).ToList());
        }

        // GET: MenuItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
            if (menuItem == null)
            {
                return HttpNotFound();
            }

            return View(menuItem);
        }

        // GET: MenuItems/Create
        [Authorize(Roles = "Fopincer,Admin")] //Csak a Fopincer es az Admin csoport tagjai hasznalhatjak ezt az Action-t

        //Az alabbi megoldast nem hasznaljuk, mert minden uj felhasznalo miatt hozza kellene nyulni a kodhoz, es ujraforditani/telepiteni
        //[Authorize(Roles = "toth.tozso.zoltan@gmail.com")] 
        public ActionResult Create()
        {
            var menuItem = ReadOrNewMenuItem(null, ReadOrNewOperation.New);
            
            return View(menuItem);
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fopincer,Admin")] //Csak a Fopincer es az Admin csoport tagjai hasznalhatjak ezt az Action-t
        public ActionResult Create([Bind(Include = "Id,Name,Description,Price, CategoryId")] MenuItem menuItem)
        {
            CreateUpdateOrDeleteMenuItem(menuItem, CreateUpdateOrDeleteOperation.Create);

            //Ujra kell az adatok ellenorzeset vegezni, hiszen
            //modositottam az egyes property-ket
            ModelState.Clear();
            TryValidateModel(menuItem);


            if (ModelState.IsValid)
            {
                db.MenuItems.Add(menuItem);
                db.SaveChanges();


                return RedirectToAction("Index");
            }

            LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        // GET: MenuItems/Edit/5
        [Authorize(Roles = "Fopincer,Admin")] //Csak a Fopincer es az Admin csoport tagjai hasznalhatjak ezt az Action-t
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
            if (menuItem == null)
            {
                return HttpNotFound();
            }

            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]//Be kell engedni a lenyilo altal kivalasztott azonositot is:CategoryId 
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,Price,CategoryId")] MenuItem menuItem)
        {
            CreateUpdateOrDeleteMenuItem(menuItem, CreateUpdateOrDeleteOperation.Update);

            //Ujra kell az adatok ellenorzeset vegezni, hiszen
            //modositottam az egyes property-ket
            ModelState.Clear();
            TryValidateModel(menuItem);

            if (ModelState.IsValid)
            { 
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadAssignableCategories(menuItem);

            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        [Authorize(Roles = "Fopincer,Admin")] //Csak a Fopincer es az Admin csoport tagjai hasznalhatjak ezt az Action-t
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            
            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fopincer,Admin")] //Csak a Fopincer es az Admin csoport tagjai hasznalhatjak ezt az Action-t
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem menuItem = ReadOrNewMenuItem(id, ReadOrNewOperation.Read);
            CreateUpdateOrDeleteMenuItem(menuItem, CreateUpdateOrDeleteOperation.Delete);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
