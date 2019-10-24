using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Framework.Helper;
using MainProject.ServiceAdmin.Model.Upload;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.IO;
using System;
using MainProject.ServiceAdmin.Model.MrCMSUpload;
using System.Collections.Generic;
using System.Web;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.Framework.Models;

namespace MainProject.ServiceAdmin.Service.Upload
{
    public class UploadService
    {
        private readonly LogHistoryService _logHistoryService;
        public UploadService(MainDbContext dbContext, string currentUser)
        {
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.Images);
        }

        /// <summary>
        /// Get index 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public UploadImageManageViewModel GetImages(string folder)
        {
            if (string.IsNullOrEmpty(folder)) return null;
            // Check folder is exist
            if (!FolderAndFileHelper.CheckFolderExist(folder))
            {
                FolderAndFileHelper.CreateFolder(folder);
            }
            // Read out files from the files directory
            var files = Directory.GetFiles(FolderAndFileHelper.GetMapPath(folder));
            // Bind folder to the model
            var images = new ImagesModel() { ImageFolder = folder, Images = new List<ImageInfo>() };
            // Bind file to model
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                images.Images.Add(new ImageInfo() { Link = folder + "/" + fileInfo.Name, Name = fileInfo.Name });
            }
            return new UploadImageManageViewModel()
            {
                FolderName = folder,
                Images = images,
            };
        }

        /// <summary>
        /// Delete images in folder
        /// </summary>
        public BaseResponseModel DeleteImage(ImageInfo model)
        {
            try
            {
                if (model != null && !string.IsNullOrEmpty(model.Link) 
                && FolderAndFileHelper.FileExist(FolderAndFileHelper.GetMapPath(model.Link)))
                {
                    FolderAndFileHelper.FileDelete(FolderAndFileHelper.GetMapPath(model.Link));
                    _logHistoryService.Create(new LogHistoryModel() { ActionType = ActionTypeCollection.Delete });

                    return new BaseResponseModel()
                    {
                        Code = HttpStatusCodeCollection.OK,
                        Message = "Đã xóa " + model.Link + " thành công!"
                    };
                }

            }
            catch (Exception ex) {
            }

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.BadRequest,
                Message = "Đã có lỗi xảy ra!"
            };
        }

        /// <summary>
        /// file post
        /// </summary>
        /// <param name="UlrFolder"></param>
        public List<ViewDataUploadFilesResult> Files_Post(string UrlFolder, HttpRequestBase Request)
        {
            var list = new List<ViewDataUploadFilesResult>();
            foreach (string files in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[files];
                if (MrCMSHelpers.IsValidFileType(file.FileName))
                {
                    var name = Path.GetFileNameWithoutExtension(file.FileName);
                    var model = new UploadImageManageViewModel()
                    {
                        FolderName = UrlFolder,
                        File = file
                    };
                    var imgPath = FolderAndFileHelper.GetFilePath(model.FolderName, model.File.FileName, name);

                    Request.Files[0].SaveAs(imgPath);

                    var dbFile = new ViewDataUploadFilesResult()
                    {
                        name = name,
                        url = imgPath,
                    };

                    list.Add(dbFile);
                }
            }
            return list;
        }

        /// <summary>
        /// Update imgaes 
        /// </summary>
        /// <param name="folder"></param>   
        public ImagesModel UpdateImage(string folder)
        {
            if (string.IsNullOrEmpty(folder)) return null;

            if (!Directory.Exists(FolderAndFileHelper.GetMapPath(folder)))
            {
                Directory.CreateDirectory(FolderAndFileHelper.GetMapPath(folder));
            }
            var images = new ImagesModel() { ImageFolder = folder };
            //Read out files from the files directory
            var files = Directory.GetFiles(FolderAndFileHelper.GetMapPath(folder));
            //Add them to the model

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                images.Images.Add(new ImageInfo() { Link = folder + "/" + fileInfo.Name, Name = fileInfo.Name });
            }
            return images;
        }
    }
}
