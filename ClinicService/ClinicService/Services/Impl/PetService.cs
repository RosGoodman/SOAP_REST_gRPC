using ClinicService.Data.Context;
using ClinicService.Data.Models;
using ClinicService.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static ClinicService.Protos.PetService;

namespace ClinicService.Services.Impl;

public class PetService : PetServiceBase
{
    private readonly ClinicServiceDbContext _dbContext;
    private readonly ILogger<PetService> _logger;

    public PetService(ClinicServiceDbContext dbContext,
            ILogger<PetService> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public override Task<CreatePetResponse> CreatePet(CreatePetRequest request, ServerCallContext context)
    {
        var Pet = new Pet
        {
            Name = request.Name,
            Birthday = request.Birthday.ToDateTime(),
            ClientId = request.ClientId,
        };
        _dbContext.Pets.Add(Pet);

        _dbContext.SaveChanges();

        var response = new CreatePetResponse
        {
            PetId = Pet.PetId
        };

        return Task.FromResult(response);
    }

    public override Task<GetPetsResponse> GetPets(GetPetsRequest request, ServerCallContext context)
    {
        var response = new GetPetsResponse();
        response.Pets.AddRange(_dbContext.Pets.Select(pet => new PetResponse
        {
            PetId = pet.PetId,
            Name = pet.Name,
            Birthday = Timestamp.FromDateTime(pet.Birthday.ToUniversalTime()),
            ClientId = pet.ClientId,

        }).ToList());

        return Task.FromResult(response);
    }
}
