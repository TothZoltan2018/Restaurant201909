using System.ComponentModel.DataAnnotations;

namespace OopRestaurant201909.Models
{
    /// <summary>
    /// Az ettermen belul az egyes helyisegek
    /// </summary>
    public class Location
    {
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