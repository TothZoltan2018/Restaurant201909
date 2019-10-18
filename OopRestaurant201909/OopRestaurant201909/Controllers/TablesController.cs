using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OopRestaurant201909.Models;

namespace OopRestaurant201909.Controllers
{
    public class TablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tables
        public ActionResult Index()
        {
            var tables = db.Tables
                           .Include(x => x.Location)
                           //.OrderBy(x => x.Location.IsOutdoor) //utana ugy is szurjuk, igy minek sorbarendezni...
                           .ToList(); //Az EF csak a ToList hatasara fog tenyleges adatbazis adatlekerest vegrehajtani!
            //elkeszitjuk a ViewModel-t
            //1. kelleni fog a termek listaja
            var locations = db.Locations.ToList();
            
            foreach (var location in locations)
            {
                location.Tables = tables.Where(x =>x.Location.Id == location.Id)
                                        .ToList();
            }

            //majd ezt elkuldjuk a nezethez
            return View(locations);
        }

        // GET: Tables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // GET: Tables/Create
        [Authorize(Roles = "Fopincer, Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fopincer, Admin")]
        public ActionResult Create([Bind(Include = "Id,Name")] Table table)
        {
            if (ModelState.IsValid)
            {
                db.Tables.Add(table);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(table);
        }

        // GET: Tables/Edit/5
        [Authorize(Roles = "Fopincer, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fopincer, Admin")]
        public ActionResult Edit([Bind(Include = "Id,Name")] Table table)
        {
            if (ModelState.IsValid)
            {
                db.Entry(table).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(table);
        }

        // GET: Tables/Delete/5
        [Authorize(Roles = "Fopincer, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Tables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fopincer, Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Table table = db.Tables.Find(id);
            db.Tables.Remove(table);
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
