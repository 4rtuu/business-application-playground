using System.Collections.Generic;

namespace BookStore.BackOffice.WebApi.BookStoreProperties
{
    public class PropertiesOfDataToInsert : IPropertiesOfDataToInsert
    {
        public Dictionary<string, string> HeaderTitle { get; set; }

        public string[,] Data { get; set; }
    }
}
