using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.BackOffice.WebApi.DocumentService
{
	public class DataFilter : IDataFilter
	{

		private readonly BookStoreDbContext context;

		public DataFilter(BookStoreDbContext context)
		{
			this.context = context;
		}

		public List<Book> GetFilteredResults(FilterModelDto filter)
		{
            var resultsQuery = context.Books.Include("Author");

            if (filter.AuthorId != null)
                resultsQuery = resultsQuery.Where(a => a.Author.Id == filter.AuthorId);
            
            if (filter.YearBefore != null)
                resultsQuery = resultsQuery.Where(a => a.PublicationYear <= filter.YearBefore);
            
            if (filter.YearAfter != null)
                resultsQuery = resultsQuery.Where(a => a.PublicationYear >= filter.YearAfter);
            
            if (filter.IfIsBestSeller != null)
                resultsQuery = resultsQuery.Where(a => a.IsBestSeller == filter.IfIsBestSeller);

            return resultsQuery.ToList();

            //return context.Books.Include("Author").Where(b =>
            //    (filter.AuthorId == null || filter.AuthorId == b.Author.Id) &&
            //    (filter.YearBefore == null || filter.YearBefore >= b.PublicationYear) &&
            //    (filter.YearAfter == null || filter.YearAfter <= b.PublicationYear) &&
            //    (filter.IfIsBestSeller == null || filter.IfIsBestSeller == b.IsBestSeller)
            //).ToList();
        }
	}
}
