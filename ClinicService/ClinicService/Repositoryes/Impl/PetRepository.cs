﻿using ClinicService.Data.Context;
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

    public int Add(Pet item)
    {
        throw new NotImplementedException();
    }

    public void Delete(Pet item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public IList<Pet> GetAll()
    {
        throw new NotImplementedException();
    }

    public Pet? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Pet item)
    {
        throw new NotImplementedException();
    }
}
