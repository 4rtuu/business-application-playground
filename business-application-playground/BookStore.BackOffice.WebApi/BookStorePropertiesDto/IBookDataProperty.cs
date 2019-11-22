using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.BookStorePropertiesDto
{
    public interface IBookDataProperty
    {
        Book BookStore { get; set; }
    }
}