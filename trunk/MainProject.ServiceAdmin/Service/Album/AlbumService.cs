using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Constant;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.Album;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Linq;

namespace MainProject.ServiceAdmin.Service.Album
{
    public class AlbumService
    {
        private readonly AlbumRepository _albumRepository;
        private readonly MediaRepository _mediaRepository;
        private readonly LanguageRepository _languageRepository;
        private readonly LogHistoryService _logHistoryService;
        private readonly int _itemPerPage = 15;

        public AlbumService(MainDbContext dbContext, string currentUser)
        {
            _albumRepository = new AlbumRepository(dbContext);
            _mediaRepository = new MediaRepository(dbContext);
            _languageRepository = new LanguageRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.Album);
        }

        public IndexViewModel<Core.Album> GetIndex(int languageId = 0, int page = 1)
        {
            languageId = languageId != 0 ? languageId : _languageRepository.FindAll().First().Id;
            if (page < 1) page = 1;
            // Get entity by language
            var sql = _albumRepository.Find(x => x.Language.Id == languageId);
            // Count total album
            int count = sql.Count();
            var albums = sql.OrderBy(d => d.Id).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList();

            return new IndexViewModel<Core.Album>()
            {
                ListItems = albums,
                FilterViewModel = new FilterViewModel()
                {
                    LanguageViewModel = new LanguageSelectModel()
                    {
                        Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), languageId),
                        LanguageSelectedValue = languageId
                    },
                    BaseUrl = "/Admin/AlbumAdmin/Index?"
                },
                PagingViewModel = new PagingModel(count, _itemPerPage, page,
                                        "href='/Admin/AlbumAdmin/Index?languageId=" + languageId + "&page={0}'")
            };
        }

        /// <summary>
        /// Initialize data for create view
        /// </summary>
        /// <returns></returns> 
        public GalleryManageViewModel Create()
        {
            var imageFolder = FolderAndFileHelper.GenerateFolder(FolderConstant.Gallery);

            return new GalleryManageViewModel
            {
                ImageFolder = imageFolder,
                Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), 0),
                LogHistoryId = _logHistoryService.Create(new LogHistoryModel()
                {
                    ActionType = ActionTypeCollection.Temp,
                    Comment = imageFolder
                }),
            };
        }

        /// <summary>
        /// Insert data to database
        /// </summary>
        /// <param name="model"></param>
        public void Insert(GalleryManageViewModel model)
        {
            var entity = new Core.Album
            {
                Language = _languageRepository.FindId(model.LanguageSelectedValue)
            };
            // Parse data from model to entity
            model.ToEntity(model, ref entity);
            // Insert entity into db
            _albumRepository.Insert(entity);
            // Check entity is album images
            if (!entity.IsVideo)
            {
                // Insert Media
                foreach (var media in model.Medias)
                {
                    var entityMedia = new Core.Media()
                    {
                        Album = entity
                    };
                    // Parse data from model to entity
                    media.ToEntity(media, ref entityMedia);
                    // Insert entity into db
                    _mediaRepository.Insert(entityMedia);
                }
            }

            // Update change logHistory from temp to create
            _logHistoryService.Update(model.LogHistoryId, entity.Id);
        }

        /// <summary>
        /// Get album for Edit View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponseModel<GalleryManageViewModel> Edit(long id)
        {
            // Get entity bind data to model
            var entity = _albumRepository.FindUnique(x => x.Id == id);
            // Check entity is exist
            if (entity == null)
            {
                return new BaseResponseModel<GalleryManageViewModel>
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Không tìm thấy album cần sửa</strong>")
                };
            }

            // Bind data to model
            var model = new GalleryManageViewModel(entity) {
                Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), entity.Language.Id),
                Medias = _mediaRepository.Find(x => x.Album.Id == id).ToList()
                                         .Select((x, posIndex) => new MediaManageViewModel(x) { Position = posIndex }).ToList()
            };

            return new BaseResponseModel<GalleryManageViewModel>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = model
            };
        }

        /// <summary>
        /// Update data album to databse
        /// </summary>
        /// <param name="model"></param>
        public BaseResponseModel Update(GalleryManageViewModel model)
        {
            // Get entity to update data
            var entity = _albumRepository.FindUnique(d => d.Id == model.Id);
            if (entity == null)
                return new BaseResponseModel()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Album không tồn tại!</strong>")
                };
            // Check entity is change form not video to video for deleting medias
            bool isVideo = entity.IsVideo == model.IsVideo;
            // Parse data from model to entity
            model.ToEntity(model, ref entity);
            // Save changes data album to entity
            _albumRepository.SaveChanges();
            // Check entity is album images
            if (isVideo)
            {
                if (model.Medias != null)
                {
                    // Insert Media
                    foreach (var media in model.Medias.Where(x => !x.IsDeleted))
                    {
                        // Get media does exist in db
                        var entityMedia = _mediaRepository.FindUnique(x => x.Id == media.Id);
                        // Create media
                        if (entityMedia == null)
                        {
                            entityMedia = new Core.Media()
                            {
                                Album = entity
                            };
                            // Parse data from model to entity
                            media.ToEntity(media, ref entityMedia);
                            // Insert entity into db
                            _mediaRepository.Insert(entityMedia);
                        }
                        else // Edit media
                        {
                            // Parse data from model to entity
                            media.ToEntity(media, ref entityMedia);
                            // Save change
                            _mediaRepository.SaveChanges();
                        }
                    }

                    // Delete media is removed
                    var listId = model.Medias.Where(x => x.IsDeleted).Select(y => y.Id);
                    _mediaRepository.DeleteByCriteria(x => x.Album.Id == entity.Id && listId.Contains(x.Id));
                }
                else // Delete all medias is removed
                {
                    _mediaRepository.DeleteByCriteria(x => x.Album.Id == entity.Id);
                }
            }
            else // Delete all medias by old isVideo entity
            {
                _mediaRepository.DeleteByCriteria(x => x.Album.Id == entity.Id);
            }

            // save logHistory 
            _logHistoryService.Create(new LogHistoryModel { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.OK,
                Message = string.Format("<strong style='color:green'>Đã cập nhật album thành công!</strong>")
            };
        }

        /// <summary>
        /// Delete data album form database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete(long id)
        {
            // Get entity to delete
            var entity = _albumRepository.FindUnique(x => x.Id == id);
            // Check album is exist
            if (entity == null)
                return string.Format("Có lỗi xảy ra không thể xóa được!");
            // Delete media
            _mediaRepository.DeleteByCriteria(x => x.Album.Id == id);
            // Delete album
            _albumRepository.Delete(entity);
            // Save history
            _logHistoryService.Insert(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Delete });
            // Delete image folder
            FolderAndFileHelper.DeleteFolder(entity.ImageFolder);

            return string.Format("<strong style='color:green'>Xóa album thành công!</strong>");
        }
    }
}
