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

        private Table ReadOrNewTable(int? id, ReadOrNewOperation op)
        {
            Table table;

            switch (op)
            {
                case ReadOrNewOperation.Read:
                    table = db.Tables.Find(id);
                    if (table == null)
                    {
                        return null;
                    }

                    //Szolni kell az EF-nek, hogy az asztalhoz toltse be a termet is.
                    db.Entry(table)
                        .Reference(x => x.Location)
                        .Load();
                    break;
                case ReadOrNewOperation.New:
                    table = new Table();
                    break;
                default:
                    throw new Exception($"Erre a muveletre nem vagyunk felkeszulve: {op}");
            }

            //A lenyilo mezo adatainak kitoltese
            //?: felteteles null operator:
            //      ha eddig a kifejezes ertekes null, akkor megall, es a vegeredmeny null
            //      ha pedig nem null, akkor folytatodik a kiertekeles, es megy tovabb

            //ugyanaz, mint ez:
            //int? eredmeny;
            //if (table.Location == null)
            //{
            //    eredmeny = null;
            //}
            //else
            //{
            //    eredmeny = table.Location.Id;
            //}

            //??: null operator: ha a bal oldalan szereplo ertek null, akkor az ererdmeny a jobb oldalan szereplo ertek
            //ugyanaz, mint ez:

            //if (eredmeny == null)
            //{
            //    eredmeny = 0;
            //}

            table.LocationId = table.Location?.Id ?? 0;
            LoadAssignableLocations(table);

            return table;
        }

        private void LoadAssignableLocations(Table table)
        {
            table.AssignableLocations = new SelectList(db.Locations.ToList(), "Id", "Name");
        }

        private void CreateUpdateOrDeleteTable(Table table, CreateUpdateOrDeleteOperation op)
        {
            switch (op)
            {
                case CreateUpdateOrDeleteOperation.Create:
                    //a kivalasztott termet is be kell allitani
                    table.Location = db.Locations.Find(table.LocationId); //table.LocationId: A lenyilo kivalasztott erteke
                    db.Tables.Add(table);
                    break;
                case CreateUpdateOrDeleteOperation.Update:
                    //a kivalasztott termet is be kell allitani

                    //1. Be kell mutatni a modellt az adatbazisnak
                    db.Tables.Attach(table);

                    //2. Be kell tolteni a hozzatartozo eredeti teremadatokat
                    db.Entry(table)                     //kerem az EF adatbazist elero reszet
                        .Reference(x => x.Location)     //kerem a csatlakozo tabalk kozul a Location-t
                        .Load();                        //Onnan betoltom az adatokat

                    //3. Modositani kell az uj terem adatot a lenyilo mezobol
                    table.Location = db.Locations.Find(table.LocationId);

                    //4. Jelezni kell, hogy valtozott, igy a tobbi ertek (Name, stb.) valtozast is figyelembe veszi az EF
                    db.Entry(table).State = EntityState.Modified;

                    
                    break;
                case CreateUpdateOrDeleteOperation.Delete:
                    db.Tables.Remove(table);
                    break;
                default:
                    throw new Exception($"Erre a muveletre nem vagyunk felkeszulve: {op}");
            }
            
        }

        // GET: Tables
        public ActionResult Index()
        {
            //Betoltom a termek listajat es megkerem az EF-ot (.Include), hogy tegye
            //hozza a teremhez tartozo asztalok listajat
            var locations = db.Locations
                                .Include(x => x.Tables)
                                .ToList();            
  
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
            Table table = ReadOrNewTable(id, ReadOrNewOperation.Read);
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
            var table = ReadOrNewTable(null, ReadOrNewOperation.New);

            return View(table);
        }

        // POST: Tables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fopincer, Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,LocationId")] Table table)
        {
            CreateUpdateOrDeleteTable(table, CreateUpdateOrDeleteOperation.Create);

            ModelState.Clear();
            TryValidateModel(table);

            if (ModelState.IsValid)
            {                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadAssignableLocations(table);

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
            Table table = ReadOrNewTable(id, ReadOrNewOperation.Read);
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
        public ActionResult Edit([Bind(Include = "Id,Name,LocationId")] Table table)
        {
            CreateUpdateOrDeleteTable(table, CreateUpdateOrDeleteOperation.Update);

            ModelState.Clear();
            TryValidateModel(table);

            if (ModelState.IsValid)
            {                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //Ha nem sikerult a validacio, akkor mielott visszamegy a nezetre, be kell toltenunk a lenyilo erteket
            LoadAssignableLocations(table);
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
            Table table = ReadOrNewTable(id, ReadOrNewOperation.Read);
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
            CreateUpdateOrDeleteTable(table, CreateUpdateOrDeleteOperation.Delete);
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
