using TravelAgencyApi.Models;

namespace TravelAgencyApi.Interfaces
{
    public interface IEmployeeDataRepository
    {
        EmployeeData GetEmployeeData(string login);
        string CreateEmployeeData(EmployeeData employeeData);
        bool UpdateEmployeeData(EmployeeData employeeData);
        bool Save();
        bool EmployeeDataExists(int id);
        int GetAuth(string login, string password);
        bool CheckingEmployeeData(string login, string password);
    }
}
