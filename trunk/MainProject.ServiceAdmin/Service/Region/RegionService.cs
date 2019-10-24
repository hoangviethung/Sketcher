using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.Branch;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Model.Region;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Service.Region
{
    public class RegionService
    {
        private readonly RegionRepository _regionRepository;
        private readonly BranchRepository _branchRepository;
        private readonly LogHistoryService _logHistoryService;
        private readonly int _itemPerPage = 20;

        public RegionService(MainDbContext dbContext, string currentUser)
        {
            _regionRepository = new RegionRepository(dbContext);
            _branchRepository = new BranchRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.Region);
        }

        #region City
        public IndexViewModel<Core.Region> GetCity(int page = 0)
        {
            if (page <= 0) page = 1;
            var query = _regionRepository.Find(x => x.RegionType == RegionTypeCollection.City);

            return new IndexViewModel<Core.Region>()
            {
                ListItems = query.OrderBy(x => x.Name).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList(),
                PagingViewModel = new PagingModel(query.Count(), _itemPerPage, page, "href='/Admin/RegionAdmin/CityIndex?page={0}'")
            };
        }

        public BaseResponseModel InsertCity(CityManageViewModel model)
        {
            var entity = new Core.Region()
            {
                RegionType = RegionTypeCollection.City
            };
            CityManageViewModel.ToEntity(model, ref entity);
            _regionRepository.Insert(entity);
            // Save history
            _logHistoryService.Create(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Create });

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.OK
            };
        }

        public BaseResponseModel<CityManageViewModel> EditCity(int id)
        {
            var entity = _regionRepository.FindUnique(x => x.Id == id);
            if (entity == null)
            {
                return new BaseResponseModel<CityManageViewModel>()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = "Không tồn tại!"
                };
            }

            return new BaseResponseModel<CityManageViewModel>()
            {
                Code = HttpStatusCodeCollection.OK,
                Result = new CityManageViewModel(entity)
            };
        }

        public BaseResponseModel UpdateCity(CityManageViewModel model)
        {
            var entity = _regionRepository.FindUnique(x => x.Id == model.Id);
            if (entity == null)
            {
                return new BaseResponseModel()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = "Không tồn tại!"
                };
            }

            CityManageViewModel.ToEntity(model, ref entity);
            _regionRepository.SaveChanges();
            // Save history
            _logHistoryService.Create(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.OK
            };
        }

        public BaseResponseModel DeleteCity(int id)
        {
            var entity = _regionRepository.FindUnique(x => x.Id == id);
            if (entity == null)
            {
                return new BaseResponseModel()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = "Không tồn tại!"
                };
            }
            // Delete branch
            _branchRepository.DeleteByCriteria(x => x.Region.Parent.Id == id);
            // Delete district
            _regionRepository.DeleteByCriteria(x => x.Parent.Id == id);
            // Delete entity
            _regionRepository.Delete(entity);

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.OK,
                Message = "Xóa thành công!"
            };
        }
        #endregion


        #region District
        public IndexViewModel<Core.Region> GetDistrict(int city = 0, int page = 0)
        {
            if (page <= 0) page = 1;
            var query = _regionRepository.Find(x => x.RegionType == RegionTypeCollection.District);
            // filter district by city
            if (city != 0)
            {
                query = query.Where(x => x.Parent.Id == city);
            }

            return new IndexViewModel<Core.Region>()
            {
                ListItems = query.OrderBy(x => x.Name).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList(),
                FilterViewModel = new FilterViewModel()
                {
                    FatherSelectModel = new FatherSelectModel()
                    {
                        Fathers = RegionHelper.BindCitySelectListItem(
                                        _regionRepository.Find(x => x.RegionType == RegionTypeCollection.City).OrderBy(x => x.Name).ToList(), city)
                    },
                    BaseUrl = "/Admin/RegionAdmin/DistrictIndex?"
                },
                PagingViewModel = new PagingModel(query.Count(), _itemPerPage, page, "href='/Admin/RegionAdmin/DistrictIndex?city" + city + "&page={0}'")
            };
        }

        public DisctrictManageViewModel CreateDistrict()
            => new DisctrictManageViewModel() {
                Cities = RegionHelper.BindCitySelectListItem(_regionRepository.Find(x => x.RegionType == RegionTypeCollection.City).ToList(), 0)
            };

        public BaseResponseModel InsertDisctrict(DisctrictManageViewModel model)
        {
            var entity = new Core.Region()
            {
                RegionType = RegionTypeCollection.District,
                Parent = _regionRepository.FindUnique(x => x.Id == model.CitySelectedValue)
            };
            DisctrictManageViewModel.ToEntity(model, ref entity);
            _regionRepository.Insert(entity);
            // Save history
            _logHistoryService.Create(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Create });

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.OK
            };
        }

        public BaseResponseModel<DisctrictManageViewModel> EditDisctrict(int id)
        {
            var entity = _regionRepository.FindUnique(x => x.Id == id);
            if (entity == null)
            {
                return new BaseResponseModel<DisctrictManageViewModel>()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = "Không tồn tại!"
                };
            }

            return new BaseResponseModel<DisctrictManageViewModel>()
            {
                Code = HttpStatusCodeCollection.OK,
                Result = new DisctrictManageViewModel(entity) {
                    Cities = RegionHelper.BindCitySelectListItem(
                                    _regionRepository.Find(x => x.RegionType == RegionTypeCollection.City).ToList(), 0)
                }
            };
        }

        public BaseResponseModel UpdateDisctrict(DisctrictManageViewModel model)
        {
            var entity = _regionRepository.FindUnique(x => x.Id == model.Id);
            if (entity == null)
            {
                return new BaseResponseModel()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = "Không tồn tại!"
                };
            }

            DisctrictManageViewModel.ToEntity(model, ref entity);
            entity.Parent = _regionRepository.FindUnique(x => x.Id == model.CitySelectedValue);
            _regionRepository.SaveChanges();
            // Save history
            _logHistoryService.Create(new LogHistoryModel() { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.OK
            };
        }

        public BaseResponseModel DeleteDisctrict(int id)
        {
            var entity = _regionRepository.FindUnique(x => x.Id == id);
            if (entity == null)
            {
                return new BaseResponseModel()
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = "Không tồn tại!"
                };
            }

            // Delete district
            _regionRepository.DeleteByCriteria(x => x.Parent.Id == id);
            // Delete entity
            _regionRepository.Delete(entity);

            return new BaseResponseModel()
            {
                Code = HttpStatusCodeCollection.OK,
                Message = "Xóa thành công!"
            };
        }
        #endregion

        public BaseResponseModel<List<SelectListItem>> GetCityApi()
            => new BaseResponseModel<List<SelectListItem>> {
                Code = HttpStatusCodeCollection.OK,
                Result = RegionHelper.BindCitySelectListItem(
                                _regionRepository.Find(x => x.RegionType == RegionTypeCollection.City).ToList(), 0)
            };

        public BaseResponseModel<List<SelectListItem>> GetDistrictApi(int cityId, int id)
            => new BaseResponseModel<List<SelectListItem>> {
                Code = HttpStatusCodeCollection.OK,
                Result = RegionHelper.BindDistrictSelectListItem(
                                _regionRepository.Find(x => x.RegionType == RegionTypeCollection.District).ToList(), cityId, id)
            };

        public BaseResponseModel<List<BranchManageViewModel>> GetBranchApi(int cityId, 
            List<int> oilTypes, int id = 0, string address = "")
        {
            var query = _branchRepository.Find(x => x.Id != 13);
            var temp = query.ToList();
            // Filter by City
            if (cityId != 0)
            {
                query = query.Where(x => x.Region.Parent.Id == cityId);
            }
            //Filter by district
            if (id != 0)
            {
                query = query.Where(x => x.Region.Id == id);
            }
            // Filter by address
            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(x => x.AddressMap.ToLower().Contains(address.ToLower()));
            }

            return new BaseResponseModel<List<BranchManageViewModel>>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = query.OrderBy(x => x.AddressMap).AsEnumerable().Select(x => new BranchManageViewModel(x)).ToList()
            };
        }
    }
}
