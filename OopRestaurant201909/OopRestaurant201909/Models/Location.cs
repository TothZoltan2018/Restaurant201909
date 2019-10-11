using System.ComponentModel.DataAnnotations;

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
        public string Name { get; set; }

        /// <summary>
        /// Jelzi, hogy a helyiseg kulteri -e? true: kulteri 
        /// </summary>
        public bool IsOutdoor { get; set; }

    }
}