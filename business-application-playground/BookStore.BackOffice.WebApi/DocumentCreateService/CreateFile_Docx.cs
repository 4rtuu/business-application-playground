using System.IO;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BookStore.BackOffice.WebApi.DocumentMapper
{
    public class CreateFile_Docx : ICreateFile_Docx
	{
        private CreateTable docTable;

		public CreateFile_Docx(CreateTable docTable)
        {
			this.docTable = docTable;
		}

        public MemoryStream CreateReport(FilterModelDto filter)
        {
			using(var memory = new MemoryStream())
			{
				using(var document = WordprocessingDocument.Create(
						memory,
						WordprocessingDocumentType.Document,
						true
					 )
				)
				{
					var doc = GetDocument(filter);

					document.AddMainDocumentPart();

					document.MainDocumentPart.Document = doc;

					document.Close();
				}

				return new MemoryStream(memory.ToArray());
			}
		}

		private Document GetDocument(FilterModelDto filter)
		{
			var paragraphProperties = ParagraphSetup();
			var rdyTable = docTable.Table(filter);

			var body = new Body();
			body.Append(paragraphProperties);
			body.Append(rdyTable);

			var doc = new Document();
			doc.Append(body);

			return doc;
		}

        private Paragraph ParagraphSetup()
        {
            var justification = new Justification() { Val = JustificationValues.Center };
            var paragraphMarkRunProperties = new ParagraphMarkRunProperties();

            var paragraphProperties = new ParagraphProperties();

            paragraphProperties.Append(justification);
            paragraphProperties.Append(paragraphMarkRunProperties);

            var run = new Run();
            var text = new Text();

            // some rand text as Main Title
            text.Text = "Book Invoice";

            run.Append(new RunProperties());
            run.Append(text);

            var paragraph = new Paragraph();

            paragraph.Append(paragraphProperties);
            paragraph.Append(run);

            return paragraph;
        }
    }
}
