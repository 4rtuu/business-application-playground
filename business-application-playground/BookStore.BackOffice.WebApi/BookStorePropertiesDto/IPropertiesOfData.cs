using System.Collections.Generic;

namespace BookStore.BackOffice.WebApi.BookStorePropertiesDto
{
    public interface IPropertiesOfData
    {
        string[,] Data { get; set; }
        Dictionary<string, string> HeaderTitle { get; set; }
        List<BookAndItsAuthorsModelDto> BooksWithAuthorList { get; set; }
    }
}