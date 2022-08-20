
namespace ClinicService.Repositoryes;

/// <summary> Интерфейс для репозиториев. </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TId"></typeparam>
public interface IRepository<T, TId>
{
    /// <summary> Добавить экземпляр класса в БД. </summary>
    /// <param name="item"> Добавляемый экземпляр. </param>
    /// <returns> Id экземпляра в БД. </returns>
    TId? Add(T item);

    /// <summary> Обновить данных экземпляра в БД. </summary>
    /// <param name="item"> Экземпляр с обновленными свойствами. </param>
    void Update(T item);

    /// <summary> Удалить экземпляр из БД. </summary>
    /// <param name="item"> Удаляемый экземпляр. </param>
    void Delete(T item);

    /// <summary> Удалить экземпляр из БД. </summary>
    /// <param name="item"> ID удаляемого экземпляра. </param>
    void Delete(TId id);

    /// <summary> Получить список всех экземпляров из БД. </summary>
    /// <returns> Список экземпляров. </returns>
    IList<T> GetAll();

    /// <summary> Получить экземпляр по ID. </summary>
    /// <param name="id"> Id искомого экземпляра. </param>
    /// <returns> Найденный в БД экземпляр. </returns>
    T? GetById(TId id);
}
