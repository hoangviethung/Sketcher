using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Core.Enums
{
    public enum LinkTargetCollection
    {
        [Description("Cửa sổ hiện tại")]
        Self,

        [Description("Cửa sổ mới")]
        Blank,

        [Description("Popup")]
        Popup
    }
}
