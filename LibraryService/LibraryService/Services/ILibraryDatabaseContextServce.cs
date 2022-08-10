
using LibraryService.Models;
using System.Collections.Generic;

namespace LibraryService.Services
{
    public interface ILibraryDatabaseContextServce
    {
        IList<BookModel> Books { get; }
    }
}
