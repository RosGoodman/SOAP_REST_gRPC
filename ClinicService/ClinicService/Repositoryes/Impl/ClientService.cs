using ClinicService.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.Repositoryes.Impl;

public class ClientService
{
    private readonly ClinicServiceDbContext _dbContext;
    private readonly ILogger<ClientService> _logger;

    public ClientService(ClinicServiceDbContext dbContext,
            ILogger<ClientService> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
}
