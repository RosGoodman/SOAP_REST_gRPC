using ClinicService.Data.Models;
using ClinicService.Models.Requests.Consultation;
using ClinicService.Repositoryes;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        #region Serives

        private readonly IConsultationRepository _consultationRepository;
        private readonly ILogger<ConsultationController> _logger;

        #endregion

        #region Constructors

        public ConsultationController(
            IConsultationRepository clientRepository,
            ILogger<ConsultationController> logger)
        {
            _logger = logger;
            _consultationRepository = clientRepository;
        }

        #endregion

        #region Public Methods

        [HttpPost("create")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public IActionResult Create([FromBody] CreateConsultationRequest createRequest) =>
            Ok(_consultationRepository.Add(new Consultation
            {
                ClientId = createRequest.ClientId,
                ConsultationDate = createRequest.ConsultationDate,
                Description = createRequest.Descripion,
                PetId = createRequest.PetId,
            }));

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdateConsultationRequest updateRequest)
        {
            _consultationRepository.Update(new Consultation
            {
                ConsultationId = updateRequest.ConsultationId,
                ClientId = updateRequest.ClientId,
                ConsultationDate = updateRequest.ConsultationDate,
                Description = updateRequest.Descripion,
                PetId = updateRequest.PetId,
            });
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int consultationId)
        {
            _consultationRepository.Delete(consultationId);
            return Ok();
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(IList<Client>), StatusCodes.Status200OK)]
        public IActionResult GetAll() =>
            Ok(_consultationRepository.GetAll());

        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public IActionResult GetById([FromRoute] int consultationId) =>
            Ok(_consultationRepository.GetById(consultationId));


        #endregion
    }
}
