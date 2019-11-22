namespace BookStore.BackOffice.WebApi.Models
{
    public class Constants
    {

        public static readonly string[] PrettyTitles = 
        {
            "title",
            "author",
            "price",
            "best seller",
            "availability"
        };

        public const string ArgumentFilterErr = "Error: The provided filter by argument is unhandled.";
        public const string ArgumentOperatorErr = "Error: The provided operator is unhandled.";
        public const string ArgumentEmptyTitleErr = "Error: Something went wrong, argument empty.";

        public const string ArgumentDataSourceNullErr = "Error: Data source empty.";
        public const string ArgumentNullErr = "Error: The value does not exist.";
        
        public const string NullReferenceErr = "Object reference not set to an instance of an object.";

        public const string AuthorId = "aid";
        public const string PublishingYear = "year";
    }
}
