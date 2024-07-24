namespace EvilCorp2000Products.Services
{
    public class PaginationMetaData
    {
        public PaginationMetaData(int currentPage, int pageSize, int totaItemCount)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotaItemCount = totaItemCount;
            TogalPageCount = (int) Math.Ceiling(totaItemCount / (double) pageSize);
        }

        public int CurrentPage { get; set; }
        public int TogalPageCount { get; set; }
        public int PageSize { get; set; }
        public int TotaItemCount { get; set; }

        
    }
}
