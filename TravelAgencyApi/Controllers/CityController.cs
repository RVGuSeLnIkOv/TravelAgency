using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;
using TravelAgencyApi.Repository;

namespace TravelAgencyApi.Controllers
{
    [ApiController]
    public class CityController : Controller
    {
        private readonly ICityRepository _cityRepository;
        private readonly IResidenceRepository _residenceRepository;
        private readonly IMapper _mapper;

        public CityController(ICityRepository cityRepository,
            IResidenceRepository residenceRepository,
            IMapper mapper)
        {
            _cityRepository = cityRepository;
            _residenceRepository = residenceRepository;
            _mapper = mapper;
        }

        [HttpGet("api/City/{id}")]
        [ProducesResponseType(200, Type = typeof(City))]
        [ProducesResponseType(400)]
        public IActionResult GetCity(int id)
        {
            var city = _mapper.Map<CityDto>(_cityRepository.GetCity(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(city);
        }

        [HttpGet("api/City/{idCity}/Residences")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Residence>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetResidences(int idCity)
        {
            var residences = _mapper.Map<List<ResidenceDto>>(_residenceRepository.GetResidences(idCity));

            if (residences.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(residences);
        }
    }
}
