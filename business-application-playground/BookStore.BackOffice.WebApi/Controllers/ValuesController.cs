using System;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.BuildDocument;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly BookStoreDbContext context;
        private DocumentProperties docSettings;

        public ValuesController
        (
            BookStoreDbContext context,
            DocumentProperties docSettings
        )
        {
            this.context = context;
            this.docSettings = docSettings;
        }

        /// <summary>
        /// Generate a docx file of provided titles that are expected.
        /// </summary>
        [HttpGet]
        public IActionResult GetDocument([FromQuery] FilterModelDto filter)
        {
            var dateTime = DateTime.Now.ToString("dddd-dd-MMMM-yyyy-HH-mm-ss");

            using (var memory = new MemoryStream())
            {
                docSettings.DocumentSetup(memory, filter);

                return File
                (
                    memory.ToArray(),
                    "application/vnd" +
                    ".openxmlformats-officedocument" +
                    ".wordprocessingml" +
                    ".document",
                    "BookInvoice-" +
                    dateTime +
                    ".docx"
                );
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromQuery] BookDataProperty prop)
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
