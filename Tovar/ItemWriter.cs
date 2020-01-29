using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Tovar
{
    public static class ItemWriter
    {

        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (cell.CellReference.Value.Length == cellReference.Length)
                    {
                        if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                        {
                            refCell = cell;
                            break;
                        }
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                worksheet.Save();
                return newCell;
            }
        }

        private static void AddCell(string column, uint row, WorksheetPart worksheetPart, string value, CellValues dataType)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            Cell cell = InsertCellInWorksheet(column, row, worksheetPart);

            cell.CellValue = new CellValue(value);
            cell.DataType = dataType;
            worksheet.Save();
        }

        private static void MergeCells(WorksheetPart worksheetPart, string cell1Name, string cell2Name)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            MergeCells mergeCells;

            if (worksheet.Elements<MergeCells>().Count() > 0)
                mergeCells = worksheet.Elements<MergeCells>().First();
            else
            {
                mergeCells = new MergeCells();

                // Insert a MergeCells object into the specified position.  
                if (worksheet.Elements<CustomSheetView>().Count() > 0)
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<CustomSheetView>().First());
                else
                    worksheet.InsertAfter(mergeCells, worksheet.Elements<SheetData>().First());
            }

            // Create the merged cell and append it to the MergeCells collection.  
            MergeCell mergeCell = new MergeCell()
            {
                Reference =
                new StringValue(cell1Name + ":" + cell2Name)
            };
            mergeCells.Append(mergeCell);
            worksheet.Save();
        }

        public static void WriteItemsToExcelDocument(List<Item> items, string fileName)
        {
            SpreadsheetDocument document;
            try
            {
                document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                throw new Exception("Невозможно создать файл.");
            }
            WorkbookPart workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            WorksheetPart worksheetPart2 = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart2.Worksheet = new Worksheet(new SheetData());

            Sheets sheets = document.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

            Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Товар"};
            sheets.Append(sheet);

            Sheet sheet2 = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart2), SheetId = 2, Name = "Магазин" };
            sheets.Append(sheet2);

            AddCell("A", 1, worksheetPart, "Номер товара", CellValues.String);
            AddCell("B", 1, worksheetPart, "Штрихкод товара", CellValues.String);
            AddCell("C", 1, worksheetPart, "Артикул", CellValues.String);
            AddCell("D", 1, worksheetPart, "Наименование товара", CellValues.String);
            AddCell("E", 1, worksheetPart, "Единица измерения товара", CellValues.String);
            AddCell("F", 1, worksheetPart, "Название производителя товара", CellValues.String);
            AddCell("G", 1, worksheetPart, "Страна производителя", CellValues.String);
            AddCell("H", 1, worksheetPart, "Адрес производителя", CellValues.String);
            AddCell("I", 1, worksheetPart, "Телефон производителя", CellValues.String);
            AddCell("J", 1, worksheetPart, "Срок годности товара", CellValues.String);
            AddCell("K", 1, worksheetPart, "Дата изготовления товара", CellValues.String);
            AddCell("L", 1, worksheetPart, "Закупочная цена товара", CellValues.String);
            AddCell("M", 1, worksheetPart, "Моржа", CellValues.String);
            AddCell("N", 1, worksheetPart, "НДС 20%", CellValues.String);
            AddCell("O", 1, worksheetPart, "Продажная цена", CellValues.String);
            AddCell("P", 1, worksheetPart, "Количество товара", CellValues.String);

            for (int i = 0; i < items.Count; i++)
            {
                AddCell("A", (uint)i + 2, worksheetPart, (i + 1).ToString(), CellValues.Number);
                AddCell("B", (uint)i + 2, worksheetPart, items[i].Barcode, CellValues.String);
                AddCell("C", (uint)i + 2, worksheetPart, items[i].VendorCode, CellValues.String);
                AddCell("D", (uint)i + 2, worksheetPart, items[i].Name, CellValues.String);
                AddCell("E", (uint)i + 2, worksheetPart, items[i].Unit, CellValues.String);
                AddCell("F", (uint)i + 2, worksheetPart, items[i].ItemManufacturer.Name, CellValues.String);
                AddCell("G", (uint)i + 2, worksheetPart, items[i].ItemManufacturer.Country, CellValues.String);
                AddCell("H", (uint)i + 2, worksheetPart, items[i].ItemManufacturer.Address, CellValues.String);
                AddCell("I", (uint)i + 2, worksheetPart, items[i].ItemManufacturer.ManufacturerPhone.ToString(), CellValues.String);
                AddCell("J", (uint)i + 2, worksheetPart, items[i].ShelfLife, CellValues.String);
                AddCell("K", (uint)i + 2, worksheetPart, items[i].DateOfManufacturing.ToString("dd.MM.yyyy"), CellValues.Date);
                AddCell("L", (uint)i + 2, worksheetPart, items[i].ZakupPrice.ToString(), CellValues.Number);
                AddCell("M", (uint)i + 2, worksheetPart, items[i].Morzha.ToString(), CellValues.Number);
                AddCell("N", (uint)i + 2, worksheetPart, items[i].NDS.ToString(), CellValues.Number);
                AddCell("O", (uint)i + 2, worksheetPart, items[i].SellPrice.ToString(), CellValues.Number);
                AddCell("P", (uint)i + 2, worksheetPart, items[i].Count.ToString(), CellValues.Number);
            }

            AddCell("A", 1, worksheetPart2, "Производитель", CellValues.String);
            MergeCells(worksheetPart2, "A1", "D1");
            AddCell("E", 1, worksheetPart2, "Товар", CellValues.String);
            MergeCells(worksheetPart2, "E1", "O1");
            AddCell("A", 2, worksheetPart2, "Название", CellValues.String);
            AddCell("B", 2, worksheetPart2, "Страна производства", CellValues.String);
            AddCell("C", 2, worksheetPart2, "Адрес", CellValues.String);
            AddCell("D", 2, worksheetPart2, "Телефон", CellValues.String);
            AddCell("E", 2, worksheetPart2, "Штрихкод", CellValues.String);
            AddCell("F", 2, worksheetPart2, "Артикул", CellValues.String);
            AddCell("G", 2, worksheetPart2, "Наименование", CellValues.String);
            AddCell("H", 2, worksheetPart2, "Единица измерения", CellValues.String);
            AddCell("I", 2, worksheetPart2, "Срок годности", CellValues.String);
            AddCell("J", 2, worksheetPart2, "Дата изготовления", CellValues.String);
            AddCell("K", 2, worksheetPart2, "Закупочная цена", CellValues.String);
            AddCell("L", 2, worksheetPart2, "Моржа", CellValues.String);
            AddCell("M", 2, worksheetPart2, "НДС 20%", CellValues.String);
            AddCell("N", 2, worksheetPart2, "Продажная цена", CellValues.String);
            AddCell("O", 2, worksheetPart2, "Количество", CellValues.String);

            uint offset = 0;
            for (int i = 0; i < Shops.GetShops.Count; i++)
            {
                AddCell("A", (uint)i + 3 + offset, worksheetPart2, Shops.GetShops[i].Manufacturer.Name, CellValues.String);
                AddCell("B", (uint)i + 3 + offset, worksheetPart2, Shops.GetShops[i].Manufacturer.Country, CellValues.String);
                AddCell("C", (uint)i + 3 + offset, worksheetPart2, Shops.GetShops[i].Manufacturer.Address, CellValues.String);
                AddCell("D", (uint)i + 3 + offset, worksheetPart2, Shops.GetShops[i].Manufacturer.ManufacturerPhone.ToString(), CellValues.String);
                if (Shops.GetShops[i].Items.Count > 1)
                {
                    MergeCells(worksheetPart2, "A" + (i + 3 + offset).ToString(), "A" + (i + 2 + offset + Shops.GetShops[i].Items.Count).ToString());
                    MergeCells(worksheetPart2, "B" + (i + 3 + offset).ToString(), "B" + (i + 2 + offset + Shops.GetShops[i].Items.Count).ToString());
                    MergeCells(worksheetPart2, "C" + (i + 3 + offset).ToString(), "C" + (i + 2 + offset + Shops.GetShops[i].Items.Count).ToString());
                    MergeCells(worksheetPart2, "D" + (i + 3 + offset).ToString(), "D" + (i + 2 + offset + Shops.GetShops[i].Items.Count).ToString());
                }
                for (int j = 0; j < Shops.GetShops[i].Items.Count; j++)
                {
                    AddCell("E", (uint)(i + j + 3 + offset), worksheetPart2, Shops.GetShops[i].Items[j].Barcode, CellValues.String);
                    AddCell("F", (uint)(i + j + 3 + offset), worksheetPart2, Shops.GetShops[i].Items[j].VendorCode, CellValues.String);
                    AddCell("G", (uint)(i + j + 3 + offset), worksheetPart2, Shops.GetShops[i].Items[j].Name, CellValues.String);
                    AddCell("H", (uint)(i + j + 3 + offset), worksheetPart2, Shops.GetShops[i].Items[j].Unit, CellValues.String);
                    AddCell("I", (uint)(i + j + 3 + offset), worksheetPart2, Shops.GetShops[i].Items[j].ShelfLife, CellValues.String);
                    AddCell("J", (uint)(i + j + 3 + offset), worksheetPart2, Shops.GetShops[i].Items[j].DateOfManufacturing.ToString("dd.MM.yyyy"), CellValues.Date);
                    AddCell("K", (uint)(i + j + 3 + offset), worksheetPart2, Shops.GetShops[i].Items[j].ZakupPrice.ToString(), CellValues.Number);
                    AddCell("L", (uint)(i + j + 3 + offset), worksheetPart2, Shops.GetShops[i].Items[j].Morzha.ToString(), CellValues.Number);
                    AddCell("M", (uint)(i + j + 3 + offset), worksheetPart2, Shops.GetShops[i].Items[j].NDS.ToString(), CellValues.Number);
                    AddCell("N", (uint)(i + j + 3 + offset), worksheetPart2, Shops.GetShops[i].Items[j].SellPrice.ToString(), CellValues.Number);
                    AddCell("O", (uint)(i + j + 3 + offset), worksheetPart2, Shops.GetShops[i].Items[j].Count.ToString(), CellValues.Number);
                }
                offset += (uint)Shops.GetShops[i].Items.Count - 1;
            }

            workbookPart.Workbook.Save();

            document.Close();
        }

    }
}
