using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BookStore.BackOffice.WebApi.DocumentMapper
{
    public class CreateTable
    {
		private IDocumentDtoMapper docMapper;
		private CreateDictionaryOfTitles dictOfTitles;

		public CreateTable(CreateDictionaryOfTitles dictOfTitles, IDocumentDtoMapper docMapper)
		{
			this.dictOfTitles = dictOfTitles;
			this.docMapper = docMapper;
		}

        public Table Table(FilterModelDto filter)
        {
			var headerData = dictOfTitles.MapTitles(filter);

			var bodyData = docMapper.MapToDto(filter);

			var table = SetupTableProperties();

            var matrix = new string
                [
					headerData.Count,
                    bodyData.Count + 2
                ];

            matrix = BuildDataHeader(matrix, headerData);

			matrix = BuildDataBody(matrix, bodyData, headerData);

            table = DrawMatrix(table, matrix, bodyData, headerData);

            return table;
        }

        private Table DrawMatrix(Table table, string[,] matrix, List<BookAndItsAuthorsModelDto> bodyData, Dictionary<string, string> headerData)
        {
            for (int j = 0; j <= bodyData.Count + 1; j++)
            {
                var row = new TableRow();

                for (int i = 0; i < headerData.Count; i++)
                {
                    var cell = new TableCell();
                    
                    cell.Append(
                        new Paragraph(
                            new Run(
                                new Text(
									matrix[i, j]
                    ))));

                    row.Append(cell);
                }
                table.Append(row);
            }

            return table;
        }

        private string[,] BuildDataHeader(string[,] matrix, Dictionary<string, string> headerData)
        {
            for (int i = 0; i < headerData.Count; i++)
            {
				matrix[i, 0] = headerData.ElementAt(i).Key;
            }

			return matrix;
        }

        private string[,] BuildDataBody(string[,] matrix, List<BookAndItsAuthorsModelDto> bodyData, Dictionary<string, string> headerData)
        {
            for (int posX = 0; posX < headerData.Count; posX++)
            {
                for (int posY = 0; posY < bodyData.Count; posY++)
                {

					var tblValue = bodyData[posY];

					var element = headerData.ElementAt(posX).Value.ToLower();

                    var bookVal = MockDataOfCell(tblValue, element);

					matrix[posX, posY + 1] = bookVal;
                }
            }

			return matrix;
        }

        // TODO: move this to document sorting related stuff
        private string MockDataOfCell(BookAndItsAuthorsModelDto book, string element)
        {
            switch (element)
            {
                case "title":
					return book.Title + " (" + book.PublicationYear + ")";

                case "author":
                    return book.Firstname + " " + book.Lastname;

                case "price":
                    return "€ " + book.Price;

                case "isbestseller":

                    if (book.IsBestSeller)
                        return "Bestseller";

                    else
                        return "Not Bestseller";

                case "availablestock":

                    if (book.AvailableStock <= 0)
                        return "Not available in stock";

                    else
                        return "Available in stock (" + book.AvailableStock + ")";

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
