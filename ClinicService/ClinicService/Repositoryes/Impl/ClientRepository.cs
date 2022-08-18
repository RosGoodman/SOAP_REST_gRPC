using ClinicService.Data.Context;
using ClinicService.Data.Models;

namespace ClinicService.Repositoryes.Impl;

public class ClientRepository : IClientRepository
{
    private readonly ClinicServiceDbContext _dbContext;
    private readonly ILogger<ClientRepository> _logger;

    public ClientRepository(ClinicServiceDbContext dbContext,
            ILogger<ClientRepository> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <inheritdoc/>
    public int Add(Client item)
    {
        try
        {
            _dbContext.Clients.Add(item);
            _dbContext.SaveChanges();
            return item.ClientId;
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Add)} в репозитории {nameof(ClientRepository)}"); }
        return -1;
    }

    /// <inheritdoc/>
    public void Delete(Client item)
    {
        try
        {
            if (item == null)
                throw new NullReferenceException();
            Delete(item.ClientId);
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Delete)} в репозитории {nameof(ClientRepository)}"); }
    }

    /// <inheritdoc/>
    public void Delete(int id)
    {
        try
        {
            var client = GetById(id);
            if (client == null)
                throw new KeyNotFoundException();
            _dbContext.Remove(client);
            _dbContext.SaveChanges();
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Delete)} в репозитории {nameof(ClientRepository)}"); }
    }

    /// <inheritdoc/>
    public IList<Client> GetAll()
    {
        try
        {
            return _dbContext.Clients.ToList();
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(GetAll)} в репозитории {nameof(ClientRepository)}"); }
        return null!;
    }

    /// <inheritdoc/>
    public Client? GetById(int id)
    {
        try
        {
            return _dbContext.Clients.FirstOrDefault(client => client.ClientId == id);
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(GetById)} в репозитории {nameof(ClientRepository)}"); }
        return null!;
    }

    /// <inheritdoc/>
    public void Update(Client item)
    {
        try
        {
            if (item == null)
                throw new NullReferenceException();

            var client = GetById(item.ClientId);
            if (client == null)
                throw new KeyNotFoundException();

            client.Document = item.Document;
            client.Surname = item.Surname;
            client.FirstName = item.FirstName;
            client.Patronymic = item.Patronymic;

            _dbContext.Update(client);
            _dbContext.SaveChanges();
        }
        catch (Exception ex) { _logger.LogError($"Ошибка при попытке выполнить метод {nameof(Update)} в репозитории {nameof(ClientRepository)}"); }
    }
}
