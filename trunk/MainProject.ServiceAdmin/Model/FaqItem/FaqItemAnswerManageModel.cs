using System;
using System.ComponentModel.DataAnnotations;

namespace MainProject.ServiceAdmin.Model.FaqItem
{
   public class FaqItemAnswerManageModel
    {
        public long Id { get; set; }

        public int Order { get; set; }

        public string AskerName { get; set; }

        public string AskTitle { get; set; }

        public string AskContent { get; set; }

        public DateTime AskTime { get; set; }

        public string AskerEmail { get; set; }

        public string AnswerName { get; set; }

        public DateTime AnswerTime { get; set; }

        [Required(ErrorMessage = "Nhập câu trả lời")]
        public string AnswerContent { get; set; }

        public bool IsPending { get; set; }

        public FaqItemAnswerManageModel()
        {

        }
        public FaqItemAnswerManageModel(Core.FaqItem model)
        {
            Id = model.Id;
            Order = model.Order;
            AskTitle = model.AskTitle;
            AskContent = model.AskContent;
            AnswerContent = model.AnswerContent;
            AnswerName = model.AnswerName;
            AnswerTime = model.AnswerTime;
            AskTime = model.AskTime;
            AskerName = model.AskerName;
            AskerEmail = model.AskerEmail;
        }
    }
}
