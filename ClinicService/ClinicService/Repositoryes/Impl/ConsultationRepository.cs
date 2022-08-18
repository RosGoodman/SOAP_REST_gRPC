﻿using ClinicService.Data.Context;
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

    public int Add(Consultation item)
    {
        throw new NotImplementedException();
    }

    public void Delete(Consultation item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IList<Consultation> GetAll()
    {
        throw new NotImplementedException();
    }

    public Consultation? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Consultation item)
    {
        throw new NotImplementedException();
    }
}
