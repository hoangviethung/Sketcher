using System;
using System.Linq;
using MainProject.Core;
using MainProject.Core.Enums;
using MainProject.Framework.Helper;
using Quartz;

namespace MainProject.Framework.ScheduleTask
{
    class DeleteImagesNotUseJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var dbContext = DalHelper.InitDbContext();
            // Delete image
            //var entities = dbContext.LogHistories.Where(x => x.ActionType == ActionTypeCollection.Temp);
            //foreach (var entity in entities)
            //{

            //}

            dbContext.LogHistories.Add(new LogHistory {
                ActionBy = "quartz",
                ActionType = ActionTypeCollection.Create,
                Comment = "Schedule hang ngay",
                CreatedDate = DateTime.Now,
                EntityId = 1,
                EntityType = EntityTypeCollection.Categories
            });
            dbContext.SaveChanges();
            dbContext.Dispose();
        }
    }
}
