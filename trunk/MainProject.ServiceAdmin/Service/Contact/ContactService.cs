using MainProject.Core.Enums;
using MainProject.Data;
using MainProject.Data.Repositories;
using MainProject.Framework.Models;
using MainProject.ServiceAdmin.Model;
using MainProject.ServiceAdmin.Model.Contact;
using MainProject.ServiceAdmin.Model.LogHistory;
using MainProject.ServiceAdmin.Service.LogHistory;
using System.Linq;

namespace MainProject.ServiceAdmin.Service.Contact
{
    public class ContactService
    {
        private readonly ContactRepository _contactRepository;
        private readonly LogHistoryService _logHistoryService;
        private readonly int _itemPerPage = 20;

        public ContactService(MainDbContext dbContext, string currentUser)
        {
            _contactRepository = new ContactRepository(dbContext);
            _logHistoryService = new LogHistoryService(dbContext, EntityTypeCollection.Contact);
        }

        /// <summary>
        /// Get end filter contact
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IndexViewModel<Core.Contact> GetIndex(int page = 1)
        {
            if (page < 1) page = 1;
            var sql = _contactRepository.FindAll();

            // Count total entity
            int count = sql.Count();

            return new IndexViewModel<Core.Contact>()
            {
                ListItems = sql.OrderByDescending(x => x.Id).Skip((page - 1) * _itemPerPage).Take(_itemPerPage).ToList(),
                PagingViewModel = new PagingModel(count, _itemPerPage, page, "href='/Admin/ContactAdmin/Index?page={0}'")
            };
        }

        /// <summary>
        /// View detail contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BaseResponseModel<ContactManageViewModel> Detail(long id)
        {
            var contact = _contactRepository.FindUnique(x => x.Id == id);
            if (contact == null)
            {
                return new BaseResponseModel<ContactManageViewModel>
                {
                    Code = HttpStatusCodeCollection.BadRequest,
                    Message = string.Format("Không tìm thấy đối tượng")
                };
            }

            return new BaseResponseModel<ContactManageViewModel>
            {
                Code = HttpStatusCodeCollection.OK,
                Result = new ContactManageViewModel(contact)
            };
        }

        /// <summary>
        /// Delete data contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Delete(long id)
        {
            // Get banner for checking exist and deleting
            var contact = _contactRepository.FindUnique(x => x.Id == id);
            if (contact == null)
                return string.Format("Có lỗi xảy ra không thể xóa được!");
            // Delete banner
            _contactRepository.Delete(contact);
            // Save history
            _logHistoryService.Insert(new LogHistoryModel() { EntityId = contact.Id, ActionType = ActionTypeCollection.Delete });
            return string.Format("<strong style='color:green'>Xóa liên hệ công!</strong>");
        }
    }
}
