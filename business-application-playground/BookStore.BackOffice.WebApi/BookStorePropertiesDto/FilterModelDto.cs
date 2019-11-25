using System.ComponentModel.DataAnnotations;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.BookStorePropertiesDto
{
    public class FilterModelDto
    {
        /// <summary>
        /// defaults: "title", "author", "price", "best seller", "availability", "description", "year published"
        /// </summary>
        [Display(Name = "Titles", Description = "{ " +
            "\"title\", " +
            "\"author\", " +
            "\"price\", " +
            "\"best seller\", " +
            "\"availability\", " +
            "\"description\", " +
            "\"year published\" " +
        "}")]
        public string[] Titles { get; set; }

        [Display(Name = "Filter", Description = "Filter By")]
        public string FilterBy { get; set; }

        [Display(Name = "Operator", Description = "Comparison Operator")]
        public Operator Operators { get; set; }

        [Display(Name = "Value", Description = "Value of Filter")]
        public int FilterByValue { get; set; }
    }
}
