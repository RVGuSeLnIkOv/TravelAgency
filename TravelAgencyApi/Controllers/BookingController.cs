using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;
using TravelAgencyApi.Repository;

namespace TravelAgencyApi.Controllers
{
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IMapper _mapper;

        public BookingController(IBookingRepository bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        [HttpGet("api/Booking/{id}")]
        [ProducesResponseType(200, Type = typeof(Booking))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBooking(int id)
        {
            if (!_bookingRepository.BookingExists(id))
                return NotFound();

            var booking = _mapper.Map<BookingDto>(_bookingRepository.GetBooking(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(booking);
        }

        [HttpPost("api/Booking")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateBooking([FromBody] BookingDto bookingCreate)
        {
            if (bookingCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookingMap = _mapper.Map<Booking>(bookingCreate);

            if (!_bookingRepository.CheckingBooking(bookingMap))
            {
                ModelState.AddModelError("", "Incorrect data");
                return StatusCode(422, ModelState);
            }

            if (!_bookingRepository.CreateBooking(bookingMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("api/Booking/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateBooking(int id, [FromBody] BookingDto updateBooking)
        {
            if (updateBooking == null)
                return BadRequest(ModelState);

            if (id != updateBooking.IdBooking)
                return BadRequest(ModelState);

            if (!_bookingRepository.BookingExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var bookingMap = _mapper.Map<Booking>(updateBooking);

            if (!_bookingRepository.CheckingBooking(bookingMap))
            {
                ModelState.AddModelError("", "Incorrect data");
                return StatusCode(422, ModelState);
            }

            if (!_bookingRepository.UpdateBooking(bookingMap))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("api/Booking/{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteBooking(int id)
        {
            if (!_bookingRepository.BookingExists(id))
                return NotFound();

            var bookingToDelete = _bookingRepository.GetBooking(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_bookingRepository.DeleteBooking(bookingToDelete))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
