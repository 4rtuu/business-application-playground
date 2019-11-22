using System.Collections.Generic;

namespace BookStore.BackOffice.WebApi.BookStorePropertiesDto
{
    public class PropertiesOfData : IPropertiesOfData
    {
        public List<BookAndItsAuthorsModelDto> BooksWithAuthorList { get; set; }

        public Dictionary<string, string> HeaderTitle { get; set; }

        public string[,] Data { get; set; }
    }
}
