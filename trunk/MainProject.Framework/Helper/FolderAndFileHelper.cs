using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;

namespace MainProject.Framework.Helper
{
    public static class FolderAndFileHelper
    {
        /// <summary>
        /// Check folder exist
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static bool CheckFolderExist(string folderName)
            => Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(folderName));

        /// <summary>
        /// Check file exist
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static bool FileExist(string file)
            => File.Exists(file);


        /// <summary>
        /// Delete folder
        /// </summary>
        /// <param name="folderName"></param>
        public static void DeleteFolder(string folderName)
        {
            if (CheckFolderExist(folderName))
            {
                Directory.Delete(System.Web.HttpContext.Current.Server.MapPath(folderName), true);
            }
        }

        /// <summary>
        /// Delete file exist
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static void FileDelete(string file)
            => File.Delete(file);


        /// <summary>
        /// Create folder
        /// </summary>
        /// <param name="folder"></param>
        public static void CreateFolder(string folder)
            => Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(folder));


        /// <summary>
        /// Generate Folder
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GenerateFolder(string url)
        {
            var folder = url + Guid.NewGuid().ToString();
            while (CheckFolderExist(folder))
            {
                folder = url + Guid.NewGuid().ToString();
            }

            return folder;
        }

        /// <summary>
        /// Get map path
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetMapPath(string url)
            => System.Web.HttpContext.Current.Server.MapPath(url);


        /// <summary>
        /// Gets an image from the specified URL.
        /// </summary>
        /// <param name="url">The URL containing an image.</param>
        /// <returns>The image as a bitmap.</returns>
        public static Bitmap GetImageFromUrl(string url)
        {
            var buffer = 1024;
            Bitmap image = null;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return image;

            using (var ms = new MemoryStream())
            {
                var req = WebRequest.Create(url);

                using (var resp = req.GetResponse())
                {
                    using (var stream = resp.GetResponseStream())
                    {
                        var bytes = new byte[buffer];
                        var n = 0;

                        while ((n = stream.Read(bytes, 0, buffer)) != 0)
                            ms.Write(bytes, 0, n);
                    }
                }

                image = Bitmap.FromStream(ms) as Bitmap;
            }

            return image;
        }


        /// <summary>
        /// Get file path
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetFilePath(string folder, string file, string name)
        {
            var imgPath = FolderAndFileHelper.GetMapPath(folder + "/" + name + Path.GetExtension(file));
            while (System.IO.File.Exists(imgPath))
            {
                if (name.Contains("-"))
                {
                    int n;
                    var str = name.Substring(name.LastIndexOf("-") + 1);
                    if (int.TryParse(str, out n))
                    {
                        n++;
                        name = name.Substring(0, name.LastIndexOf("-") + 1) + n;
                    }
                    else
                    {
                        name = name + "-1";
                    }
                }
                else
                {
                    name = name + "-1";
                }
                imgPath = FolderAndFileHelper.GetMapPath(folder + "/" + name + Path.GetExtension(file));
            }
            return imgPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetFilePath(string folder, HttpPostedFileBase model, string name)
        {
            var imgPath = HostingEnvironment.MapPath(folder + name + Path.GetExtension(model.FileName));
            while (System.IO.File.Exists(imgPath))
            {
                if (name.Contains("-"))
                {
                    int n;
                    var str = name.Substring(name.LastIndexOf("-") + 1);
                    if (int.TryParse(str, out n))
                    {
                        n++;
                        name = name.Substring(0, name.LastIndexOf("-") + 1) + n;
                    }
                    else
                    {
                        name = name + "-1";
                    }
                }
                else
                {
                    name = name + "-1";
                }
                imgPath = HostingEnvironment.MapPath(folder + name + Path.GetExtension(model.FileName));
            }
            return imgPath;
        }

        /// <summary>
        /// Get all images in Folder
        /// </summary>
        /// <param name="imageFolder"></param>
        /// <returns></returns>
        public static List<string> GetImages(string imageFolder, string[] imageExtensions = null)
        {
            var result = new List<string>();
            imageExtensions = imageExtensions ?? new[] { ".jpg", ".jpeg", ".gif", ".png" };
            if (!string.IsNullOrEmpty(imageFolder) && Directory.Exists(GetMapPath(imageFolder)))
            {

                var directory = new DirectoryInfo(GetMapPath(imageFolder));
                FileInfo[] files = directory.GetFiles();
                foreach (FileInfo file in files)
                {
                    if (imageExtensions.Contains(file.Extension.ToLower())) result.Add(imageFolder + "/" + file.Name);
                }
            }
            return result;
        }

        public static byte[] ReadAllBytes(string url)
            => File.ReadAllBytes(GetMapPath(url));
    }
}
