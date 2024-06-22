using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;
using TravelAgencyApi.Repository;

namespace TravelAgencyApi.Controllers
{
    [ApiController]
    public class TypeMealController : Controller
    {
        private readonly ITypeMealRepository _typeMealRepository;
        private readonly IMapper _mapper;

        public TypeMealController(ITypeMealRepository typeMealRepository, IMapper mapper)
        {
            _typeMealRepository = typeMealRepository;
            _mapper = mapper;
        }

        [HttpGet("api/TypeMeal/TypesMeal")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TypeMeal>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTypeMeal()
        {
            var typesMeal = _mapper.Map<List<TypeMealDto>>(_typeMealRepository.GetTypesMeal());

            if (typesMeal.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(typesMeal);
        }

        [HttpGet("api/TypeMeal/{id}")]
        [ProducesResponseType(200, Type = typeof(TypeMeal))]
        [ProducesResponseType(400)]
        public IActionResult GetTypeMeal(int id)
        {
            var typeMeal = _mapper.Map<TypeMealDto>(_typeMealRepository.GetTypeMeal(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(typeMeal);
        }
    }
}
