using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.BookStorePropertiesDto
{
    public class BookDataProperty : IBookDataProperty
    {
        public Book BookStore { get; set; }
    }
}
