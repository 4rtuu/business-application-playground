using Microsoft.AspNetCore.Mvc;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Factory;
using System;

namespace BookStore.BackOffice.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
		private IDocumentTypeFactory factory;


		public ValuesController(IDocumentTypeFactory factory)
        {
			this.factory = factory;
        }

        /// <summary>
        /// Generate a docx file.
        /// </summary>
        [HttpGet]
        public FileStreamResult Get([FromQuery] FilterModelDto filter)
        {
			var dateTime = DateTime.Now.ToString("dddd-dd-MMMM-yyyy-HH-mm-ss");

			var memoryStream = factory.GetFileStreamResult(filter);

			return new FileStreamResult
			(
				memoryStream,
				"application/vnd" +
				".openxmlformats-officedocument" +
				".wordprocessingml" +
				".document"
			)
			{
				FileDownloadName = "Book_Report-" + dateTime + ".doc"
			};
		}

        // POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        // PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
