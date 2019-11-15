using BookStore.BackOffice.WebApi.BookStoreProperties;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BookStore.BackOffice.WebApi.BuildDocument
{
    public class DocumentProperties
    {
        private IPropertiesOfDataToInsert propertiesToInsert;

        public DocumentProperties(IPropertiesOfDataToInsert propertiesToInsert)
        {
            this.propertiesToInsert = propertiesToInsert;
        }

        public Paragraph PropertiesSetup()
        {
            var justification = new Justification() { Val = JustificationValues.Center };
            var paragraphMarkRunProperties = new ParagraphMarkRunProperties();

            var paragraphProperties = new ParagraphProperties();

            paragraphProperties.Append(justification);
            paragraphProperties.Append(paragraphMarkRunProperties);

            var run = new Run();
            var text = new Text();

            //text.Text = propertiesToInsert.HeaderCellNames.ToString();

            run.Append(new RunProperties());
            run.Append(text);

            var paragraph = new Paragraph();

            paragraph.Append(paragraphProperties);
            paragraph.Append(run);

            return paragraph;
        }
    }
}
