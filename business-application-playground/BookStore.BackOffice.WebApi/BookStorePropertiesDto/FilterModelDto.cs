using System.ComponentModel;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.BookStorePropertiesDto
{
    public class FilterModelDto
    {
        /// <summary>
        /// Customize table column names
        /// { "title", "author", "price", "best seller", "availability", "description", "year published" }
        /// Leave empty for default table layout
        /// </summary>
        [DisplayName("Title Block")]
        public string[] Titles { get; set; }

        /// <summary>
        /// Choose filter by author / year
        /// { "aid", "year" }
        /// Leave empty for full table list
        /// </summary>
        [DisplayName("Filter By")]
        public string FilterBy { get; set; }

        /// <summary>
        /// Choose Comparison operator in case sorting by release year
        /// GreaterThan db.val > FilterByValue;
        /// GreaterThanOrEqual db.val >= FilterByValue;
        /// LessThan db.val < FilterByValue;
        /// LessThanOrEqual db.val <= FilterByValue;
        /// Equal db.val == FilterByValue;
        /// </summary>
        [DisplayName("Comparison Operator")]
        public Operator Operators { get; }

        /// <summary>
        /// The value for which to search eg. aid = FilterByValue [ 8 ] / year = FilterByValue [ 1999 ]
        /// </summary>
        [DisplayName("Value of Filter")]
        public int FilterByValue { get; set; }
    }
}
