using System;
using System.Linq;
using BookStore.BackOffice.WebApi.BookStoreProperties;
using BookStore.BackOffice.WebApi.BuildDocument;
using BookStore.BackOffice.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using BookStore.BackOffice.WebApi.HelperClasses;
using System.Collections.Generic;

namespace BookStore.BackOffice.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly BookStoreDbContext context;
        private IPropertiesOfDataToInsert propertiesToInsert;
        private IHelpers helpers;

        public ValuesController(BookStoreDbContext context, IPropertiesOfDataToInsert propertiesToInsert, IHelpers helpers)
        {
            this.context = context;
            this.propertiesToInsert = propertiesToInsert;
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
                    helpers.ToDictionary(titles);

                    document.AddMainDocumentPart();

                    var props = new DocumentProperties(propertiesToInsert);
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

        /// <summary>
        /// This is a get action result
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] PropertiesOfDataToInsert props)
        {
            try
            {
                var author = BuildAuthor(props);

                var authorInContext = context.Authors.FirstOrDefault(a => a.Firstname == props.Firstname && a.Lastname == props.Lastname);

                if (authorInContext == null)
                {
                    context.Authors.Add(author);
                }

                var book = BuildBook(props, author);

                var bookInContext = context.Books.FirstOrDefault(b => b.Title == props.Title && b.Author.Id == book.Author.Id);

                if (bookInContext == null)
                {
                    context.Books.Add(book);
                }

                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception($"Exception caught: {e}");
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        
        private Author BuildAuthor(PropertiesOfDataToInsert props)
        {
            return new Author
            {
                Firstname = props.Firstname,
                Lastname = props.Lastname
            };
        }

        private Book BuildBook(PropertiesOfDataToInsert props, Author author)
        {
            return new Book
            {
                Title = props.Title,
                ShortDescription = props.ShortDescription,
                PublicationYear = props.PublicationYear,
                Price = props.Price,
                AvailableStock = props.AvailableStock,
                IsBestSeller = props.IsBestSeller,
                Author = author
            };
        }
    }
}
