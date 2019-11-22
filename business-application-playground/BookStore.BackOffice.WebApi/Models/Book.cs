namespace BookStore.BackOffice.WebApi.Models
{
    public class Book
    {
        /// <summary>
        /// This is ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// This is Title
        /// </summary>
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public int PublicationYear { get; set; }
        public decimal Price { get; set; }
        public int AvailableStock { get; set; }
        public bool IsBestSeller { get; set; }
        public Author Author { get; set; }
    }
}