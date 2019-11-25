using System;
using System.Linq;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BookStore.BackOffice.WebApi.BuildDocument
{
    public class DocumentTable
    {
        private IPropertiesOfData data;

        public DocumentTable(IPropertiesOfData data)
        {
            this.data = data;
        }

        public Table TableSetup()
        {
            var table = SetupTableProperties();

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
            for (int j = 0; j < data.BooksWithAuthorList.Count + 1; j++)
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
            for (int posX = 0; posX < data.HeaderTitle.Count; posX++)
            {
                for (int posY = 0; posY < data.BooksWithAuthorList.Count; posY++)
                {
                    var tblValue = data.BooksWithAuthorList[posY];

                    var element = data.HeaderTitle.ElementAt(posX).Value.ToLower();

                    var bookVal = MockDataOfCell(tblValue, element);

                    data.Data[posX, posY+1] = bookVal;
                }
            }
        }

        // TODO: move this to document sorting related stuff
        private string MockDataOfCell(BookAndItsAuthorsModelDto book, string element)
        {
            switch (element)
            {
                case "title":
                    return book.Title;

                case "author":
                    return book.Firstname + " " + book.Lastname;

                case "price":
                    return "€ " + book.Price;

                case "isbestseller":

                    if (book.IsBestSeller)
                    {
                        return "Bestseller";
                    }
                    else
                    {
                        return "Not Bestseller";
                    }

                case "availablestock":

                    if (book.AvailableStock <= 0)
                    {
                        return "Not available in stock";
                    }
                    else
                    {
                        return "Available in stock (" + book.AvailableStock + ")";
                    }

                default:
                    throw new ArgumentNullException(Constants.ArgumentDataSourceNullErr);
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
