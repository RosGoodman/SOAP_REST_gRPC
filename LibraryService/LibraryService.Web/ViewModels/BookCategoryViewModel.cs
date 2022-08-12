using LibraryService.Web.Models;
using LibraryServiceReference;

namespace LibraryService.Web.ViewModels
{
    public class BookCategoryViewModel
    {
        public BookModel[] Books { get; set; }

        public SearchTypeEnum SearchType { get; set; }

        public string? SearchString { get; set; }
    }
}
