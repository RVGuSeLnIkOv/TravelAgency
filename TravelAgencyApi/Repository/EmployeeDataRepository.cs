using TravelAgencyApi.Data;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Helper;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Repository
{
    public class EmployeeDataRepository : IEmployeeDataRepository
    {
        private readonly DataContext _context;

        public EmployeeDataRepository(DataContext context)
        {
            _context = context;
        }

        public bool CheckingEmployeeData(string login, string password)
        {
            if (!string.IsNullOrEmpty(login)
                && !string.IsNullOrEmpty(password)
                && password.Length > 8)
            {
                return true;
            }

            return false;
        }

        public string CreateEmployeeData(EmployeeData createEmployeeData)
        {
            string loginStr = "user_" + DateTime.Now.ToString("yyyyMMddHHmmss");

            var employeeData = new EmployeeData
            {
                IdEmployee = createEmployeeData.IdEmployee,
                Login = loginStr,
                Password = Checking.GetHash("1234567890")
            };

            _context.Add(employeeData);

            if (Save())
                return employeeData.Login;

            return "-1";
        }

        public bool EmployeeDataExists(int id)
        {
            return _context.EmployeeData.Any(ed => ed.IdEmployeeData == id);
        }

        public int GetAuth(string login, string password)
        {
            var employeeData = _context.EmployeeData.Where(ed => ed.Login == login)
                    .FirstOrDefault();

            if (employeeData != null)
            {
                string hashPass = employeeData.Password;

                if (BCrypt.Net.BCrypt.Verify(password, hashPass))
                {
                    var employee = _context.Employees.Where(e => e.IdEmployee == employeeData.IdEmployee).FirstOrDefault();

                    if (employee != null)
                    {
                        return employee.IdEmployee;
                    }
                }
            }
            return -1;
        }

        public EmployeeData GetEmployeeData(string login)
        {
            var employeeData = _context.EmployeeData.Where(ed => ed.Login == login).FirstOrDefault();



            if (employeeData != null)
            {
                var result = new EmployeeData
                {
                    IdEmployeeData = employeeData.IdEmployeeData,
                    IdEmployee = employeeData.IdEmployee
                };

                return result;
            }

            return null;
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateEmployeeData(EmployeeData updateEmployeeData)
        {
            var employeeData = new EmployeeData
            {
                IdEmployeeData = updateEmployeeData.IdEmployeeData,
                IdEmployee = updateEmployeeData.IdEmployee,
                Login = updateEmployeeData.Login,
                Password = Checking.GetHash(updateEmployeeData.Password),
            };
            _context.Update(employeeData);

            return Save();
        }
    }
}
