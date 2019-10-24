using System;
using System.ComponentModel.DataAnnotations;
using MainProject.Core.Enums;

namespace MainProject.Core
{
    public class FaqItem
    {
        public FaqItem()
        {
            Order = 1;
        }

        [Key]
        public long Id { get; set; }

        public int Order { get; set; }

        public string AskerName { get; set; }

        public string AskerPhone { get; set; }

        public string AskerEmail { get; set; }

        public string AskerAddress { get; set; }

        public DateTime AskTime { get; set; }

        public string AskTitle { get; set; }

        public string AskContent { get; set; }

        public string AnswerName { get; set; }

        public DateTime AnswerTime { get; set; }

        public string AnswerContent { get; set; }

        public FaqApprovalStatusCollection ApprovalStatus { get; set; }
    }
}
