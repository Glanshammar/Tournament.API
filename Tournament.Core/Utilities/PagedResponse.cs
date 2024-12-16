using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Utilities
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }

        public PagedResponse(IEnumerable<T> data, int totalPages, int pageSize, int currentPage, int totalItems)
        {
            Data = data;
            TotalPages = totalPages;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalItems = totalItems;
        }
    }

}
