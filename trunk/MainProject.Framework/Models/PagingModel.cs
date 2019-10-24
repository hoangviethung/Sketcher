namespace MainProject.Framework.Models
{
    public class PagingModel
    {
        public int TotalItemCount { get; set; }
        public int PageSize { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string ActionCode { get; set; }
        public bool HasPaging { get; set; }

        public PagingModel(int totalItems, int itemsCountPerPage, int currentPage, string actionCode)
        {
            TotalItemCount = totalItems;
            PageSize = itemsCountPerPage;

            CurrentPage = currentPage;
            HasPaging = totalItems > itemsCountPerPage;
            if (!HasPaging)
            {
                TotalPages = 1;
            }
            else
            {
                int totalPage = totalItems / itemsCountPerPage;
                TotalPages = totalItems % itemsCountPerPage == 0 ? totalPage : (totalPage + 1);
            }
            ActionCode = actionCode;
        }
    }
}
