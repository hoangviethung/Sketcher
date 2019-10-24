using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Helper;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Model.StringResource;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Linq;

namespace MainProject.ServiceAdmin.Service.StringResource
{
    public class StringResourceService
    {
        private readonly LanguageRepository _languageRepository;
        private readonly StringResourceKeyRepository _stringResourceKeyRepository;
        private readonly StringResourceValueRepository _stringResourceValueRepository;
        private readonly LogHistoryService _logHistoryService;
        private readonly int _itemPerPage = 10;

        public StringResourceService(MainDbContext dbContext, string currentUser)
        {
            _languageRepository = new LanguageRepository(dbContext);
            _stringResourceKeyRepository = new StringResourceKeyRepository(dbContext);
            _stringResourceValueRepository = new StringResourceValueRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.Resource);
        }

        /// <summary>
        /// Get index resource
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public StringResourceViewModel GetIndex(int languageId = 0, string filter = null, int page = 1)
        {
            // Set default language
            languageId = languageId != 0 ? languageId : _languageRepository.FindAll().First().Id;
            // Get resource by langauge
            var sql = _stringResourceValueRepository.Find(d => d.Language.Id == languageId);
            // Filter by value
            if (!string.IsNullOrWhiteSpace(filter))
                sql = sql.Where(d => d.Value.Contains(filter.Trim()));
            // Count total
            int count = sql.Count();

            return new StringResourceViewModel
            {
                Languages = LanguageHelper.BindDropdown(_languageRepository.FindAll().ToList(), languageId),
                LanguageSelectedValue = languageId,
                ListStringResource = sql.OrderByDescending(x => x.Id).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList(),
                PagingViewModel = new PagingModel(count, _itemPerPage, page, "href='/Admin/ResourceAdmin/Index?languageId=" + languageId + "&page={0}'"),
                Filter = filter
            };
        }

        ///// <summary>
        ///// init resource
        ///// </summary>
        //public void InitResources()
        //{
        //    CacheHelper.Dispose();
        //    var resources = Enum.GetNames(typeof(ResourceKeyCollection)).ToList();
        //    foreach (var item in resources)
        //        AddResourceKeyAndValues(item);
        //    _stringResourceValueRepository.SaveChanges();
        //    // clean dupplicate
        //    var items = _stringResourceKeyRepository.FindAll();

        //    var exitsItemNames = new List<string>();
        //    var removeItems = new List<long>();
        //    foreach (var item in items)
        //    {
        //        if (!exitsItemNames.Contains(item.Name))
        //            exitsItemNames.Add(item.Name);
        //        else
        //            removeItems.Add(item.Id);
        //    }
        //    _stringResourceValueRepository.DeleteByCriteria(c => removeItems.Contains(c.Key.Id));
        //    _stringResourceKeyRepository.DeleteByCriteria(c => removeItems.Contains(c.Id));
        //    _stringResourceKeyRepository.SaveChanges();
        //}

        ///// <summary>
        ///// Add resource key and value
        ///// </summary>
        ///// <param name="resourceName"></param>
        //public void AddResourceKeyAndValues(string resourceName)
        //{
        //    var keyItem = _stringResourceKeyRepository.Find(x => x.Name.Equals(resourceName)).FirstOrDefault();
        //    if (keyItem == null)
        //    {
        //        keyItem = new StringResourceKey
        //        {
        //            Name = resourceName
        //        };
        //        _stringResourceKeyRepository.Insert(keyItem);
        //        _stringResourceKeyRepository.SaveChanges();
        //    }
        //    _stringResourceKeyRepository.SaveChanges();
        //    var langs = _languageRepository.FindAll().ToList();
        //    var resourceValues = _stringResourceValueRepository.Find(c => c.Key.Id == keyItem.Id).ToList();

        //    foreach (var lang in langs)
        //    {
        //        var resourceItem = resourceValues.FirstOrDefault(x => x.Language.Id == lang.Id);
        //        if (resourceItem == null)
        //        {
        //            resourceItem = new StringResourceValue { Key = keyItem, Language = lang, Value = resourceName };
        //            _stringResourceValueRepository.Insert(resourceItem);
        //        }
        //        else
        //        {
        //            if (string.IsNullOrEmpty(resourceItem.Value))
        //            {
        //                resourceItem.Value = resourceName;
        //                _stringResourceValueRepository.SaveChanges();
        //            }
        //        }
        //    }

        //    var exitsItems = new List<StringResourceValue>();
        //    var resourcesToRemoveIds = new List<long>();
        //    foreach (var resourceValue in resourceValues)
        //    {
        //        if (exitsItems.All(x => x.Language.Id != resourceValue.Language.Id))
        //            exitsItems.Add(resourceValue);
        //        else
        //            resourcesToRemoveIds.Add(resourceValue.Id);
        //    }
        //    _stringResourceValueRepository.DeleteByCriteria(c => resourcesToRemoveIds.Contains(c.Id));
        //    _stringResourceValueRepository.SaveChanges();
        //    _logHistoryService.Create(new LogHistoryModel { EntityId = keyItem.Id, ActionType = ActionTypeCollection.Create });
        //}

        /// <summary>
        /// Get string resource for edit view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StringResourceValueModel Edit(long id)
        {
            // Get entity bind data to model
            var entity = _stringResourceValueRepository.FindUnique(x => x.Id == id);
            if (entity != null)
                return new StringResourceValueModel(entity);
            return null;
        }

        /// <summary>
        /// Update data to db
        /// </summary>
        /// <param name="stringResourceValue"></param>
        public void Update(StringResourceValueModel model)
        {
            // Get entity to update
            var entity = _stringResourceValueRepository.FindUnique(d => d.Id == model.Id);
            // Bind model to entity
            StringResourceValueModel.ToEntity(model, ref entity);
            // Save entity
            _stringResourceValueRepository.SaveChanges();
            // Save reosurce to cache
            ResourceHelper.UpdateResourceOnCache(entity.Key.Name, entity.Language.LanguageKey, entity.Value);
            // Save log history
            _logHistoryService.Create(new LogHistoryModel { EntityId = entity.Id, ActionType = ActionTypeCollection.Edit });
        }

    }
}
