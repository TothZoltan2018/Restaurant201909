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

        //[NotMapped] Most viszont igenis tegyuk a DB-be! (Ezzel a TablesController nagyon egyszeru lesz, az elozo commithoz kepest!)
        //Felhasznaljuk a Table-bol ide iranyulo kapcsolatot(FK), visszafele is
        //az EF automtikusan betolti az adott Location-hoz tartozo Table-ok listajat
        //Bovebben:
        //1. Az alabbi property is a EF CF fennhatosaga ala kerukl
        //2. Mivel az asztaltol az ide mutato iranyba mutat kapcsolat (egy asztal (Table) egyetlen teremhez (Location) tartozik)
        //3. ezert a teremhez visszafele is ki lehet gyujteni a hozza tartozo asztalokat.
        //Ezzel a property-vel ezt a letezo kapcsolatot felhasznalva elerhetove tesszuk az adott teremhez tartozo asztalokat
        public List<Table> Tables { get; set; } 
    }
}