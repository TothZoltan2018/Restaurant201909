namespace OopRestaurant201909.Migrations
{
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

            context.Categories.AddOrUpdate( x => x.Name, new Category(name: "Pizzák"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Italok"));
            context.Categories.AddOrUpdate(x => x.Name, new Category(name: "Desszertek"));
            context.SaveChanges();
            
            var pizzaCategory = context.Categories.Single(x => x.Name == "Pizzák");
                        
            context.MenuItems.AddOrUpdate(x => x.Name,
                new MenuItem(name: "Margarita", description: "Mozzarella, paradicsomszósz", price: 100, category: pizzaCategory));
            context.MenuItems.AddOrUpdate(x => x.Name,
                new MenuItem(name: "Hawaii", description: "Sonka, ananász, mozzarella, paradicsomszósz", price: 100, category: pizzaCategory));
            //Ide mar nem kotelezo...
            context.SaveChanges();
        }
    }
}
