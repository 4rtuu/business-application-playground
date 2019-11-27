using System;
using System.IO;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.DocumentMapper;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.Factory
{
	public class DocumentTypeFactory : IDocumentTypeFactory
	{
		private ICreateFile_Docx createFileDocx;
		private ICreateFile_Pdf createFilePdf;

		public DocumentTypeFactory(ICreateFile_Docx createFileDocx, ICreateFile_Pdf createFilePdf)
		{
			this.createFileDocx = createFileDocx;
			this.createFilePdf = createFilePdf;
		}

		public MemoryStream GetFileStreamResult(FilterModelDto filter)
		{
			switch(filter.Type)
			{
				case Document.Docx:
					return createFileDocx.CreateReport(filter);

				case Document.Pdf:
					return createFilePdf.CreateReport(filter);

				default:
					throw new InvalidOperationException(Constants.InvalidTypeErr);
			}
		}
	}
}
