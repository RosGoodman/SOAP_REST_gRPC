
using LibraryService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        #region repository methods

        /// <inheritdoc/>
        public string Add(BookModel item)
        {
            try
            {
                item.Id = Guid.NewGuid().ToString();
                _dbContext.Books.Add(item);

                return item.Id;
            }
            catch (Exception ex)
            {
                Debug.Print($"Ошибка при попытке добавить экземпляр {typeof(BookModel)} в БД. {ex.Message}");
                return string.Empty;
            }
        }

        /// <inheritdoc/>
        public bool Delete(string id)
        {
            try
            {
                var bookDb = _dbContext.Books
                    .Where(b => b.Id == id)
                    .FirstOrDefault();

                if (bookDb is null) return true;

                _dbContext.Books.Remove(bookDb);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print($"Ошибка при попытке добавить экземпляр {typeof(BookModel)} в БД. {ex.Message}");
                return false;
            }
        }

        /// <inheritdoc/>
        public IList<BookModel> GetAll()
        {
            try
            {
                return _dbContext.Books;
            }
            catch (Exception ex)
            {
                Debug.Print($"Ошибка при попытке получить список всех экземпляров {typeof(BookModel)} из БД. {ex.Message}");
                return new List<BookModel>();
            }
        }

        /// <inheritdoc/>
        public IList<BookModel> GetByAuthor(string authorName)
        {
            try
            {
                return _dbContext.Books
                .Where(b => b.Authors
                .Where(a => a.Name.ToLower().Contains(authorName.ToLower())).Count() > 0)
                .ToList();
            }
            catch (Exception ex)
            {
                Debug.Print($"Ошибка при попытке получить список экземпляров {typeof(BookModel)} из БД по автору. {ex.Message}");
                return new List<BookModel>();
            }
        }

        /// <inheritdoc/>
        public IList<BookModel> GetByCategory(string category)
        {
            try
            {
                return _dbContext.Books
                .Where(b => b.Title.ToLower().Contains(category.ToLower()))
                .ToList();
            }
            catch (Exception ex)
            {
                Debug.Print($"Ошибка при попытке поулчить список экземпляров {typeof(BookModel)} из БД по категории. {ex.Message}");
                return new List<BookModel>();
            }
        }

        /// <inheritdoc/>
        public BookModel GetById(string id)
        {
            try
            {
                return _dbContext.Books
                    .Where(b => b.Id == id)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.Print($"Ошибка при попытке получить экземпляр {typeof(BookModel)} из БД по Id. {ex.Message}");
                return new BookModel();
            }
        }

        /// <inheritdoc/>
        public IList<BookModel> GetByTitle(string title)
        {
            try
            {
                return _dbContext.Books
                .Where(b => b.Title.ToLower().Contains(title.ToLower()))
                .ToList();
            }
            catch (Exception ex)
            {
                Debug.Print($"Ошибка при попытке получить список экземпляров {typeof(BookModel)} из БД по наименованию. {ex.Message}");
                return new List<BookModel>();
            }
        }

        /// <inheritdoc/>
        public void Update(BookModel item)
        {
            try
            {
                var bookDb = _dbContext.Books
                    .Where(b => b.Id == item.Id)
                    .FirstOrDefault();

                bookDb.Title = item.Title;
                bookDb.AgeLimit = item.AgeLimit;
                bookDb.Authors = item.Authors;
                bookDb.PublicationDate = item.PublicationDate;
                bookDb.Category = item.Category;
                bookDb.Lang = item.Lang;
                bookDb.Pages = item.Pages;
            }
            catch (Exception ex)
            {
                Debug.Print($"Ошибка при попытке обновить экземпляр {typeof(BookModel)} в БД. {ex.Message}");
            }
        }

        #endregion
    }
}