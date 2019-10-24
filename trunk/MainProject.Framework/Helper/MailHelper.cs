using MainProject.Core;
using MainProject.Framework.Constant;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace MainProject.Framework.Helper
{
    public class MailHelper
    {
        public static void Send(Contact entity, string subject, IEnumerable<string> filesToAttach = null)
        {
            try
            {
                // Create template for email
                string body = "<div><h3>Chúng tôi đã nhận được thông tin liên hệ từ bạn với thông tin sau:</h3></div>";
                body += "<fieldset><legend>Thông tin liên hệ</legend>";
                body += "<div><b>Họ tên:</b></div>";
                body += "<div>" + entity.FullName + "</div>";
                body += "<div><b>Điện thoại:</b></div>";
                body += "<div>" + entity.PhoneNumber + "</div>";
                body += "<div><b>Email:</b></div>";
                body += "<div>" + entity.Email + "</div>";
                body += "<div><b>Địa chỉ:</b></div>";
                body += "<div>" + entity.Address + "</div>";
                body += "<div><b>Nội dung :</b></div>";
                body += "<div>" + entity.Content + "</div>";
                body += "</fieldset>";

                // Create a message and set up the recipients.
                MailMessage message = new MailMessage(
                    SettingHelper.GetValueSetting(MailConstant.UserName),
                    entity.Email, subject, body);
                message.IsBodyHtml = true;
                foreach (var emailCC in SettingHelper.GetValueSetting(MailConstant.EmailCC).Split(';'))
                {
                    message.CC.Add(emailCC);
                }

                //Send the message.
                SmtpClient client = new SmtpClient(
                    SettingHelper.GetValueSetting(MailConstant.Server),
                    Convert.ToInt32(SettingHelper.GetValueSetting(MailConstant.Port)));
                client.EnableSsl = Convert.ToBoolean(SettingHelper.GetValueSetting(MailConstant.SSL));
                // Add credentials if the SMTP server requires them.
                client.Credentials = new NetworkCredential(
                    SettingHelper.GetValueSetting(MailConstant.UserName),
                    SettingHelper.GetValueSetting(MailConstant.Password));

                client.Send(message);
            }
            catch (Exception) { }
        }
    }
}
