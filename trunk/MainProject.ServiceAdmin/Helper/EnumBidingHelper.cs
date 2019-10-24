using MainProject.Core.Enums;
using MainProject.Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MainProject.ServiceAdmin.Helper
{
   public class EnumBidingHelper
    {
        public static List<SelectListItem> LinkTargetSelectListItems(LinkTargetCollection? selectedvalue)
        {
            return Enum.GetValues(typeof(LinkTargetCollection)).Cast<LinkTargetCollection>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.GetDescription(),
                    Selected = selectedvalue == c
                }).ToList();
        }
    }
}
