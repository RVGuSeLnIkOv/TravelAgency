using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;
using TravelAgencyApi.Repository;

namespace TravelAgencyApi.Controllers
{
    [ApiController]
    public class TouristDataController : Controller
    {
        private readonly ITouristDataRepository _touristDataRepository;
        private readonly ITouristRepository _touristRepository;
        private readonly IMapper _mapper;

        public TouristDataController(ITouristDataRepository touristDataRepository,
            ITouristRepository touristRepository,
            IMapper mapper)
        {
            _touristDataRepository = touristDataRepository;
            _touristRepository = touristRepository;
            _mapper = mapper;
        }

        [HttpPost("api/TouristData")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateTouristData([FromBody] TouristDataDto touristDataCreate)
        {
            if (touristDataCreate == null)
                return BadRequest(ModelState);

            if (_touristDataRepository.TouristHaveData(touristDataCreate.IdTourist))
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var touristDataMap = _mapper.Map<TouristData>(touristDataCreate);

            if (!_touristRepository.TouristExists(touristDataMap.IdTourist))
                return NotFound();

            if (!_touristDataRepository.CheckingTouristData(touristDataMap))
            {
                ModelState.AddModelError("", "Incorrect data");
                return StatusCode(422, ModelState);
            }

            if (!_touristDataRepository.CreateTouristData(touristDataMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("api/TouristData/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateTouristData(int id, [FromBody] TouristDataDto updateTouristData)
        {
            if (updateTouristData == null)
                return BadRequest(ModelState);

            if (id != updateTouristData.IdTouristData)
                return BadRequest(ModelState);

            if (!_touristDataRepository.TouristDataExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var touristDataMap = _mapper.Map<TouristData>(updateTouristData);

            if (!_touristDataRepository.CheckingTouristData(touristDataMap))
            {
                ModelState.AddModelError("", "Incorrect data");
                return StatusCode(422, ModelState);
            }

            if (!_touristDataRepository.UpdateTouristData(touristDataMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
