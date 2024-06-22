using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;
using TravelAgencyApi.Repository;

namespace TravelAgencyApi.Controllers
{
    [ApiController]
    public class TourOperatorController : Controller
    {
        private readonly ITourOperatorRepository _tourOperatorRepository;
        private readonly IMapper _mapper;

        public TourOperatorController(ITourOperatorRepository tourOperatorRepository, IMapper mapper)
        {
            _tourOperatorRepository = tourOperatorRepository;
            _mapper = mapper;
        }

        [HttpGet("api/TourOperator/TourOperators")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TourOperator>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTourOperators()
        {
            var tourOperators = _mapper.Map<List<TourOperatorDto>>(_tourOperatorRepository.GetTourOperators());

            if (tourOperators.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tourOperators);
        }

        [HttpGet("api/TourOperator/{id}")]
        [ProducesResponseType(200, Type = typeof(TourOperator))]
        [ProducesResponseType(400)]
        public IActionResult GetTourOperator(int id)
        {
            var tourOperator = _mapper.Map<TourOperatorDto>(_tourOperatorRepository.GetTourOperator(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tourOperator);
        }
    }
}
