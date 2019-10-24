using MainProject.Service.Model.Contact;
using MainProject.Service.Service.Contact;
using MainProject.Web.BaseControllers;
using System.Linq;
using System.Web.Mvc;

namespace MainProject.Web.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Contact
        private readonly ContactService _service;

        public ContactController()
        {
            _service = new ContactService(DbContext);
        }

        [HttpPost]
        public ActionResult Submit(ContactManageModel model)
            => Json(_service.Save(ModelState.IsValid,
                            string.Join("\n", ModelState.Values.SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage)), Request["g-recaptcha-response"], model));
    }
}