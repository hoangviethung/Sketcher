using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.Core
{
    public class Subscribe
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }
}
