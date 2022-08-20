using ClinicService.Data.Context;
using ClinicService.Data.Models;

namespace ClinicService.Repositoryes.Impl;

public class ConsultationRepository : IConsultationRepository
{
    private readonly ClinicServiceDbContext _dbContext;
    private readonly ILogger<ConsultationRepository> _logger;

    public ConsultationRepository(ClinicServiceDbContext dbContext,
            ILogger<ConsultationRepository> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public int Add(Consultation item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();

            _dbContext.Consultations.Add(item);
            _dbContext.SaveChanges();
            return item.ConsultationId;
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Add)} в репозитории {nameof(ClientRepository)}. {ex.Message}"); }
        return -1;
    }

    /// <inheritdoc/>
    public void Delete(Consultation item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();
            Delete(item.ConsultationId);
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Delete)} в репозитории {nameof(ClientRepository)}. {ex.Message}"); }
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        try
        {
            var consultation = GetById(id);
            if (consultation is null)
                throw new KeyNotFoundException();
            _dbContext.Remove(consultation);
            _dbContext.SaveChanges();
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Delete)} в репозитории {nameof(ClientRepository)}. {ex.Message}"); }
    }

    /// <inheritdoc/>
    public IList<Consultation> GetAll()
    {
        try
        {
            return _dbContext.Consultations.ToList();
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(GetAll)} в репозитории {nameof(ClientRepository)}. {ex.Message}"); }
        return null!;
    }

    /// <inheritdoc/>
    public Consultation? GetById(int id)
    {
        try
        {
            return _dbContext.Consultations.FirstOrDefault(c => c.ConsultationId == id);
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(GetById)} в репозитории {nameof(ClientRepository)}. {ex.Message}"); }
        return null!;
    }

    /// <inheritdoc/>
    public void Update(Consultation item)
    {
        try
        {
            if (item is null)
                throw new NullReferenceException();

            var consultation = GetById(item.ConsultationId);
            if (consultation is null)
                throw new KeyNotFoundException();

            consultation.ConsultationDate = item.ConsultationDate;
            consultation.Description = item.Description;
            consultation.ClientId = item.ClientId;
            consultation.PetId = item.PetId;

            _dbContext.Update(consultation);
            _dbContext.SaveChanges();
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Update)} в репозитории {nameof(ClientRepository)}. {ex.Message}"); }
    }
}
