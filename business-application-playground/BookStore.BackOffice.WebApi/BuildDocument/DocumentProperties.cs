using BookStore.BackOffice.WebApi.BookStoreProperties;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BookStore.BackOffice.WebApi.BuildDocument
{
    public class DocumentProperties
    {
        public Paragraph PropertiesSetup()
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
