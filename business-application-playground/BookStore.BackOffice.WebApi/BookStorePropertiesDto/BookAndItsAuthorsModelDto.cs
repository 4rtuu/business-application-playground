using System.ComponentModel;

namespace BookStore.BackOffice.WebApi.BookStorePropertiesDto
{
    public class BookAndItsAuthorsModelDto : IBookAndItsAuthorsModelDto
    {
        [DisplayName("Name")]
        public string Firstname { get; set; }

        [DisplayName("Surname")]
        public string Lastname { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Description")]
        public string ShortDescription { get; set; }

        [DisplayName("Year Published")]
        public int PublicationYear { get; set; }

        [DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Availability")]
        public int AvailableStock { get; set; }

        [DisplayName("Best Seller")]
        public bool IsBestSeller { get; set; }
    }
}
