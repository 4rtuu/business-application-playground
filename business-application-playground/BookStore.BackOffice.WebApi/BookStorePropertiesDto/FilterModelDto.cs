using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using BookStore.BackOffice.WebApi.Models;
using Newtonsoft.Json;

namespace BookStore.BackOffice.WebApi.BookStorePropertiesDto
{
	public class FilterModelDto
	{
        [Required]
        public Document Type { get; set; }

        public int? AuthorId { get; set; }

        public int? YearBefore { get; set; }

        public int? YearAfter { get; set; }

        public bool? IfIsBestSeller { get; set; }
	}
}
