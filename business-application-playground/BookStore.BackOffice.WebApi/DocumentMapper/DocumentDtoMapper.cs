using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.DocumentService;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.DocumentMapper
{
	public class DocumentDtoMapper : IDocumentDtoMapper
	{
		private IDataFilter filter;

		public DocumentDtoMapper(IDataFilter filter)
		{
			this.filter = filter;
		}

		public List<BookAndItsAuthorsModelDto> MapToDto(FilterModelDto filterer)
		{
			var books = filter.GetFilteredResults(filterer);

            if (books.Any() == false)
                throw new InvalidOperationException(Constants.InvalidListErr);

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
