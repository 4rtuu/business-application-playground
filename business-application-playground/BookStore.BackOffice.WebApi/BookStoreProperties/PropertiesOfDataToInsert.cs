using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace BookStore.BackOffice.WebApi.BookStoreProperties
{
    public class PropertiesOfDataToInsert : IPropertiesOfDataToInsert
    {
        /// <summary>
        /// Author Name
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// Author Surname
        /// </summary>
        public string Lastname { get; set; }

        /// <summary>
        /// Book Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Book Description
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Year book was published
        /// </summary>
        public int PublicationYear { get; set; }

        /// <summary>
        /// Price of the book
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Book availability/ in stock
        /// </summary>
        public int AvailableStock { get; set; }

        /// <summary>
        /// Is book a bestseller
        /// </summary>
        public bool IsBestSeller { get; set; }

        public Dictionary<string, string> HeaderTitle { get; set; }

        public string[,] Data { get; set; }
    }
}
