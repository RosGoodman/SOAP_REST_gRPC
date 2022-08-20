using ClinicService.Data.Context;
using ClinicService.Data.Models;

namespace ClinicService.Repositoryes.Impl;

public class PetRepository : IPetRepository
{
    private readonly ClinicServiceDbContext _dbContext;
    private readonly ILogger<PetRepository> _logger;

    public PetRepository(ClinicServiceDbContext dbContext,
            ILogger<PetRepository> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public int Add(Pet item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();

            _dbContext.Pets.Add(item);
            _dbContext.SaveChanges();
            return item.PetId;
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Add)} в репозитории {nameof(PetRepository)}. {ex.Message}"); }
        return -1;
    }

    /// <inheritdoc/>
    public void Delete(Pet item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();
            Delete(item.PetId);
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Delete)} в репозитории {nameof(PetRepository)}. {ex.Message}"); }
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        try
        {
            var pet = GetById(id);
            if (pet is null)
                throw new KeyNotFoundException();
            _dbContext.Remove(pet);
            _dbContext.SaveChanges();
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Delete)} в репозитории {nameof(PetRepository)}. {ex.Message}"); }
    }

    /// <inheritdoc/>
    public IList<Pet> GetAll()
    {
        try
        {
            return _dbContext.Pets.ToList();
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(GetAll)} в репозитории {nameof(PetRepository)}. {ex.Message}"); }
        return null!;
    }

    /// <inheritdoc/>
    public Pet? GetById(int id)
    {
        try
        {
            return _dbContext.Pets.FirstOrDefault(pet => pet.PetId == id);
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(GetById)} в репозитории {nameof(PetRepository)}. {ex.Message}"); }
        return null!;
    }

    /// <inheritdoc/>
    public void Update(Pet item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();

            var pet = GetById(item.PetId);
            if (pet is null)
                throw new KeyNotFoundException();

            pet.Name = item.Name;
            pet.Birthday = item.Birthday;
            pet.ClientId = item.ClientId;

            _dbContext.Update(pet);
            _dbContext.SaveChanges();
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Update)} в репозитории {nameof(PetRepository)}. {ex.Message}"); }
    }
}
