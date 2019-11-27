using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Models;
using System;
using System.IO;

namespace BookStore.BackOffice.WebApi.DocumentMapper
{
	public class CreateFile_Pdf : ICreateFile_Pdf
	{

		public MemoryStream CreateReport(FilterModelDto filter)
		{
			throw new NotImplementedException(Constants.NotImplementedErr);
		}
	}
}
