using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.BackOffice.WebApi.Factory
{
	public class DataFilterFactory : IDataFilterFactory
	{
		private readonly BookStoreDbContext context;

		public DataFilterFactory(BookStoreDbContext context)
		{
			this.context = context;
		}

		public List<Book> GetFilteredResults(FilterModelDto filter)
		{
			Validate();

			var anyBook = GetAnyBookOfAuthor(filter);

			if (anyBook != null)
				return anyBook;

			switch (filter.FilterBy)
			{
				case FilterByEnums.aid:
					return FilterBasedOnAuthorId(filter);

				case FilterByEnums.year:
					return FilterBasedOnPublishYear(filter);

				default:
					throw new InvalidOperationException(Constants.InvalidFilterErr);
			}
		}

		private List<Book> GetAnyBookOfAuthor(FilterModelDto filter)
		{
			if (filter.FilterByValue <= 0)
				return context.Books.Include("Author").Select(x => x).ToList();

			return null;
		}

		private void Validate()
		{
			var any = context.Books.Any();

			if (any == false)
				throw new ArgumentNullException(Constants.ArgumentDataSourceNullErr);
		}

		// TODO: Move to new document filter service
		private List<Book> FilterBasedOnPublishYear(FilterModelDto filter)
		{
			switch (filter.Operators)
			{
				case Operator.GreaterThan:
					return context.Books.Include("Author").Where(a => a.PublicationYear > filter.FilterByValue).ToList();
				case Operator.GreaterThanOrEqual:
					return context.Books.Include("Author").Where(a => a.PublicationYear >= filter.FilterByValue).ToList();
				case Operator.LessThan:
					return context.Books.Include("Author").Where(a => a.PublicationYear < filter.FilterByValue).ToList();
				case Operator.LessThanOrEqual:
					return context.Books.Include("Author").Where(a => a.PublicationYear <= filter.FilterByValue).ToList();
				case Operator.Equal:
					return context.Books.Include("Author").Where(a => a.PublicationYear == filter.FilterByValue).ToList();
				default:
					throw new InvalidOperationException(Constants.InvalidOperatorErr);
			}
		}

		private List<Book> FilterBasedOnAuthorId(FilterModelDto filter)
		{
			var any = context.Books.Any(c => c.Author.Id == filter.FilterByValue);

			if (any == false)
				throw new ArgumentNullException(Constants.ArgumentNullErr);

			var book = context.Books.Include("Author").Where(a => a.Author.Id == filter.FilterByValue).ToList();

			if (book.Any() == false)
				throw new ArgumentNullException(Constants.ArgumentDataSourceNullErr);

			return book;
		}
	}
}
