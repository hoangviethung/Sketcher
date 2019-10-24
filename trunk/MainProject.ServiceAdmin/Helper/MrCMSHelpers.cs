using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MainProject.ServiceAdmin.Helper
{
    public static class MrCMSHelpers
    {
        private static readonly List<string> ImageExtensions = new List<string> { ".jpg", ".jpeg", ".gif", ".png", ".bmp" };

        public static bool IsValidFileType(string fileName)
        {
            var mediaAllowedFileTypeList = ".gif,.jpeg,.jpg,.png,.rar,.zip,.pdf,.mp3,.mp4,.wmv,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.avi,.mpg,.wav,.mov,.wma,.webm,.ogv,.mpeg,.flv,.7z,.txt,.csv,.html,.htm";
            string extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension) || extension.Length < 1)
                return false;
            return Array.ConvertAll(mediaAllowedFileTypeList.Split(','), p => p.Trim()).Contains(extension, StringComparer.OrdinalIgnoreCase);
        }
    }
}
