
namespace LibraryService.Models
{
    public class BookModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string Lang { get; set; }

        public int Pages { get; set; }

        public int AgeLimit { get; set; }

        public int PublicationDate { get; set; }

        public AuthorModel[] Authors { get; set; }
    }
}