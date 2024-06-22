using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;
using TravelAgencyApi.Repository;

namespace TravelAgencyApi.Controllers
{
    [ApiController]
    public class TouristController : Controller
    {
        private readonly ITouristRepository _touristRepository;
        private readonly IMapper _mapper;

        public TouristController(ITouristRepository touristRepository, IMapper mapper) 
        {
            _touristRepository = touristRepository;
            _mapper = mapper;
        }

        [HttpGet("api/Tourist/Tourists")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tourist>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTourists()
        {
            var tourists = _mapper.Map<List<TouristDto>>(_touristRepository.GetTourists());

            if (tourists.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tourists);
        }

        [HttpGet("api/Tourist/AdultTourists")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tourist>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetAdultTourists()
        {
            var tourists = _mapper.Map<List<TouristDto>>(_touristRepository.GetAdultTourists());

            if (tourists.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tourists);
        }

        [HttpGet("api/Tourist/ChildrenTourists")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tourist>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetChildrenTourists()
        {
            var tourists = _mapper.Map<List<TouristDto>>(_touristRepository.GetChildrenTourists());

            if (tourists.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tourists);
        }

        [HttpGet("api/Tourist/TouristsSearch")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tourist>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTouristsSearch([FromQuery] string? surname, [FromQuery] string? name, [FromQuery] string? patronymic, [FromQuery] DateTime? birthDate)
        {
            var tourists = _mapper.Map<List<TouristDto>>(_touristRepository.GetTouristsSearch(surname, name, patronymic, birthDate));

            if (tourists.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tourists);
        }

        [HttpGet("api/Tourist/{id}")]
        [ProducesResponseType(200, Type = typeof(Tourist))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTourist(int id)
        {
            if (!_touristRepository.TouristExists(id))
                return NotFound();

            var tourist = _mapper.Map<TouristDto>(_touristRepository.GetTourist(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tourist);
        }

        [HttpGet("api/Tourist/{id}/Data")]
        [ProducesResponseType(200, Type = typeof(TouristData))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTouristData(int id)
        {
            if (!_touristRepository.TouristExists(id))
                return NotFound();

            var touristData = _mapper.Map<TouristDataDto>(_touristRepository.GetTouristData(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(touristData);
        }

        [HttpPost("api/Tourist")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateTourist([FromBody] TouristDto touristCreate)
        {
            if (touristCreate == null)
                return BadRequest(ModelState);

            var tourists = _touristRepository.GetTouristTrimToUpper(touristCreate);

            if (tourists != null)
            {
                ModelState.AddModelError("", "Already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var touristMap = _mapper.Map<Tourist>(touristCreate);

            if (!_touristRepository.CheckingTourist(touristMap))
            {
                ModelState.AddModelError("", "Incorrect data");
                return StatusCode(422, ModelState);
            }

            var result = _touristRepository.CreateTourist(touristMap);

            if (result == -1)
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok(result);
        }

        [HttpPut("api/Tourist/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateTourist(int id, [FromBody] TouristDto updateTourist)
        {
            if (updateTourist == null)
                return BadRequest(ModelState);

            if (id != updateTourist.IdTourist)
                return BadRequest(ModelState);

            if (!_touristRepository.TouristExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var touristMap = _mapper.Map<Tourist>(updateTourist);

            if (!_touristRepository.CheckingTourist(touristMap))
            {
                ModelState.AddModelError("", "Incorrect data");
                return StatusCode(422, ModelState);
            }

            if (!_touristRepository.UpdateTourist(touristMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("api/Tourist/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteTourist(int id)
        {
            if (!_touristRepository.TouristExists(id))
                return NotFound();

            var touristToDelete = _touristRepository.GetTourist(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_touristRepository.DeleteTourist(touristToDelete))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
