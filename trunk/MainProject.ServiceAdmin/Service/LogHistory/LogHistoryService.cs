using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.ServiceAdmin.Model.LogHistory;
using System;

namespace MainProject.ServiceAdmin.Service.LogHistory
{
    public class LogHistoryService
    {
        private readonly LogHistoryRepository _logHistoryRepository;
        private readonly string _userName = AccountHelper.CurrentUserName();
        private readonly EntityTypeCollection _entityType;

        public LogHistoryService(MainDbContext dbContext,
            EntityTypeCollection entityType)
        {
            _logHistoryRepository = new LogHistoryRepository(dbContext);
            _entityType = entityType;
        }

        /// <summary>
        /// In function Create
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long Create(LogHistoryModel model)
        {
            var entity = new Core.LogHistory()
            {
                EntityId = model.EntityId,
                ActionBy = _userName,
                CreatedDate = DateTime.Now,
                EntityType = _entityType,
                ActionType = model.ActionType,
                Comment = EnumHelper.GetDescription(model.ActionType) + " " + EnumHelper.GetDescription(_entityType)
            };
            _logHistoryRepository.Insert(entity);
            return entity.Id;
        }

        /// <summary>
        /// In function Insert
        /// </summary>
        /// <param name="id"></param>
        public void Update(long id, long entityId)
        {
            var logHistory = _logHistoryRepository.FindUnique(x => x.Id == id);
            logHistory.EntityId = entityId;
            logHistory.ActionType = ActionTypeCollection.Create;
            logHistory.Comment = EnumHelper.GetDescription(logHistory.ActionType) + " " + EnumHelper.GetDescription(logHistory.EntityType);
            _logHistoryRepository.SaveChanges();
        }

        /// <summary>
        /// In function Update, Delete
        /// </summary>
        /// <param name="model"></param>
        public void Insert(LogHistoryModel model)
        {
            _logHistoryRepository.Insert(new Core.LogHistory()
            {
                EntityId = model.EntityId,
                ActionBy = _userName,
                CreatedDate = DateTime.Now,
                EntityType = _entityType,
                ActionType = model.ActionType,
                Comment = model.Comment ?? EnumHelper.GetDescription(model.ActionType) + " " + EnumHelper.GetDescription(_entityType)
            });
        }
    }
}
