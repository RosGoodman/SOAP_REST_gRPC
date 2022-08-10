using System.ComponentModel.DataAnnotations;

namespace LibraryService.Web.Models
{
    public enum SearchTypeEnum
    {
        [Display(Name = "Заголовок")]
        Title,

        [Display(Name = "Автор")]
        Author,

        [Display(Name = "Категория")]
        Category,
    }
}
