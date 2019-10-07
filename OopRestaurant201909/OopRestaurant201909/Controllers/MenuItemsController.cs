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
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }

            LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        // GET: MenuItems/Create
        [Authorize(Roles = "Fopincer,Admin")] //Csak a Fopincer es az Admin csoport tagjai hasznalhatjak ezt az Action-t

        //Az alabbi megoldast nem hasznaljuk, mert minden uj felhasznalonak hozza kell nyulni a kodhoz, es ujraforditani/telepiteni
        //[Authorize(Roles = "toth.tozso.zoltan@gmail.com")] 
        public ActionResult Create()
        {
            var menuItem = new MenuItem();
            //todo: ezt az adatbetoltest csinaljuk meg jol!
            LoadAssignableCategories(menuItem);

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
            var category = db.Categories.Find(menuItem.CategoryId);

            if (category == null)
            {//Ha nincs ilyen kategoria, akko rnem lehet mit, visszakuldom az adatokat modositasra 
                LoadAssignableCategories(menuItem);
                return View(menuItem);
            }

            db.MenuItems.Attach(menuItem);

            //Mivel ez egy teljesen uj elem, ami meg nem volt az adatbazisban (ellentetben az Edit-tel), 
            //ezert nem tudunk property-t tolteni, mert meg nincs honnan.
            //Ezert ebben az estben az alabbi sor nem kell (es InvalidOperationsException-t is dobna).
            //db.Entry(menuItem).Reference(x => x.Category).Load();

            menuItem.Category = category;

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
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }

            //Ahhoz, hogy legyen, be kell toltenunk a menuItem Category property-jet,
            //amit magatol az EF nem tolt be
            db.Entry(menuItem)
                .Reference(x => x.Category)
                .Load();

            //Hogy be tudjuk allitani a lenyilot, ezert megadjuk az aktualis Category azonositojat
            menuItem.CategoryId = menuItem.Category.Id;
            //Lekuldjuk a Categories adatbazistabla tartalmat (db.Categories.ToList()),
            //megadjuk, hogy melyik mezo azonositja a sort, es adja azt az erteket, ami a a vegeredmeny (Id)
            //megadjuk, hogy a lenyilo egyes soraiba melyik property (oszlop) ertekei keruljenek. (Name)
            LoadAssignableCategories(menuItem);

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
            var category = db.Categories.Find(menuItem.CategoryId);

            if (category == null)
            {//Ha nincs ilyen kategoria, akkor nem lehet mit, visszakuldom az adatokat modositasra 
                LoadAssignableCategories(menuItem);
                return View(menuItem);
            }

            //A html formrol jovo adatokat bemutatjuk az adatbazisnak
            db.MenuItems.Attach(menuItem);

            //az adatbazissal kaocsolatos dolgok eleresehez kell az entry
            var entry = db.Entry(menuItem);

            //ennek segitsegevel betoltjuk a Category tabla adatait a menuItem.Category property-be
            entry.Reference(x => x.Category).Load();

            //majd felulirjuk azzal, ami bejott a html formon
            menuItem.Category = category;

            //Ujra kell az adatok ellenorzeset vegezni, hiszen
            //modositottam az egyes property-ket
            ModelState.Clear();
            TryValidateModel(menuItem);

            if (ModelState.IsValid)
            {
                entry.State = EntityState.Modified;
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
            MenuItem menuItem = db.MenuItems.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }

            LoadAssignableCategories(menuItem);
            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fopincer,Admin")] //Csak a Fopincer es az Admin csoport tagjai hasznalhatjak ezt az Action-t
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem menuItem = db.MenuItems.Find(id);
            db.MenuItems.Remove(menuItem);
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
