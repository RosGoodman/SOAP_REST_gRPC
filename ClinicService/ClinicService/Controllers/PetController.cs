using ClinicService.Data.Models;
using ClinicService.Models.Requests.Pet;
using ClinicService.Repositoryes;
using Microsoft.AspNetCore.Mvc;

namespace ClinicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        #region Serives

        private readonly IPetRepository _petRepository;
        private readonly ILogger<PetController> _logger;

        #endregion

        #region Constructors

        public PetController(
            IPetRepository clientRepository,
            ILogger<PetController> logger)
        {
            _logger = logger;
            _petRepository = clientRepository;
        }

        #endregion

        #region Public Methods

        [HttpPost("create")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public IActionResult Create([FromBody] CreatePetRequest createRequest) =>
            Ok(_petRepository.Add(new Pet
            {
                Birthday = createRequest.Birthday,
                ClientId = createRequest.ClientId,
                Name = createRequest.Name
            }));

        [HttpPut("update")]
        public IActionResult Update([FromBody] UpdatePetRequest updateRequest)
        {
            _petRepository.Update(new Pet
            {
                PetId = updateRequest.PetId,
                Birthday = updateRequest.Birthday,
                ClientId = updateRequest.ClientId,
                Name = updateRequest.Name
            });
            return Ok();
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] int petId)
        {
            _petRepository.Delete(petId);
            return Ok();
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(IList<Client>), StatusCodes.Status200OK)]
        public IActionResult GetAll() =>
            Ok(_petRepository.GetAll());

        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public IActionResult GetById([FromRoute] int petId) =>
            Ok(_petRepository.GetById(petId));


        #endregion
    }
}
