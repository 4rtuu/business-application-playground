using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.BackOffice.WebApi.BookStorePropertiesDto
{
    public class BookAndItsAuthorsModelDto : IBookAndItsAuthorsModelDto
    {
        [Display(Name = "Name", Description = "Name of author")]
        public string Firstname { get; set; }

        [Display(Name = "Surname", Description = "Last name of author")]
        public string Lastname { get; set; }

        [Display(Name = "Title", Description = "Title of the book")]
        public string Title { get; set; }

        [Display(Name = "Description", Description = "Description of the book")]
        public string ShortDescription { get; set; }

        [Display(Name = "Year Published", Description = "Year book was published")]
        public int PublicationYear { get; set; }

        [Display(Name = "Price", Description = "Price of the book")]
        public decimal Price { get; set; }

        [Display(Name = "Availability", Description = "Is book available")]
        public int AvailableStock { get; set; }

        [Display(Name = "Best Seller", Description = "Is book a best seller")]
        public bool IsBestSeller { get; set; }
    }
}
