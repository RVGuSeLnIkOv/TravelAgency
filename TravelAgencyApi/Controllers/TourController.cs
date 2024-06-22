using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;
using TravelAgencyApi.Repository;

namespace TravelAgencyApi.Controllers
{
    [ApiController]
    public class TourController : Controller
    {
        private readonly ITourRepository _tourRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public TourController(ITourRepository tourRepository,
            IBookingRepository bookingRepository,
            IMapper mapper)
        {
            _tourRepository = tourRepository;
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        [HttpGet("api/Tour/ActiveTours")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tour>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetActiveTours()
        {
            var tours = _mapper.Map<List<TourDto>>(_tourRepository.GetActiveTours());

            if (tours.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tours);
        }

        [HttpGet("api/Tour/ArchiveTours")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tour>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetArchiveTours()
        {
            var tours = _mapper.Map<List<TourDto>>(_tourRepository.GetArchiveTours());

            if (tours.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tours);
        }

        [HttpGet("api/Tour/{id}")]
        [ProducesResponseType(200, Type = typeof(Tour))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTour(int id)
        {
            if (!_tourRepository.TourExists(id))
                return NotFound();

            var tour = _mapper.Map<TourDto>(_tourRepository.GetTour(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tour);
        }

        [HttpGet("api/Tour/{id}/Bookings")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Booking>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBookings(int id)
        {
            if (!_tourRepository.TourExists(id))
                return NotFound();

            var bookings = _mapper.Map<List<BookingDto>>(_bookingRepository.GetBookings(id));

            if (bookings.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(bookings);
        }

        [HttpGet("api/Tour/{id}/Tourists")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Tourist>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTourists(int id)
        {
            if (!_tourRepository.TourExists(id))
                return NotFound();

            var tourists = _mapper.Map<List<TouristDto>>(_tourRepository.GetTouristsOnTour(id));

            if (tourists.Count == 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(tourists);
        }

        [HttpPost("api/Tour")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateTour([FromQuery] List<int> tourists, [FromBody] TourDto tourCreate)
        {
            if (tourCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var tourMap = _mapper.Map<Tour>(tourCreate);

            if (!_tourRepository.CheckingTour(tourMap))
            {
                ModelState.AddModelError("", "Incorrect data");
                return StatusCode(422, ModelState);
            }

            var result = _tourRepository.CreateTour(tourMap, tourists);

            if (result == -1)
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok(result);
        }

        [HttpPut("api/Tour/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateTour(int id, [FromBody] TourDto updateTour)
        {
            if (updateTour == null)
                return BadRequest(ModelState);

            if (id != updateTour.IdTour)
                return BadRequest(ModelState);

            if (!_tourRepository.TourExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var tourMap = _mapper.Map<Tour>(updateTour);

            if (!_tourRepository.CheckingTour(tourMap))
            {
                ModelState.AddModelError("", "Incorrect data");
                return StatusCode(422, ModelState);
            }

            if (!_tourRepository.UpdateTour(tourMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("api/Tour/{id}/Archive")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult ArchiveTour(int id, [FromBody] TourDto archiveTour)
        {
            if (archiveTour == null)
                return BadRequest(ModelState);

            if (id != archiveTour.IdTour)
                return BadRequest(ModelState);

            if (!_tourRepository.TourExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var tourMap = _mapper.Map<Tour>(archiveTour);

            if (!_tourRepository.ArchiveTour(tourMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("api/Tour/{id}/Unarchive")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UnarchiveTour(int id, [FromBody] TourDto unarchiveTour)
        {
            if (unarchiveTour == null)
                return BadRequest(ModelState);

            if (id != unarchiveTour.IdTour)
                return BadRequest(ModelState);

            if (!_tourRepository.TourExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var tourMap = _mapper.Map<Tour>(unarchiveTour);

            if (!_tourRepository.UnarchiveTour(tourMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
