using TravelAgencyApi.Models;

namespace TravelAgencyApi.Interfaces
{
    public interface ITypeMealRepository
    {
        ICollection<TypeMeal> GetTypesMeal();
        int GetIdTypeMeal(string typeMealName);
        TypeMeal GetTypeMeal(int id);
    }
}
