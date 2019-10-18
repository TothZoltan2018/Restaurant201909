using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OopRestaurant201909.Models
{
    /// <summary>
    /// Az ettermen belul az egyes helyisegek
    /// </summary>
    public class Location
    {
        public Location() {} //Alapertelemzett konstr az EF-nek

        public Location(string name, bool isOutdoor) //Ezzel konyebb az adatfeltoltes a Configuration.cs-ben
        {
            Name = name;
            IsOutdoor = isOutdoor;
        }
        /// <summary>
        /// PK mezo az adatbazishoz
        /// </summary>
        public int Id { get; set; }

        [Required]
        [Display(Name="Megnevezés")]
        public string Name { get; set; }

        /// <summary>
        /// Jelzi, hogy a helyiseg kulteri -e? true: kulteri 
        /// </summary>
        [Display(Name = "A szabadban van?")]
        public bool IsOutdoor { get; set; }

        //[NotMapped] Most viszont igenis tegyuk a DB-be! (Ezzel a TablesController nagzon egyszeru lesz, az elozo commithoz kepest!)
        //Felhasznaljuk a Table-bol ide iranyulo kapcsolatot(FK), visszafele is
        //az EF automtikusan betolti az adott Location-hoz tartozo Table-ok listajat
        public List<Table> Tables { get; set; } 
    }
}