using Microsoft.EntityFrameworkCore;
using System.Drawing;
using TravelAgencyApi.Data;
using TravelAgencyApi.Helper;
using TravelAgencyApi.Interfaces;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Repository
{
    public class TouristDataRepository : ITouristDataRepository
    {
        private readonly DataContext _context;

        public TouristDataRepository(DataContext context)
        {
            _context = context;
        }

        public bool CheckingTouristData(TouristData touristData)
        {
            if (!Checking.CheckingPhoneNumber(touristData.PhoneNumber))
            {
                return false;
            }

            if (!Checking.CheckingEmail(touristData.EmailAddress))
            {
                return false;
            }

            if (!Checking.CheckingBirthdayCertificate(touristData.BirthCertificateNumber))
            {
                return false;
            }

            if (!Checking.CheckingInternalPassport(touristData.InternalPassportNumber))
            {
                return false;
            }

            if (!Checking.CheckingForeignPassport(touristData.ForeignPassportNumber))
            {
                return false;
            }

            if (string.IsNullOrEmpty(touristData.ForeignPassportNumber.Trim()) && string.IsNullOrEmpty(touristData.InternalPassportNumber.Trim()) && string.IsNullOrEmpty(touristData.BirthCertificateNumber.Trim()))
            {
                return false;
            }

            return true;
        }

        public bool CreateTouristData(TouristData createTouristData)
        {
            if (!string.IsNullOrEmpty(createTouristData.Address))
                createTouristData.Address = Checking.StrConversion(createTouristData.Address);

            _context.Add(createTouristData);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool TouristDataExists(int id)
        {
            return _context.TouristData.Any(td => td.IdTouristData == id);
        }

        public bool TouristHaveData(int idTourist)
        {
            return _context.TouristData.Any(td => td.IdTourist == idTourist);
        }

        public bool UpdateTouristData(TouristData touristData)
        {
            if (!string.IsNullOrEmpty(touristData.Address))
                touristData.Address = Checking.StrConversion(touristData.Address);

            _context.Update(touristData);

            return Save();
        }
    }
}
