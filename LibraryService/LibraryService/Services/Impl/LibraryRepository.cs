
using LibraryService.Models;
using System.Collections.Generic;
using System.Linq;

namespace LibraryService.Services.Impl
{
    public class LibraryRepository : ILibraryRepositoryService
    {
        private readonly ILibraryDatabaseContextServce _dbContext;

        public LibraryRepository(ILibraryDatabaseContextServce dbContext)
        {
            _dbContext = dbContext;
        }


        public int? Add(BookModel item)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(BookModel item)
        {
            throw new System.NotImplementedException();
        }

        public IList<BookModel> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public IList<BookModel> GetByAuthor(string authorName)
        {
            return _dbContext.Books
                .Where(b => b.Authors
                .Where(a => a.Name.ToLower().Contains(authorName.ToLower())).Count() > 0)
                .ToList();
        }

        public IList<BookModel> GetByCategory(string category)
        {
            return _dbContext.Books
                .Where(b => b.Title.ToLower().Contains(category.ToLower()))
                .ToList();
        }

        public BookModel GetById(string id)
        {
            throw new System.NotImplementedException();
        }

        public IList<BookModel> GetByTitle(string title)
        {
            return _dbContext.Books
                .Where(b => b.Title.ToLower().Contains(title.ToLower()))
                .ToList();
        }

        public int Update(BookModel item)
        {
            throw new System.NotImplementedException();
        }
    }
}