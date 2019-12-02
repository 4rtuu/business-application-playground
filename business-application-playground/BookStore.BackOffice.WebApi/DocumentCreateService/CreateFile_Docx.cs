using System.IO;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BookStore.BackOffice.WebApi.DocumentMapper
{
    public class CreateFile_Docx : ICreateFile_Docx
	{
        private ICreateTable docTable;

		public CreateFile_Docx(ICreateTable docTable)
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
					document.AddMainDocumentPart();

                    var doc = GetDocument(filter);

                    document.MainDocumentPart.Document = doc;

					document.Close();
				}

				return new MemoryStream(memory.ToArray());
			}
		}

		private Document GetDocument(FilterModelDto filter)
		{
			var paragraphProperties = ParagraphSetup();
			var rdyTable = docTable.GetTable(filter);

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

			var bold = new Bold();
			var fontSize = new FontSize();
			var runPara = new RunProperties();
			var run = new Run();
            var text = new Text();

			fontSize.Val = "36";
			//some rand text as Main Title
			text.Text = "Book Report\n";

			runPara.Append(bold);
			runPara.Append(fontSize);
			run.Append(runPara);
			run.Append(text);

            var paragraph = new Paragraph();

            paragraph.Append(paragraphProperties);
			paragraph.Append(run);

			return paragraph;
        }
    }
}
