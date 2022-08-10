
using LibraryService.Models;
using System.Collections.Generic;

namespace LibraryService.Services
{
    public interface ILibraryRepositoryService : IRepository<BookModel>
    {
        IList<BookModel> GetByTitle(string title);

        IList<BookModel> GetByAuthor(string authorName);

        IList<BookModel> GetByCategory(string category);
    }
}