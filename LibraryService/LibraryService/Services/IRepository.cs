
using System.Collections.Generic;

namespace LibraryService.Services
{
    /// <summary> Интерфейс репозитория. </summary>
    /// <typeparam name="T"> Обрабатываемый элемент. </typeparam>
    public interface IRepository<T>
    {
        /// <summary> Добавить экземпляр в БД. </summary>
        /// <param name="item"> Добавляемый экземпляр. </param>
        /// <returns> Id в БД. </returns>
        string Add(T item);

        /// <summary> Обновить экземпляр в БД. </summary>
        /// <param name="item"> Экземпляр с измененными данными. </param>
        void Update(T item);

        /// <summary> Удалить экземпляр из БД. </summary>
        /// <param name="id"> Id удаляемого экземпляра. </param>
        /// <returns> Результат операции. </returns>
        bool Delete(string id);

        /// <summary> Получить список всех эезмпляров из БД. </summary>
        /// <returns> Список всех экземпляров. </returns>
        IList<T> GetAll();

        /// <summary> Получить экземпляр по Id. </summary>
        /// <param name="id"> Id искомого экземпляра в БД. </param>
        /// <returns> Найденный в БД экземпляр или null. </returns>
        T GetById(string id);
    }
}
