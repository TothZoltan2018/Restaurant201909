using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace OopRestaurant201909
{
    /// <summary>
    /// Az etlaon szereplo tetelek koxul egynek az adatait tartalmazo osztaly
    /// </summary>
    public class MenuItem
    {
        public MenuItem(string name, string description, int price, Category category)
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
        }

        /// <summary>
        /// Az Entity Framwork-nek szuksege van erre a konstruktorra!
        /// </summary>
        public MenuItem() { }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Category Category { get; set; }
        /// <summary>
        /// A lenyilo lista kivalasztott elemenek az azonositoja reszere
        /// </summary>
        [NotMapped] //Ezzel mondjuk meg, hogy ez nem resze az adatbazisnak, ne akarja beleirni (Add-Migration, update-database...)
        public int CategoryId { get; set; }

        /// <summary>
        /// A lenyilo lista tartalma: azonosito es a megjelenitendo szoveg parok
        /// </summary>
        [NotMapped]
        public SelectList AssignableCategories { get; set; }

    }
}