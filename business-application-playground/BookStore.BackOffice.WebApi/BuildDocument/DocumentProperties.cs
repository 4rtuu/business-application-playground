using System.Collections.Generic;
using System.IO;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.BackOffice.WebApi.BuildDocument
{
    public class DocumentProperties : ControllerBase
    {
        private IPropertiesOfData propertiesToInsert;
        private DocumentData docData;
        private DocumentTable docTable;
        private DocumentSorting docFilter;

        public DocumentProperties(
            IPropertiesOfData propertiesToInsert,
            DocumentData docData,
            DocumentTable docTable,
            DocumentSorting docFilter
        )
        {
            this.propertiesToInsert = propertiesToInsert;
            this.docData = docData;
            this.docTable = docTable;
            this.docFilter = docFilter;
        }

        public void DocumentSetup(MemoryStream memory, FilterModelDto filter)
        {
            using(var document = WordprocessingDocument.Create(
                    memory,
                    WordprocessingDocumentType.Document,
                    true
                 )
            )
            {
                propertiesToInsert.HeaderTitle = new Dictionary<string, string>();

                docData.ToDictionary(filter.Titles);

                docFilter.ConditionalFiltering(filter);

                document.AddMainDocumentPart();

                var paragraphProperties = ParagraphSetup();

                var rdyTable = docTable.TableSetup();

                var body = new Body();
                body.Append(paragraphProperties);
                body.Append(rdyTable);

                var doc = new Document();
                doc.Append(body);

                document.MainDocumentPart.Document = doc;

                document.Close();
            }
        }

        public Paragraph ParagraphSetup()
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
