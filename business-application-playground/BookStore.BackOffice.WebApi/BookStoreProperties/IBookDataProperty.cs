using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.BookStoreProperties
{
    public interface IBookDataProperty
    {
        Book BookStore { get; set; }
    }
}