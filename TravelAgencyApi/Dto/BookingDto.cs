namespace TravelAgencyApi.Dto
{
    public class BookingDto
    {
        public int IdBooking { get; set; }
        public int IdTour { get; set; }
        public int IdTourist { get; set; }
        public int IdEmployee { get; set; }
        public DateTime? Date { get; set; }
        public int? Cost { get; set; }
    }
}
