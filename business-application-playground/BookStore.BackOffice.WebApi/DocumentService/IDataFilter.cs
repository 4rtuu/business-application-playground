using System.Collections.Generic;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.DocumentService
{
	public interface IDataFilter
	{
		List<Book> GetFilteredResults(FilterModelDto filter);
	}
}