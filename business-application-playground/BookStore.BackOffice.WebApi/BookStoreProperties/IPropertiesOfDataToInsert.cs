using System.Collections.Generic;

namespace BookStore.BackOffice.WebApi.BookStoreProperties
{
    public interface IPropertiesOfDataToInsert
    {
        string[,] Data { get; set; }
        Dictionary<string, string> HeaderTitle { get; set; }
    }
}