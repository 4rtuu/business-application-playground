using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace BookStore.BackOffice.WebApi.DocumentMapper
{
    public class CreateTable : ICreateTable
    {
        private IDocumentDtoMapper docMapper;

        public CreateTable(IDocumentDtoMapper docMapper)
        {
            this.docMapper = docMapper;
        }

        public Table GetTable(FilterModelDto filter)
        {
            var headerData = Constants.TitleMap;

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

        private Table DrawMatrix(Table table, string[,] matrix, List<BookAndItsAuthorsModelDto> bodyData, ReadOnlyDictionary<string, string> headerData)
        {
            for (int j = 0; j <= bodyData.Count; j++)
            {
                var row = new TableRow();

                for (int i = 0; i < headerData.Count; i++)
                {
                    var isHeader = j == 0 ? true : false;

                    var cell = GetCellWithStyling(isHeader, j);

                    var text = cell.ChildElements
                        .OfType<Paragraph>()
                        .Select(p => p.ChildElements
                            .OfType<Run>().First())
                        .Select(r => r.ChildElements
                            .OfType<Text>().First())
                        .First().Text = matrix[i, j];

                    row.Append(cell);
                }
                table.Append(row);
            }

            var newLine = new Paragraph(new Run(new Text(Environment.NewLine)));

            table.Append(newLine);

            return table;
        }

        private TableCell GetCellWithStyling(bool header, int row)
        {
            // MESS

            var cell = new TableCell();
            var paragraph = new Paragraph();
            var paraProperties = new ParagraphProperties();
            var spacing = new SpacingBetweenLines();
            var justification = new Justification();
            var run = new Run();
            var runProperties = new RunProperties();
            var cellColor = new Color();
            var text = new Text();
            var cellProperties = new TableCellProperties();
            var cellMargin = new TableCellMargin();
            var leftMargin = new LeftMargin();
            var rightMargin = new RightMargin();
            var cellVerticalAlignment = new TableCellVerticalAlignment();
            var cellWidth = new TableCellWidth();

            var shading = new Shading();

            if (header)
                justification.Val = JustificationValues.Center;
            else
                justification.Val = JustificationValues.Left;

            spacing.After = "80";
            spacing.Before = "80";
            leftMargin.Width = "140";
            rightMargin.Width = "140";

            cellMargin.Append(leftMargin);
            cellMargin.Append(rightMargin);

            cellColor.Val = "24292e";

            paraProperties.Append(justification);
            paraProperties.Append(spacing);
            paraProperties.Append(cellMargin);

            if (header)
                runProperties.Append(new Bold());

            runProperties.Append(cellColor);

            run.Append(runProperties);
            run.Append(text);

            cellVerticalAlignment.Val = TableVerticalAlignmentValues.Center;
            cellWidth.Type = TableWidthUnitValues.Auto;

            if (row % 2 == 0 && header == false)
            {
                shading.Color = "auto";
                shading.Fill = "F6F8FA";
                shading.Val = ShadingPatternValues.Clear;

                cellProperties.Append(shading);
            }

            cellProperties.Append(cellVerticalAlignment);
            cellProperties.Append(cellWidth);

            paragraph.Append(paraProperties);
            paragraph.Append(run);

            cell.Append(paragraph);
            cell.Append(cellProperties);

            return cell;
        }

        private string[,] BuildDataHeader(string[,] matrix, ReadOnlyDictionary<string, string> headerData)
        {
            for (int posX = 0; posX < headerData.Count; posX++)
            {
                matrix[posX, 0] = headerData.ElementAt(posX).Key;
            }

            return matrix;
        }

        private string[,] BuildDataBody(string[,] matrix, List<BookAndItsAuthorsModelDto> bodyData, ReadOnlyDictionary<string, string> headerData)
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

        private string MockDataOfCell(BookAndItsAuthorsModelDto book, string element)
        {
            switch (element)
            {
                case "title":
                    return book.Title + " (" + book.PublicationYear + ")";

                case "author":
                    return book.Firstname + " " + book.Lastname;

                case "price":
                    return "€ " + decimal.Round(book.Price, 2);

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
            var size = (UInt32Value)3;
            var color = "dfe2e5";

            var insideVert = new InsideVerticalBorder    { Val = val, Size = size, Color = color };
            var insideHoriz = new InsideHorizontalBorder { Val = val, Size = size, Color = color };
            var right = new RightBorder                  { Val = val, Size = size, Color = color };
            var left = new LeftBorder                    { Val = val, Size = size, Color = color };
            var bottom = new BottomBorder                { Val = val, Size = size, Color = color };
            var top = new TopBorder                      { Val = val, Size = size, Color = color };

            var tableBorders = new TableBorders(
                top, bottom,
                left, right,
                insideHoriz,
                insideVert
            );

            var props = new TableProperties();

            props.Append(tableWidth);
            props.Append(tableBorders);

            table.AppendChild(props);

            return table;
        }
    }
}
