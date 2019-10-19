using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OopRestaurant201909.Models
{
    public class Table
    {
        public Table()
        {

        }

        public Table(string name, Location location)
        {
            Name = name;
            Location = location;
        }

        /// <summary>
        /// Primary Key mezo
        /// A Code First ebbol csinalja meg a DB identity mezot
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Az asztal azonositasa.pl. 23-as, jobb 2-es, B3-as...
        /// </summary>
        [Required] //Ket dolgot tesz: 1. Az adatbazisban kotelezo kitolteni. 2. A ViewModel-kent a feluleten is kotelzo kitolteni
        [Display(Name = "A sztal")]
        public string Name { get; set; }

        [Required]
        public Location Location { get; set; } //Foreign Key

        /// <summary>
        /// ViewModel, a lenyilomezo kivalasztott sora
        /// </summary>
        [NotMapped]
        public int LocationId { get; set; }

        /// <summary>
        /// ViewModel, a lenyilomezo tartalma
        /// </summary>
        [NotMapped]
        public SelectList AssignableLocations { get; set; }
    }
}