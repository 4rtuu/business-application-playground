using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using System.IO;

namespace BookStore.BackOffice.WebApi.DocumentMapper
{
	public interface ICreateFile_Docx
	{
		MemoryStream CreateReport(FilterModelDto filter);
	}
}