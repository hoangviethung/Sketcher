using MainProject.Core.Enums;
using System;

namespace MainProject.ServiceAdmin.Model.LogHistory
{
    public class LogHistoryModel
    {
        public long EntityId { get; set; }

        public ActionTypeCollection ActionType { get; set; }

        public string Comment { get; set; }
    }
}
