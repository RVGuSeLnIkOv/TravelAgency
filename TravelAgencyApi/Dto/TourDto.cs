namespace TravelAgencyApi.Dto
{
    public class TourDto
    {
        public int IdTour { get; set; }
        public int IdTourOperator { get; set; }
        public int IdResidence { get; set; }
        public int IdTypeMeal { get; set; }
        public DateTime? CheckinDate { get; set; }
        public DateTime? CheckoutDate { get; set; }
        public int? Duration { get; set; }
        public string? Notes { get; set; }
        public bool IsArchive { get; set; }
    }
}
