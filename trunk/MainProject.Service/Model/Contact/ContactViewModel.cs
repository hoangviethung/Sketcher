using MainProject.Core;
using System.Collections.Generic;

namespace MainProject.Service.Model.Contact
{
    /// <summary>
    /// Return data to index view
    /// </summary>
    public class ContactViewModel
    {
        /// <summary>
        /// Current category for getting Title, Descrition,... for SEO
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// List branch of company
        /// </summary>
        public List<Branch> Branches { get; set; }
    }
}
