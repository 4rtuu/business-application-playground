using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BookStore.BackOffice.WebApi.Models
{
    public class Constants
    {
        public static readonly ReadOnlyDictionary<string, string> TitleMap = 
            new ReadOnlyDictionary<string, string>(
                new Dictionary<string, string>()
                {
                    { "Title", "Title" },
                    { "Author", "Author" },
                    { "Price", "Price" },
                    { "Best Seller", "IsBestSeller" },
                    { "Availability", "AvailableStock" },
                }
            );

        public const string ArgumentFilterErr = "Error: The provided filter by argument is unhandled.";
        public const string ArgumentEmptyTitleErr = "Error: Something went wrong, argument empty.";

        public const string ArgumentDataSourceNullErr = "Error: Data source empty.";
        public const string ArgumentNullErr = "Error: The value does not exist.";
        
        public const string NullReferenceErr = "Object reference not set to an instance of an object.";

		public const string NotImplementedErr = "Such functionality has not yet been implemented!";

		public const string InvalidTypeErr = "Select a valid document type to creating a report!";
        public const string InvalidListErr = "Error: List contains no elements!";
		public const string InvalidFilterErr = "Select a valid filter to creating a report!";
		public const string InvalidOperatorErr = "Error: The provided operator is unhandled.";
    }
}
