using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Helper;
using MainProject.Framework.Models;
using MainProject.Service.Model.Contact;
using Newtonsoft.Json;
using System;
using System.Net;

namespace MainProject.Service.Service.Contact
{
    public class ContactService
    {
        private readonly ContactRepository _contactRepository;
        public ContactService(MainDbContext dbContext)
        {
            _contactRepository = new ContactRepository(dbContext);
        }

        public BaseResponseModel Save(bool modelState, string message, string recaptcha, ContactManageModel model)
        {
            if (modelState)
            {
                //secret that was generated in key value pair
                string secret = SettingHelper.GetValueSetting("SecretKeyRecaptcha");
                var client = new WebClient();
                var reply =
                    client.DownloadString(
                        string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret,
                            recaptcha));

                var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                //when response is true check for the error message
                if (captchaResponse.Success)
                {
                    // Bind data model to entity
                    var entity = new Core.Contact
                    {
                        CreatedDate = DateTime.Now
                    };
                    ContactManageModel.ToEntity(model, ref entity);
                    // Insert contact information to db
                    _contactRepository.Insert(entity);
                    // Send mail to submitter
                    MailHelper.Send(entity, "Xác nhận thông tin liên hệ!");

                    return new BaseResponseModel
                    {
                        Code = HttpStatusCodeCollection.OK,
                        Message = string.Format("<script> alert('" + ResourceHelper.GetResource(ResourceKeyCollection.Contact_success) + "')</script>")
                    };
                }

                switch (captchaResponse.ErrorCodes[0].ToLower())
                {
                    case ("missing-input-secret"):
                        message = string.Format("<script> alert('The secret parameter is missing.')</script>");
                        break;
                    case ("invalid-input-secret"):
                        message = string.Format("<script> alert('The secret parameter is invalid or malformed.')</script>");
                        break;
                    case ("missing-input-response"):
                        message = string.Format("<script> alert('The response parameter is missing.')</script>");
                        break;
                    case ("invalid-input-response"):
                        message = string.Format("<script> alert('The response parameter is invalid or malformed.')</script>");
                        break;
                    default:
                        message = string.Format("<script> alert('Error occured. Please try again')</script>");
                        break;
                }

            }

            return new BaseResponseModel
            {
                Code = HttpStatusCodeCollection.BadRequest,
                Message = message
            };
        }
    }
}
