using System.Linq;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BookStore.BackOffice.WebApi.BuildDocument
{
    public class DocumentTable
    {
        private readonly BookStoreDbContext context;
        private IPropertiesOfData data;

        public DocumentTable(BookStoreDbContext context, IPropertiesOfData data)
        {
            this.context = context;
            this.data = data;
        }

        public Table TableSetup()
        {
            var table = SetupTableProperties();

            var test = data.BooksWithAuthorList;

            // not to count context.. build and use of "BuildBookListBasedOnCondition()" returned count
            data.Data = new string
                [
                    data.HeaderTitle.Count,
                    data.BooksWithAuthorList.Count + 1
                ];

            //populate header section
            BuildDataHeader();

            //populate body section
            BuildDataBody();

            table = DrawMatrix(table);

            return table;
        }

        private Table DrawMatrix(Table table)
        {
            for (int j = 0; j < context.Books.Count() + 1; j++)
            {
                var row = new TableRow();

                for (int i = 0; i < data.HeaderTitle.Count; i++)
                {
                    var cell = new TableCell();
                    
                    cell.Append(
                        new Paragraph(
                            new Run(
                                new Text(
                                    data.Data[i, j]
                    ))));

                    row.Append(cell);
                }
                table.Append(row);
            }

            return table;
        }

        private void BuildDataHeader()
        {
            for (int i = 0; i < data.HeaderTitle.Count; i++)
            {
                data.Data[i, 0] = data.HeaderTitle.ElementAt(i).Key;
            }
        }

        private void BuildDataBody()
        {
            context.Authors.ToList();

            for (int posX = 0; posX < data.HeaderTitle.Count; posX++)
            {
                var bookId = 0;

                for (int posY = 0; posY < data.BooksWithAuthorList.Count + 1; posY++)
                {
                    // sort conditional "BuildBookListBasedOnCondition()"
                    // count failure need to check count based on the condition criteria aswell otherwise loops for ever and either
                    // way would produce a table of empty rows because some books are skipped because of the given criteria
                    var sortCondition = context.Books.All(a => a.IsBestSeller == true);

                    // !!! Dto
                    var tblRep = context.Books.Where(x => x.Id == bookId).FirstOrDefault();

                    bookId++;

                    if (data.BooksWithAuthorList == null && posY - 1 > 0)
                    {
                        posY -= 1;

                        continue;
                    }
                    else if (data.BooksWithAuthorList == null && posY - 1 <= 0)
                    {
                        posY = 0;

                        continue;
                    }

                    var value = data.BooksWithAuthorList.GetType()
                        .GetProperties()
                            .Where(a => a.Name == data.HeaderTitle.ElementAt(posX).Value)
                                .Select(b => b.GetValue(data.BooksWithAuthorList, null))
                                    .FirstOrDefault();

                    var author = data.HeaderTitle.ElementAt(posX).Value.ToLower();

                    if (value != null && data.HeaderTitle.ElementAt(posX).Value.ToLower() == "author")
                        value = data.BooksWithAuthorList.Firstname + " " + data.BooksWithAuthorList..Lastname;

                    // Avoid broken doc in case the value is null
                    if (value == null)
                        value = "null";

                    data.Data[posX, posY] = value.ToString();
                }
            }
        }

        private Table SetupTableProperties()
        {
            // the table
            var table = new Table();

            // Make the table width 100% of the page width.
            var tableWidth = new TableWidth() { Width = "5000", Type = TableWidthUnitValues.Pct };

            var val = new EnumValue<BorderValues>(BorderValues.Single);
            var size = (UInt32Value)12;

            var insideVert = new InsideVerticalBorder { Val = val, Size = size };

            var insideHoriz = new InsideHorizontalBorder { Val = val, Size = size };

            var right = new RightBorder { Val = val, Size = size };

            var left = new LeftBorder { Val = val, Size = size };

            var bottom = new BottomBorder { Val = val, Size = size };

            var top = new TopBorder { Val = val, Size = size };

            var tableBorders = new TableBorders(
                top, bottom, 
                left, right, 
                insideHoriz, 
                insideVert
            );

            var props = new TableProperties(tableBorders);

            props.Append(tableWidth);

            table.AppendChild(props);

            return table;
        }
    }
}
