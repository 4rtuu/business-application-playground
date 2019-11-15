using System.Collections.Generic;

namespace BookStore.BackOffice.WebApi.BookStoreProperties
{
    public interface IPropertiesOfDataToInsert
    {
        string[,] Data { get; set; }
        Dictionary<string, string> HeaderTitle { get; set; }
        int AvailableStock { get; set; }
        string Firstname { get; set; }
        bool IsBestSeller { get; set; }
        string Lastname { get; set; }
        decimal Price { get; set; }
        int PublicationYear { get; set; }
        string ShortDescription { get; set; }
        string Title { get; set; }
    }
}