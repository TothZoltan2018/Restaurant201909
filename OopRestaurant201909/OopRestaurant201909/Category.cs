using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace OopRestaurant201909
{
    /// <summary>
    /// Az etelek kategoriajat tartalmazo osztaly
    /// </summary>
    public class Category
    {        
        /// <summary>
        /// A Configuration.cs hasznalja kategoria nev felvetelehez
        /// </summary>
        /// <param name="name"></param>
        public Category(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Az Entity Framwork-nek szuksege van erre a konstruktorra!
        /// </summary>
        public Category() { }
        

        /// <summary>
        /// Az Id nevbol a CodeFirst tudja, hogy ez majd primary key lesz
        /// </summary>
        public int Id { get; set; } 

        /// <summary>
        /// Az etel kategoria neve
        /// </summary>
        public string Name { get; set; }
    }
}