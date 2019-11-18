using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.BookStoreProperties
{
    public class BookDataProperty : IBookDataProperty
    {
        public Book BookStore { get; set; }
    }
}
