using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using BookStore.BackOffice.WebApi.BookStoreProperties;
using BookStore.BackOffice.WebApi.BuildDocument;
using BookStore.BackOffice.WebApi.Models;
using BookStore.BackOffice.WebApi.HelperClasses;

namespace BookStore.BackOffice.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly BookStoreDbContext context;
        private IPropertiesOfDataToInsert propertiesToInsert;
        private IBookDataProperty bookDataProperty;
        private IHelpers helpers;

        public ValuesController(BookStoreDbContext context, IPropertiesOfDataToInsert propertiesToInsert, IBookDataProperty bookDataProperty, IHelpers helpers)
        {
            this.context = context;
            this.propertiesToInsert = propertiesToInsert;
            this.bookDataProperty = bookDataProperty;
            this.helpers = helpers;
        }

        /// <summary>
        /// Defaults { | "title", "author", "price", "isbestseller", "availablestock", | "shortdescription", "publicationyear" }
        /// Generate a docx file of provided titles that are expected.
        /// </summary>
        [HttpGet]
        public IActionResult GenerateDocument([FromQuery] string[] titles)
        {
            using (var memory = new MemoryStream())
            {
                using (var document = WordprocessingDocument.Create(memory, WordprocessingDocumentType.Document, true))
                {
                    propertiesToInsert.HeaderTitle = new Dictionary<string, string>();

                    bookDataProperty.BookStore = new Book();

                    helpers.ToDictionary(titles);

                    document.AddMainDocumentPart();

                    var props = new DocumentProperties();
                    var paragraphProperties = props.PropertiesSetup();

                    var table = new DocumentTable(context, propertiesToInsert);
                    var rdyTable = table.TableSetup();

                    var body = new Body();
                    body.Append(paragraphProperties);
                    body.Append(rdyTable);

                    var doc = new Document();
                    doc.Append(body);

                    document.MainDocumentPart.Document = doc;

                    document.Close();
                }

                return File(memory.ToArray(), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "BookInvoice.docx");
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] BookDataProperty prop)
        {
            try
            {
                var author = context.Authors.FirstOrDefault(a => a.Firstname == prop.BookStore.Author.Firstname &&
                                                                 a.Lastname == prop.BookStore.Author.Lastname);

                var book = context.Books.FirstOrDefault(b => b.Title == prop.BookStore.Title);

                if (author == null)
                {
                    author = BuildAuthor(prop);
                }

                if (book == null)
                {
                    book = BuildBook(prop, author);

                    context.Books.Add(book);
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Exception caught: {e}");
            }
        }

        private Author BuildAuthor(BookDataProperty prop)
        {
            return new Author
            {
                Firstname = prop.BookStore.Author.Firstname,
                Lastname = prop.BookStore.Author.Lastname
            };
        }

        private Book BuildBook(BookDataProperty prop, Author author)
        {
            return new Book
            {
                Title = prop.BookStore.Title,
                ShortDescription = prop.BookStore.ShortDescription,
                PublicationYear = prop.BookStore.PublicationYear,
                Price = prop.BookStore.Price,
                AvailableStock = prop.BookStore.AvailableStock,
                IsBestSeller = prop.BookStore.IsBestSeller,
                Author = author
            };
        }

        ///// <summary>
        ///// This is a get action result
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
