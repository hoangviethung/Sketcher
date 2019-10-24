using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;

namespace MainProject.EPPlus
{
    public static class InportFileHelper
    {
        //https://riptutorial.com/epplus

        /// <summary>
        /// Create file excel with specified format
        /// </summary>
        /// <param name="path">Map path saved file</param>
        /// <param name="author">Created by</param>
        /// <param name="title">Title of Document</param>
        /// <param name="subject">EPPlus demo export data</param>
        public static void CreateExcelPackage(string path, string author, string title, string subject)
        {
            //Create a new ExcelPackage
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //Set some properties of the Excel document
                #region File detail
                excelPackage.Workbook.Properties.Author = author;
                excelPackage.Workbook.Properties.Title = title;
                excelPackage.Workbook.Properties.Subject = subject;
                excelPackage.Workbook.Properties.Created = DateTime.Now;
                #endregion


                #region Filling document with data
                // create a datatable
                DataTable dataTable = new DataTable();

                //add three colums to the datatable
                dataTable.Columns.Add("ID", typeof(int));
                dataTable.Columns.Add("Type", typeof(string));
                dataTable.Columns.Add("Name", typeof(string));

                //add some rows
                dataTable.Rows.Add(0, "Country", "Netherlands");
                dataTable.Rows.Add(1, "Country", "Japan");
                dataTable.Rows.Add(2, "Country", "America");
                dataTable.Rows.Add(3, "State", "Gelderland");
                dataTable.Rows.Add(4, "State", "Texas");
                dataTable.Rows.Add(5, "State", "Echizen");
                dataTable.Rows.Add(6, "City", "Amsterdam");
                dataTable.Rows.Add(7, "City", "Tokyo");
                dataTable.Rows.Add(8, "City", "New York");
                #endregion

                //Create the WorkSheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet 1");

                //add all the content from the DataTable, starting at cell A1
                worksheet.Cells["A1"].LoadFromDataTable(dataTable, true);

                #region Rich text
                // Set style for title cells
                RichText(worksheet.Cells["A1"].RichText.Add("This is my title"));
                #endregion

                #region Cell Width anh Height
                //optional use this to make all columms just a bit wider, text would sometimes still overflow after AutoFitColumns().
                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    worksheet.Column(col).Width = worksheet.Column(col).Width + 1;
                }
                #endregion

                //Save your file
                //FileInfo fi = new FileInfo(@"Path\To\Your\File.xlsx");
                FileInfo fi = new FileInfo(path);
                excelPackage.SaveAs(fi);
            }
        }

        private static void RichText(ExcelRichText title)
        {
            // Data Type:     bool
            // Default Value: false
            title.Bold = true;

            // Data Type:     System.Drawing.Color
            // Default Value: Color.Black
            title.Color = Color.Red;
            title.Color = Color.FromArgb(255, 0, 0);
            title.Color = ColorTranslator.FromHtml("#FF0000");

            // Data Type:     string
            // Default Value: "Calibri"
            title.FontName = "Verdana";

            // Data Type:     bool
            // Default Value: false
            title.Italic = true;

            // Data Type:     bool
            // Default Value: true
            // If this property is set to false, any whitespace (including new lines) 
            // is trimmed from the start and end of the Text
            title.PreserveSpace = true;

            // Data Type:     float
            // Default Value: 11
            // The font size is specified in Points
            title.Size = 16;

            // Data Type:     bool
            // Default Value: false
            // Strikethrough
            title.Strike = false;

            // Data Type:     string
            // Default Value: Whatever was set when the text was added to the RichText collection
            title.Text += " (updated)";

            // Data Type:     bool
            // Default Value: false
            title.UnderLine = true;

            // Data Type:     OfficeOpenXml.Style.ExcelVerticalAlignmentFont
            // Default Value: ExcelVerticalAlignmentFont.None
            title.VerticalAlign = ExcelVerticalAlignmentFont.None;
        }

        public static void OpenFile()
        {
            //Opening an existing Excel file
            FileInfo fi = new FileInfo(@"Path\To\Your\File.xlsx");
            using (ExcelPackage excelPackage = new ExcelPackage(fi))
            {
                //Get a WorkSheet by index. Note that EPPlus indexes are base 1, not base 0!
                ExcelWorksheet firstWorksheet = excelPackage.Workbook.Worksheets[1];

                //Get a WorkSheet by name. If the worksheet doesn't exist, throw an exeption
                ExcelWorksheet namedWorksheet = excelPackage.Workbook.Worksheets["SomeWorksheet"];

                //If you don't know if a worksheet exists, you could use LINQ,
                //So it doesn't throw an exception, but return null in case it doesn't find it
                ExcelWorksheet anotherWorksheet =
                    excelPackage.Workbook.Worksheets.FirstOrDefault(x => x.Name == "SomeWorksheet");

                //Get the content from cells A1 and B1 as string, in two different notations
                string valA1 = firstWorksheet.Cells["A1"].Value.ToString();
                string valB1 = firstWorksheet.Cells[1, 2].Value.ToString();

                //Save your file
                excelPackage.Save();
            }
        }
    }
}
