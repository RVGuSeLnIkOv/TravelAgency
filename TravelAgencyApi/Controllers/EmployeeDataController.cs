using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;
using TravelAgencyApi.Repository;

namespace TravelAgencyApi.Controllers
{
    [ApiController]
    public class EmployeeDataController : Controller
    {
        private readonly IEmployeeDataRepository _employeeDataRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeDataController(IEmployeeDataRepository employeeDataRepository,
            IEmployeeRepository employeeRepository,
            IMapper mapper)
        {
            _employeeDataRepository = employeeDataRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet("api/EmployeeData/Auth/{login}/{password}")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult GetAuth(string login, string password)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_employeeDataRepository.CheckingEmployeeData(login, password))
            {
                ModelState.AddModelError("", "Incorrect data");
                return StatusCode(422, ModelState);
            }

            return Ok(_employeeDataRepository.GetAuth(login, password));
        }

        [HttpGet("api/EmployeeData/{login}")]
        [ProducesResponseType(200, Type = typeof(EmployeeData))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        public IActionResult GetEmployeeData(string login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeeDataMap = _mapper.Map<EmployeeData>(_employeeDataRepository.GetEmployeeData(login));

            if (employeeDataMap == null)
                return NotFound();

            return Ok(employeeDataMap);
        }

        [HttpPost("api/EmployeeData")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateEmployeeData([FromBody] EmployeeDataDto employeeDataCreate)
        {
            if (employeeDataCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employeeDataMap = _mapper.Map<EmployeeData>(employeeDataCreate);

            if (!_employeeRepository.EmployeeExists(employeeDataMap.IdEmployee))
                return NotFound();

            var result = _employeeDataRepository.CreateEmployeeData(employeeDataMap);

            if (result == "-1")
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok(result);
        }

        [HttpPut("api/EmployeeData/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateEmployeeData(int id, [FromBody] EmployeeDataDto updateEmployeeData)
        {
            if (updateEmployeeData == null)
                return BadRequest(ModelState);

            if (id != updateEmployeeData.IdEmployeeData)
                return BadRequest(ModelState);

            if (!_employeeDataRepository.EmployeeDataExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var employeeDataMap = _mapper.Map<EmployeeData>(updateEmployeeData);

            if (!_employeeDataRepository.CheckingEmployeeData(employeeDataMap.Login, employeeDataMap.Password))
            {
                ModelState.AddModelError("", "Incorrect data");
                return StatusCode(422, ModelState);
            }

            if (!_employeeDataRepository.UpdateEmployeeData(employeeDataMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
