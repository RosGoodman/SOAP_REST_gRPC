
using LibraryService.Models;
using System.Collections.Generic;

namespace LibraryService.Services
{
    /// <summary> Интерфейс репозитория моделей книг. </summary>
    public interface ILibraryRepositoryService : IRepository<BookModel>
    {
        /// <summary> Получить список экземпляров по наименованию. </summary>
        /// <param name="title"> Наименование книги. </param>
        /// <returns> Список найденных экземпляров. </returns>
        IList<BookModel> GetByTitle(string title);

        /// <summary> Получить список экземпляров по автору. </summary>
        /// <param name="authorName"> Автор. </param>
        /// <returns> Список найденных экземпляров. </returns>
        IList<BookModel> GetByAuthor(string authorName);

        /// <summary> Получить список экземпляров по категории. </summary>
        /// <param name="category"> Категория книги. </param>
        /// <returns> Список найденных экземпляров. </returns>
        IList<BookModel> GetByCategory(string category);
    }
}