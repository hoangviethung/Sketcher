using MainProject.EPPlus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace MainProject.Framework.Helper
{
    public static class ExcelHelper
    {
        private static string folder = "/Upload/ImportExcel";
        // https://support.office.com/en-gb/article/file-formats-that-are-supported-in-excel-0943ff2c-6014-4e8d-aaea-b83d51d46247
        private static string[] extensions = { ".xlsx", ".xlsm", ".xlsb", ".xltx", ".xltm", ".xls", ".xlt" };

        // The hosting or server need install driver to use this function
        public static IEnumerable<DataRow> Import(HttpPostedFileBase file)
        {
            if (extensions.Contains(Path.GetExtension(file.FileName)))
            {
                if (!FolderAndFileHelper.CheckFolderExist(folder))
                {
                    FolderAndFileHelper.CreateFolder(folder);
                }

                string fileLocation = FolderAndFileHelper.GetFilePath(folder, file, Path.GetFileNameWithoutExtension(file.FileName));
                file.SaveAs(fileLocation);

                var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0;",
                                                     fileLocation);

                var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                var ds = new DataSet();

                adapter.Fill(ds, "Sheet1");

                return ds.Tables["Sheet1"].AsEnumerable();
            }

            return null;
        }

        public static string Export()
        {
            if (!FolderAndFileHelper.CheckFolderExist(folder))
            {
                FolderAndFileHelper.CreateFolder(folder);
            }
            var filePath = "/Upload/ExportExcel/" + Guid.NewGuid() + ".xlsx";

            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES\"",
                                                FolderAndFileHelper.GetMapPath(filePath));

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;

                // nvarchar has max lenght is 255
                // Longtext for content larger than 255
                cmd.CommandText = "CREATE TABLE [table1] (id INT, name VARCHAR, datecol DATE );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO [table1](id,name,datecol) VALUES(1,'AAAA','2014-01-01');";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO [table1](id,name,datecol) VALUES(2, 'BBBB','2014-01-03');";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "INSERT INTO [table1](id,name,datecol) VALUES(3, 'CCCC','2014-01-03');";
                cmd.ExecuteNonQuery();

                cmd.CommandText = "UPDATE [table1] SET name = 'DDDD' WHERE id = 3;";
                cmd.ExecuteNonQuery();

                conn.Close();
            }

            return filePath;
        }

        public static string ExportEPPlus()
        {
            if (!FolderAndFileHelper.CheckFolderExist(folder))
            {
                FolderAndFileHelper.CreateFolder(folder);
            }
            var filePath = "/Upload/ExportExcel/" + Guid.NewGuid() + ".xlsx";

            InportFileHelper.CreateExcelPackage(filePath, "Web4g Solution", "Document", "");

            return filePath;
        }
    }
}
