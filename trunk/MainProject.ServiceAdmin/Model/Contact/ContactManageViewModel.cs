using MainProject.Framework.Models;
using System;
using System.Collections.Generic;

namespace MainProject.ServiceAdmin.Model.Contact
{
    public class ContactManageViewModel
    {
        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }

        public ContactManageViewModel() {  }

        public ContactManageViewModel(Core.Contact entity)
        {
            FullName = entity.FullName;
            PhoneNumber = entity.PhoneNumber;
            Email = entity.Email;
            Address = entity.Address;
            Content = entity.Content;
            CreatedDate = entity.CreatedDate;
        }
    }
}
