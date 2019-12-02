using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BookStore.BackOffice.WebApi.DocumentMapper
{
    public interface ICreateTable
    {
        Table GetTable(FilterModelDto filter);
    }
}