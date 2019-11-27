using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Models;
using System.Collections.Generic;

namespace BookStore.BackOffice.WebApi.DocumentMapper
{
	public interface IDocumentDtoMapper
	{
		List<BookAndItsAuthorsModelDto> MapToDto(FilterModelDto filter);
	}
}