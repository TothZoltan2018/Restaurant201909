using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Ezt a mezőt kötelező kitölteni!")]
        [Display(Name = "Megnevezés")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ezt a mezőt kötelező kitölteni!")]
        [Display(Name = "Leírás")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(1, 100000, ErrorMessage = "Hibás ár!")] //Ez csak az urlaprol erkezo adatkora ervenyes, azaz adatbazisba nem irodik be ez a megszoritas
        [Display(Name = "Ár")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Ezt a mezőt kötelező kitölteni!")]
        [Display(Name = "Kategória")]
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