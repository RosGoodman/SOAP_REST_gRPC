
using LibraryService.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace LibraryService.Services.Impl
{
    public class LibraryDatabaseContext : ILibraryDatabaseContextServce
    {
        private IList<BookModel> _libraryDatabase;

        public IList<BookModel> Books => _libraryDatabase;

        public LibraryDatabaseContext()
        {
            Initialize();
        }

        private void Initialize()
        {
            _libraryDatabase = JsonConvert.DeserializeObject<List<BookModel>>(Encoding.UTF8.GetString(Properties.Resources.books));
        }
    }
}