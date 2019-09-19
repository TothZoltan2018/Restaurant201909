using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace OopRestaurant201909
{
    /// <summary>
    /// Az etlaon szereplo tetelek koxul egynek az adatait tartalmazo osztaly
    /// </summary>
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Category Category { get; set; }

    }
}