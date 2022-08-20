using ClinicService.Data.Context;
using ClinicService.Data.Models;
using ClinicService.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static ClinicService.Protos.ConsultationService;

namespace ClinicService.Services.Impl;

public class ConsultationService : ConsultationServiceBase
{
    private readonly ClinicServiceDbContext _dbContext;
    private readonly ILogger<ConsultationService> _logger;

    public ConsultationService(ClinicServiceDbContext dbContext,
            ILogger<ConsultationService> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public override Task<CreateConsultationResponse> CreateConsultation(CreateConsultationRequest request, ServerCallContext context)
    {
        var consultation = new Consultation
        {
            ClientId = request.ClientId,
            ConsultationDate = request.ConsultationDate.ToDateTime(),
            Description = request.Description,
            PetId = request.PetId,
        };
        _dbContext.Consultations.Add(consultation);

        _dbContext.SaveChanges();

        var response = new CreateConsultationResponse
        {
            ConsultationId = consultation.ConsultationId
        };

        return Task.FromResult(response);
    }

    public override Task<GetConsultationsResponse> GetConsultations(GetConsultationsRequest request, ServerCallContext context)
    {
        var response = new GetConsultationsResponse();
        response.Consultations.AddRange(_dbContext.Consultations.Select(consultation => new ConsultationResponse
        {
            ConsultationId = consultation.ConsultationId,
            ClientId = consultation.ClientId,
            ConsultationDate = Timestamp.FromDateTime(consultation.ConsultationDate.ToUniversalTime()),
            Description = consultation.Description,
            PetId = consultation.PetId,
        }).ToList());

        return Task.FromResult(response);
    }
}
