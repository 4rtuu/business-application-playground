namespace BookStore.BackOffice.WebApi.BookStorePropertiesDto
{
    public interface IBookAndItsAuthorsModelDto
    {
        string Firstname { get; set; }
        string Lastname { get; set; }
        int AvailableStock { get; set; }
        bool IsBestSeller { get; set; }
        decimal Price { get; set; }
        string Title { get; set; }
        int PublicationYear { get; set; }
    }
}