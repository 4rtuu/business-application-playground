using System.Collections.Generic;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Factory;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.DocumentMapper
{
	public class DocumentDtoMapper : IDocumentDtoMapper
	{
		private IDataFilterFactory filterFactory;

		public DocumentDtoMapper(IDataFilterFactory filterFactory)
		{
			this.filterFactory = filterFactory;
		}

		public List<BookAndItsAuthorsModelDto> MapToDto(FilterModelDto filter)
		{
			var books = filterFactory.GetFilteredResults(filter);

			var dtoList = new List<BookAndItsAuthorsModelDto>();

			foreach (var book in books)
			{
				dtoList.Add(BuildDtoOfBook(book));
			}

			return dtoList;
		}

		private BookAndItsAuthorsModelDto BuildDtoOfBook(Book book)
		{
			return new BookAndItsAuthorsModelDto()
			{
				Title = book.Title,
				Firstname = book.Author.Firstname,
				Lastname = book.Author.Lastname,
				Price = book.Price,
				IsBestSeller = book.IsBestSeller,
				AvailableStock = book.AvailableStock,
				PublicationYear = book.PublicationYear
			};
		}
	}
}
