using OopRestaurant201909.Models;

namespace OopRestaurant201909.Migrations
{
    //using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OopRestaurant201909.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OopRestaurant201909.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //A lambda kif. mondja meg, hogy mi alapjan azonositson. Nelkule az id lenne az, de 0-tol kezdodne, ami nem jo, mert a db-ben 1-tol van a szamozas
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Pizzák"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Italok"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Desszertek"));
            context.SaveChanges();

            var pizzaCategory = context.Categories
                                        .Single(x => x.Name == "Pizzák"); //Az egyetlent adja vissza, amire igaz. Ha nem pont egy, akkor exception

            context.MenuItems.AddOrUpdate(x => x.Name,
                new MenuItem(name: "Margarita", description: "Mozzarella, paradicsomszósz", price: 1000, category: pizzaCategory));
            context.MenuItems.AddOrUpdate(x => x.Name,
                new MenuItem("Hawaii", "Sonka, ananász, mozzarella, paradicsomszósz", 1300, pizzaCategory));
            //Ide mar nem kotelezo...
            context.SaveChanges();

            context.Locations.AddOrUpdate(x => x.Name, new Location() { Name = "Terasz", IsOutdoor = true }); //Igy, vagy, ha van ilyen konstruktor, akkor
            context.Locations.AddOrUpdate(x => x.Name, new Location("Belso terem", false)); //Igy is hasznalhato
            context.Locations.AddOrUpdate(x => x.Name, new Location("Nagy terem", false));
            context.SaveChanges();

            var outdoorLocation = context.Locations
                                        .Where(x => x.Name == "Terasz") //Az osszes sort visszaadja, amire igaz
                                                                        //.Single()
                                                                        //.First() ha ures a lista: hiba, amugy az elso elem
                                        .FirstOrDefault(); //ha ures a lista: null
            var indoorLocationBelsoTerem = context.Locations
                                        .Single(x => x.Name == "Belso terem");
            var indoorLocationNagyTerem = context.Locations
                                        .Single(x => x.Name == "Nagy terem");

            if (outdoorLocation == null || indoorLocationBelsoTerem == null || indoorLocationNagyTerem == null)
            {//ha nincs location peldany, akkor nem erdemes tovabbmenni
                throw new Exception($"Nincs megfelelo Location az adatbazisban");
            }

            context.Tables.AddOrUpdate(x => x.Name, new Table() { Name = "Bal-2", Location = outdoorLocation });
            context.Tables.AddOrUpdate(x => x.Name, new Table("Belso-1", indoorLocationBelsoTerem));
            context.Tables.AddOrUpdate(x => x.Name, new Table("Belso-2", indoorLocationBelsoTerem));
            context.Tables.AddOrUpdate(x => x.Name, new Table("Nagy-1", indoorLocationNagyTerem));
            context.SaveChanges();
        }
    }
}
