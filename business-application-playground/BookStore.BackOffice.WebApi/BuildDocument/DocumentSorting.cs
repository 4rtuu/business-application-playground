using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.BuildDocument
{
    public class DocumentSorting
    {
        private readonly BookStoreDbContext context;
        private IPropertiesOfData data;

        public DocumentSorting(BookStoreDbContext context, IPropertiesOfData data)
        {
            this.context = context;
            this.data = data;
        }

        public void ConditionalFiltering(FilterModelDto filter)
        {
            var response = Validate(filter);

            if (response == false)
                return;

            context.Authors.ToList();

            var books = FilterCaseHandler(filter);

            data.BooksWithAuthorList = new List<BookAndItsAuthorsModelDto>();

            MapToDto(books);
        }

        private bool Validate(FilterModelDto filter)
        {
            if (string.IsNullOrWhiteSpace(filter.FilterBy))
                return false;

            var any = context.Books.Any();

            if (any == false)
                throw new ArgumentNullException(Constants.ArgumentDataSourceNullErr);

            return true;
        }

        private List<Book> FilterCaseHandler(FilterModelDto filter)
        {
            var anyBook = GetAnyBookOfAuthor(filter);

            if (anyBook != null)
                return anyBook;

            anyBook = new List<Book>();

            switch (filter.FilterBy)
            {
                case Constants.AuthorId:
                    anyBook.Add(FilterBasedOnAuthorId(filter));

                    break;
                case Constants.PublishingYear:
                    anyBook = FilterBasedOnPublishYear(filter);

                    break;
                default:
                    throw new ArgumentException(Constants.ArgumentFilterErr);
            }

            return anyBook;
        }

        private List<Book> FilterBasedOnPublishYear(FilterModelDto filter)
        {
            switch (filter.Operators)
            {
                case Operator.GreaterThan:
                    return context.Books.Where(a => a.PublicationYear > filter.FilterByValue).ToList();
                case Operator.GreaterThanOrEqual:
                    return context.Books.Where(a => a.PublicationYear >= filter.FilterByValue).ToList();
                case Operator.LessThan:
                    return context.Books.Where(a => a.PublicationYear < filter.FilterByValue).ToList();
                case Operator.LessThanOrEqual:
                    return context.Books.Where(a => a.PublicationYear <= filter.FilterByValue).ToList();
                case Operator.Equal:
                    return context.Books.Where(a => a.PublicationYear == filter.FilterByValue).ToList();
                default:
                    throw new ArgumentException(Constants.ArgumentOperatorErr);
            }
        }

        // Filter base on provided id of Author Id
        private Book FilterBasedOnAuthorId(FilterModelDto filter)
        {
            var book = context.Books.FirstOrDefault(a => a.Author.Id == filter.FilterByValue);

            if (book == null)
                throw new ArgumentNullException(Constants.ArgumentDataSourceNullErr);

            return book;
        }

        private List<Book> GetAnyBookOfAuthor(FilterModelDto filter)
        {
            if (filter.FilterByValue <= 0) 
                return context.Books.Select(x => x).ToList();

            return null;
        }

        private void MapToDto(List<Book> books)
        {
            foreach (var book in books)
            {
                BuildDtoOfBook(book);
            }
        }

        private void BuildDtoOfBook(Book book)
        {
            var bookDto = new BookAndItsAuthorsModelDto()
            {
                Title = book.Title,
                Firstname = book.Author.Firstname,
                Lastname = book.Author.Lastname,
                Price = book.Price,
                IsBestSeller = book.IsBestSeller,
                AvailableStock = book.AvailableStock
            };

            data.BooksWithAuthorList.Add(bookDto);
        }
    }
}
