using System.Collections.Generic;
using System.Linq;
using BookStore.BackOffice.WebApi.BookStoreProperties;
using BookStore.BackOffice.WebApi.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BookStore.BackOffice.WebApi.BuildDocument
{
    public class DocumentTable
    {
        private readonly BookStoreDbContext context;
        private IPropertiesOfDataToInsert propertiesToInsert;

        public DocumentTable(BookStoreDbContext context, IPropertiesOfDataToInsert propertiesToInsert)
        {
            this.context = context;
            this.propertiesToInsert = propertiesToInsert;
        }

        public Table TableSetup()
        {
            var table = SetupTableProperties();

            // not to count context.. build and use of "BuildBookListBasedOnCondition()" returned count
            propertiesToInsert.Data = new string
                [
                    propertiesToInsert.HeaderTitle.Count,
                    context.Books.Count() + 1
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

                for (int i = 0; i < propertiesToInsert.HeaderTitle.Count; i++)
                {
                    var cell = new TableCell();

                    cell.Append(
                        new Paragraph(
                            new Run(
                                new Text(
                                    propertiesToInsert.Data[i, j]
                    ))));

                    row.Append(cell);
                }
                table.Append(row);
            }

            return table;
        }

        private int BuildBookListBasedOnCondition()
        {
            // build a new property list of books to return based on conditional criteria
            // return the list.count to build data property
            return 0;
        }

        private void BuildDataHeader()
        {
            for (int i = 0; i < propertiesToInsert.HeaderTitle.Count; i++)
            {
                propertiesToInsert.Data[i, 0] = propertiesToInsert.HeaderTitle.ElementAt(i).Key;
            }
        }

        private void BuildDataBody()
        {
            context.Authors.ToList();

            for (int posX = 0; posX < propertiesToInsert.HeaderTitle.Count; posX++)
            {
                var bookId = 0;

                for (int posY = 0; posY < context.Books.Count() + 1; posY++)
                {
                    // sort conditional "BuildBookListBasedOnCondition()"
                    // count failure need to check count based on the condition criteria aswell otherwise loops for ever and either
                    // way would produce a table of empty rows because some books are skipped because of the given criteria
                    var sortCondition = context.Books.All(a => a.IsBestSeller == true);

                    var tblRep = context.Books.Where(x => x.Id == bookId).Select(b => b).FirstOrDefault();

                    bookId++;

                    if (tblRep == null && posY - 1 > 0)
                    {
                        posY -= 1;

                        continue;
                    }
                    else if (tblRep == null && posY - 1 <= 0)
                    {
                        posY = 0;

                        continue;
                    }

                    var value = tblRep.GetType()
                        .GetProperties()
                            .Where(a => a.Name == propertiesToInsert.HeaderTitle.ElementAt(posX).Value)
                                .Select(b => b.GetValue(tblRep, null))
                                    .FirstOrDefault();

                    var author = propertiesToInsert.HeaderTitle.ElementAt(posX).Value.ToLower();

                    if (value != null && propertiesToInsert.HeaderTitle.ElementAt(posX).Value.ToLower() == "author")
                        value = tblRep.Author.Firstname + " " + tblRep.Author.Lastname;

                    // Avoid broken doc in case the value is null
                    if (value == null)
                        value = "null";

                    propertiesToInsert.Data[posX, posY] = value.ToString();
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

            var props = new TableProperties(
                new TableBorders(
                    new TopBorder
                    {
                        Val = val,
                        Size = size
                    },
                    new BottomBorder
                    {
                        Val = val,
                        Size = size
                    },
                    new LeftBorder
                    {
                        Val = val,
                        Size = size
                    },
                    new RightBorder
                    {
                        Val = val,
                        Size = size
                    },
                    new InsideHorizontalBorder
                    {
                        Val = val,
                        Size = size
                    },
                    new InsideVerticalBorder
                    {
                        Val = val,
                        Size = size
                    }
                )
            );

            props.Append(tableWidth);

            table.AppendChild(props);

            return table;
        }
    }
}
