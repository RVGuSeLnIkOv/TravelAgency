using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlX.XDevAPI;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using TravelAgencyApi.Data;
using TravelAgencyApi.Dto;
using TravelAgencyApi.Helper;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Repository
{
    public class TouristRepository : ITouristRepository
    {
        private readonly DataContext _context;

        public TouristRepository(DataContext context)
        {
            _context = context;
        }

        public bool CheckingTourist(Tourist tourist)
        {
            //проверка вводимых данных
            if (!string.IsNullOrEmpty(tourist.Surname)
                && !string.IsNullOrEmpty(tourist.Name)
                && !string.IsNullOrEmpty(tourist.BirthDate.ToString())
                && tourist.BirthDate != DateTime.MinValue
                && !string.IsNullOrEmpty(tourist.Gender)
                && Checking.CheckingGender(tourist.Gender))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int CreateTourist(Tourist createTourist)
        {
            var tourist = new Tourist
            {
                Surname = Checking.StrConversion(createTourist.Surname),
                Name = Checking.StrConversion(createTourist.Name),
                Patronymic = Checking.StrConversion(createTourist.Patronymic),
                ForeignSurname = Checking.StrConversion(createTourist.ForeignSurname),
                ForeignName = Checking.StrConversion(createTourist.ForeignName),
                ForeignPatronymic = Checking.StrConversion(createTourist.ForeignPatronymic),
                BirthDate = createTourist.BirthDate,
                Gender = createTourist.Gender[0].ToString().ToUpper(),
                IsChild = Checking.CheckAgeDifference(createTourist.BirthDate)
            };

            _context.Add(tourist);

            if (Save())
                return tourist.IdTourist;

            return -1;
        }

        public bool DeleteTourist(Tourist tourist)
        {
            _context.Remove(tourist);
            return Save();
        }

        public ICollection<Tourist> GetAdultTourists()
        {
            return _context.Tourists.Where(t => t.IsChild == false).OrderBy(t => t.IdTourist).ToList();
        }

        public ICollection<Tourist> GetChildrenTourists()
        {
            return _context.Tourists.Where(t => t.IsChild == true).OrderBy(t => t.IdTourist).ToList();
        }

        public Tourist GetTourist(int id)
        {
            return _context.Tourists.Where(t => t.IdTourist == id).FirstOrDefault();
        }

        public TouristData GetTouristData(int id)
        {
            return _context.TouristData.Where(td => td.IdTourist == id).FirstOrDefault();
        }

        public ICollection<Tourist> GetTourists()
        {
            return _context.Tourists.OrderBy(t => t.IdTourist).ToList();
        }

        public ICollection<Tourist> GetTouristsSearch(string? surname, string? name, string? patronymic, DateTime? birthDate)
        {
            var query = _context.Tourists.AsQueryable();

            if (!string.IsNullOrEmpty(surname))
            {
                surname = surname.Trim();
                query = query.Where(t => t.Surname.Contains(surname));
            } 

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim();
                query = query.Where(t => t.Name.Contains(name));
            } 

            if (!string.IsNullOrEmpty(patronymic))
            {
                patronymic = patronymic.Trim();
                query = query.Where(t => t.Patronymic.Contains(patronymic));
            }

            if (birthDate != DateTime.MinValue && birthDate != null)
                query = query.Where(t => t.BirthDate == birthDate);

            List<Tourist> result = query.ToList();

            return result;
        }

        public Tourist GetTouristTrimToUpper(TouristDto touristCreate)
        {
            return GetTourists()
                .Where(t => t.Surname.Trim().ToUpper() == touristCreate.Surname.TrimEnd().ToUpper()
                && t.Name.Trim().ToUpper() == touristCreate.Name.TrimEnd().ToUpper()
                && t.BirthDate == touristCreate.BirthDate)
                .FirstOrDefault();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool TouristExists(int id)
        {
            return _context.Tourists.Any(t => t.IdTourist == id);
        }

        public bool UpdateTourist(Tourist updateTourist)
        {
            var tourist = new Tourist
            {
                IdTourist = updateTourist.IdTourist,
                Surname = Checking.StrConversion(updateTourist.Surname),
                Name = Checking.StrConversion(updateTourist.Name),
                Patronymic = Checking.StrConversion(updateTourist.Patronymic),
                ForeignSurname = Checking.StrConversion(updateTourist.ForeignSurname),
                ForeignName = Checking.StrConversion(updateTourist.ForeignName),
                ForeignPatronymic = Checking.StrConversion(updateTourist.ForeignPatronymic),
                BirthDate = updateTourist.BirthDate,
                Gender = updateTourist.Gender[0].ToString().ToUpper(),
                IsChild = Checking.CheckAgeDifference(updateTourist.BirthDate)
            };

            _context.Update(tourist);

            return Save();
        }
    }
}
