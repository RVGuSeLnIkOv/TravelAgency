using System.Reflection;
using TravelAgencyApi.Data;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Helper;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;

        public EmployeeRepository(DataContext context)
        {
            _context = context;
        }

        public bool CheckingEmployee(Employee employee)
        {
            //проверка вводимых данных
            if (!string.IsNullOrEmpty(employee.Surname)
                && !string.IsNullOrEmpty(employee.Name)
                && !string.IsNullOrEmpty(employee.BirthDate.ToString())
                && employee.BirthDate != DateTime.MinValue
                && !string.IsNullOrEmpty(employee.Post)
                && Checking.CheckingPost(employee.Post)
                && Checking.CheckingInt(employee.Salary.ToString(), 0, Int32.MaxValue) != -1
                && (string.IsNullOrEmpty(employee.PhoneNumber) || Checking.CheckingPhoneNumber(employee.PhoneNumber)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CreateEmployee(Employee createEmployee)
        {
            var employee = new Employee
            {
                Surname = Checking.StrConversion(createEmployee.Surname),
                Name = Checking.StrConversion(createEmployee.Name),
                Patronymic = Checking.StrConversion(createEmployee.Patronymic),
                BirthDate = createEmployee.BirthDate,
                Salary = Checking.CheckingInt(createEmployee.Salary.ToString(), 0, Int32.MaxValue),
                Post = Checking.StrConversion(createEmployee.Post),
                PhoneNumber = createEmployee.PhoneNumber
            };

            _context.Add(employee);

            if (Save())
                return employee.IdEmployee;

            return -1;
        }

        public bool DeleteEmployee(Employee employee)
        {
            _context.Remove(employee);
            return Save();
        }

        public bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.IdEmployee == id);
        }

        public Employee GetEmployee(int id)
        {
            return _context.Employees.Where(e => e.IdEmployee == id).FirstOrDefault();
        }

        public ICollection<Employee> GetEmployees()
        {
            return _context.Employees.OrderBy(e => e.IdEmployee).ToList();
        }

        public ICollection<Employee> GetEmployeesSearch(string? surname = null, string? name = null, string? patronymic = null, DateTime? birthDate = null)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(surname))
            {
                surname = surname.Trim();
                query = query.Where(e => e.Surname.Contains(surname));
            }

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim();
                query = query.Where(e => e.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(patronymic))
            {
                patronymic = patronymic.Trim();
                query = query.Where(e => e.Patronymic.Contains(patronymic));
            }

            if (birthDate != DateTime.MinValue && birthDate != null)
                query = query.Where(e => e.BirthDate == birthDate);

            List<Employee> result = query.ToList();

            return result;
        }

        public Employee GetEmployeeTrimToUpper(EmployeeDto employeeCreate)
        {
            return GetEmployees()
                .Where(e => e.Surname.Trim().ToUpper() == employeeCreate.Surname.TrimEnd().ToUpper()
                && e.Name.Trim().ToUpper() == employeeCreate.Name.TrimEnd().ToUpper()
                && e.BirthDate == employeeCreate.BirthDate)
                .FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateEmployee(Employee updateEmployee)
        {
            var employee = new Employee
            {
                IdEmployee = updateEmployee.IdEmployee,
                Surname = Checking.StrConversion(updateEmployee.Surname),
                Name = Checking.StrConversion(updateEmployee.Name),
                Patronymic = Checking.StrConversion(updateEmployee.Patronymic),
                BirthDate = updateEmployee.BirthDate,
                Salary = updateEmployee.Salary,
                Post = Checking.StrConversion(updateEmployee.Post),
                PhoneNumber = updateEmployee.PhoneNumber
            };

            _context.Update(employee);

            return Save();
        }
    }
}
