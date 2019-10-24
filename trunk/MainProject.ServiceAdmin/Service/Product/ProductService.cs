using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Model.Product;
using MainProject.ServiceAdmin.Service.LogHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Service.Product
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly PropertyRepository _propertyRepository;
        private readonly CommerceCategoryRepository _commerceCategoryRepository;
        private readonly ProductCommerceCategoryRefRepository _productCommerceCategoryRefRepository;
        private readonly ProductPropertyRefRepository _productPropertyRefRepository;
        private readonly LogHistoryService _logHistoryService;
        private readonly int _itemPerPage = 10;

        public ProductService(MainDbContext dbContext)
        {
            _productRepository = new ProductRepository(dbContext);
            _propertyRepository = new PropertyRepository(dbContext);
            _commerceCategoryRepository = new CommerceCategoryRepository(dbContext);
            _productCommerceCategoryRefRepository = new ProductCommerceCategoryRefRepository(dbContext);
            _productPropertyRefRepository = new ProductPropertyRefRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.CommerceProduct);
        }

        /// <summary>
        /// Filter data for Index View
        /// </summary>
        /// <param name="fa">Commerce product category id</param>
        /// <param name="title">Product title</param>
        /// <param name="page">current page</param>
        /// <returns></returns>
        public IndexViewModel<Core.Commerce.Product> GetIndex(long fa = 0, string title = null, int page = 1)
        {
            // Set default data in case some error occured
            if (fa < 0) fa = 0;
            if (page < 1) page = 1;
            // Get all product
            var sql = _productRepository.FindAll();

            // Filter product by commerce product category
            if (fa != 0)
            {
                sql = _productCommerceCategoryRefRepository.Find(x => x.CommerceCategory.Id == fa).Select(x => x.Product).Distinct();
            }

            // Search product by title or product code
            if (title != null)
            {
                sql = sql.Where(d => d.Name.Contains(title) || d.ProductCode.Contains(title));
            }
            // Count total product
            var count = sql.Count();
            var products = sql.OrderByDescending(x => x.Id).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList();

            return new IndexViewModel<Core.Commerce.Product>()
            {
                ListItems = products,
                FilterViewModel = new FilterViewModel()
                {
                    FatherSelectModel = new FatherSelectModel()
                    {
                        Fathers = CommerceCategoryHelper.BindSelectListItem(_commerceCategoryRepository, fa, isContainDefault: true),
                        FatherSelectedValue = fa
                    },
                    Title = title ?? "",
                    HasFatherSelect = true,
                    BaseUrl = "/Admin/ProductAdmin/Index?"
                },
                PagingViewModel = new PagingModel(count, _itemPerPage, page, "href='/Admin/ProductAdmin/Index?fa=" + fa + "&title=" + title + "&page={0}'")
            };
        }

        /// <summary>
        /// Prepare data for Create View
        /// </summary>
        /// <returns></returns>
        public ProductManageModel Create()
        {
            // Create folder contain image
            var imageFolder = FolderAndFileHelper.GenerateFolder("/Upload/Product/");

            // Initialize product for View
            var model = new ProductManageModel()
            {
                ImageFolder = imageFolder,
                // Save log history,
                LogHistoryId = _logHistoryService.Create(new LogHistoryModel()
                {
                    ActionType = ActionTypeCollection.Temp,
                    Comment = imageFolder
                })
            };
            // Bind reference data to select list
            InitExtDataViewModel(model);

            return model;
        }

        /// <summary>
        /// Insert data into db
        /// </summary>
        /// <param name="model"></param>
        public BaseResponseModel Insert(ProductManageModel model)
        {
            // Bind data from model to entity
            var entity = new Core.Commerce.Product()
            {
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };
            ProductManageModel.ToEntity(model, ref entity);
            // Save product
            _productRepository.Insert(entity);

            // Save commerce product category reference to product
            ProductHelper.SaveProductCategoryRef(model, entity, _commerceCategoryRepository, _productCommerceCategoryRefRepository);
            // Save properties reference to product
            ProductHelper.SaveProductPropertyRefs(model, entity, _propertyRepository, _productPropertyRefRepository);

            _productPropertyRefRepository.SaveChanges();
            _productRepository.SaveChanges();
            // Save log History
            _logHistoryService.Update(model.LogHistoryId, entity.Id);

            return new BaseResponseModel
            {
                Code = HttpStatusCodeCollection.OK,
                Message = string.Format("<strong style='color:green'>Thêm thành công!</strong>")
            };
        }

        public PropertyManageModel CreateProperty(int pos, List<long> selectedIds)
            => new PropertyManageModel
            {
                Position = pos,
                Properties = _propertyRepository.FindAll().OrderBy(x => x.Order)
                                                 .Select(x => new SelectListItem
                                                 {
                                                     Text = x.Name,
                                                     Value = x.Id.ToString(),
                                                     Disabled = selectedIds.Contains(x.Id)
                                                 }).ToList(),
            };

        public BaseResponseModel<ProductManageModel> Edit(long id)
        {
            // Get entity bind data to model
            var entity = _productRepository.FindUnique(x => x.Id == id);
            // Check entity is exist
            if (entity == null)
                return new BaseResponseModel<ProductManageModel>
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("<strong style='color:red'>Không tìm thấy đối tượng cần chỉnh sửa !</strong>")
                };
            var model = new ProductManageModel(entity);
            // Bind reference data to select list
            InitExtDataViewModel(model);

            return new BaseResponseModel<ProductManageModel>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = model
            };
        }

        /// <summary>
        /// Update product into db
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public BaseResponseModel Update(ProductManageModel model)
        {
            var entity = _productRepository.FindUnique(x => x.Id == model.Id);
            if (entity == null)
            {
                return new BaseResponseModel
                {
                    Code = HttpStatusCodeCollection.BadRequest
                };
            }
            ProductManageModel.ToEntity(model, ref entity);
            entity.UpdatedDate = DateTime.Now;

            // Save commerce product category reference to product
            ProductHelper.SaveProductCategoryRef(model, entity, _commerceCategoryRepository, _productCommerceCategoryRefRepository);
            // Save properties reference to product
            ProductHelper.SaveProductPropertyRefs(model, entity, _propertyRepository, _productPropertyRefRepository);

            _productPropertyRefRepository.SaveChanges();
            _productRepository.SaveChanges();

            // Save log History
            _logHistoryService.Create(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });
            return new BaseResponseModel
            {
                Code = HttpStatusCodeCollection.OK,
                Message = string.Format("<strong style='color:green'>Cập nhật thành công!</strong>")
            };
        }

        /// <summary>
        /// Delete entity in db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete(long id)
        {
            // get product to delete
            var entity = _productRepository.FindId(id);
            if (entity == null) return string.Format("<strong style='color:red'>Có lỗi xảy ra không thể xóa!</strong>");

            // Delete product
            _productRepository.Delete(entity);
            // Save history
            _logHistoryService.Create(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Delete });
            return string.Format("<strong style='color:green'>Đã xóa thành công!</strong>");

        }

        /// <summary>
        /// Bind data to model for Create, Edit View
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        private void InitExtDataViewModel(ProductManageModel model)
        {
            // Bind commerce product category to select list
            model.CommerceCategories = CommerceCategoryHelper.BindSelectListItem(_commerceCategoryRepository, 0);
            // Get selected commerce product category
            if (model.Id != 0)
            {
                model.SelectedCommerceCategoryIds =
                            _productCommerceCategoryRefRepository.Find(
                                    x => x.Product.Id == model.Id).Select(x => x.CommerceCategory.Id).ToList();
            }
            // Get product properties
            model.Properties = _productPropertyRefRepository.Find(
                                        x => x.Product.Id == model.Id).ToList().Select(
                                                (x, i) => new PropertyManageModel(x) { Position = i }).ToList();
        }

        /// <summary>
        /// Lock and un-lock
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public string ManageLock(int id, bool isLock)
        {
            // Get user to update
            var entity = _productRepository.FindUnique(d => d.Id == id);
            // Check user is exist
            if (entity == null)
            {
                return string.Format("<strong style='color:red'>Không tìm thấy!</strong>");
            }

            // Update active stautus for product
            entity.IsLocked = isLock;
            _productRepository.SaveChanges();
            // Save log History
            _logHistoryService.Create(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });

            return string.Format("<strong style='color:green'>{0} sản phẩm thành công!</strong>", isLock ? "Khóa" : "Mở khóa");
        }

        /// <summary>
        /// Hide and un-hide
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isHide"></param>
        /// <returns></returns>
        public string ManageHide(int id, bool isHide)
        {
            // Get user to update
            var product = _productRepository.FindUnique(d => d.Id == id);
            // Check user is exist
            if (product == null)
            {
                return string.Format("<strong style='color:red'>Không tìm thấy!</strong>");
            }

            // Update active stautus for product
            product.IsHide = isHide;
            _productRepository.SaveChanges();
            // Save log History
            _logHistoryService.Create(new LogHistoryModel() { EntityId = product.Id, ActionType = ActionTypeCollection.Edit });

            return string.Format("<strong style='color:green'>{0} thành công!</strong>", isHide ? "Ẩn sản phẩm" : "Cho phép hiển thị");
        }
    }
}
