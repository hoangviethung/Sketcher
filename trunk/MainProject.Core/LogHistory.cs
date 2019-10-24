using System;
using System.ComponentModel.DataAnnotations;
using MainProject.Core.Enums;

namespace MainProject.Core
{
    public class LogHistory
    {
        [Key]
        public long Id { get; set; }

        public EntityTypeCollection EntityType { get; set; }
        
        public long EntityId { get; set; }

        public ActionTypeCollection ActionType { get; set; }
        
        public string ActionBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Comment { get; set; }
    }
}
