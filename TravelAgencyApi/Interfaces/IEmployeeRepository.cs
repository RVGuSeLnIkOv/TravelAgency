using TravelAgencyApi.Dto;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Interfaces
{
    public interface IEmployeeRepository
    {
        ICollection<Employee> GetEmployees();
        ICollection<Employee> GetEmployeesSearch
            (string? surname = null,
            string? name = null,
            string? patronymic = null,
            DateTime? birthDate = null);
        Employee GetEmployee(int id);
        Employee GetEmployeeTrimToUpper(EmployeeDto employeeDto);
        int CreateEmployee(Employee employee);
        bool CheckingEmployee(Employee employee);
        bool UpdateEmployee(Employee employee);
        bool DeleteEmployee(Employee employee);
        bool EmployeeExists(int id);
        bool Save();
    }
}
