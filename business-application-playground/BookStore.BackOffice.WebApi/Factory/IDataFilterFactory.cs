using System.Collections.Generic;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.Factory
{
	public interface IDataFilterFactory
	{
		List<Book> GetFilteredResults(FilterModelDto filter);
	}
}