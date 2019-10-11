using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OopRestaurant201909.Models
{
    public class Table
    {
        /// <summary>
        /// Primary Key mezo
        /// A Code First ebbol csinalja meg a DB identity mezot
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Az asztal azonositasa.pl. 23-as, jobb 2-es, B3-as...
        /// </summary>
        [Required] //Ket dolgot tesz: 1. Az adatbazisban kotelezo kitolteni. 2. A ViewModel-kent a feluleten is kotelzo kitolteni
        public string Name { get; set; }

        [Required]
        public Location Location { get; set; }
    }
}