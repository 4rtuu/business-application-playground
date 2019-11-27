using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using System.IO;

namespace BookStore.BackOffice.WebApi.Factory
{
	public interface IDocumentTypeFactory
	{
		MemoryStream GetFileStreamResult(FilterModelDto filter);
	}
}