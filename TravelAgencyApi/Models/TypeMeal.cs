using System.ComponentModel.DataAnnotations;

namespace TravelAgencyApi.Models
{
    public class TypeMeal
    {
        [Key]
        public int IdTypeMeal { get; set; }
        public string? TypeMealName { get; set; }
        public string? TypeMealAbbrName { get; set; }

        public virtual ICollection<Tour> Tours { get; set; }
    }
}
