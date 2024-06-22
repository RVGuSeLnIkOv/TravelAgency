using Org.BouncyCastle.Crypto.Generators;
using System.Globalization;
using System.Text.RegularExpressions;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Helper
{
    public class Checking
    {
        //метод получения хеш-кода
        public static string GetHash(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        //метод преобразования строки для добавления в БД
        public static string StrConversion(string input)
        {
            if (input == null)
                return null;

            input = input.Trim();
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string output = textInfo.ToLower(input);
            output = textInfo.ToTitleCase(output);

            return output;
        }

        //метод проверки номера заграничного паспорта
        public static bool CheckingForeignPassport(string foreignPassport)
        {
            if (string.IsNullOrEmpty(foreignPassport.Trim()))
                return true;

            string patternForeign = @"^\d{2}[\s№]\d{7}$";
            bool isCorrect = Regex.IsMatch(foreignPassport, patternForeign);

            return isCorrect;
        }

        //метод проверки номера внутреннего паспорта
        public static bool CheckingInternalPassport(string internalPassport)
        {
            if (string.IsNullOrEmpty(internalPassport.Trim()))
                return true;

            string patternInternal = @"^\d{4}\s\d{6}$";
            bool isCorrect = Regex.IsMatch(internalPassport, patternInternal);

            return isCorrect;
        }

        //метод проверки номера email
        public static bool CheckingEmail(string email)
        {
            if (string.IsNullOrEmpty(email.Trim()))
                return true;

            string patternEmail = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            bool isCorrect = Regex.IsMatch(email, patternEmail);

            return isCorrect;
        }

        //метод проверки номера свидетельства о рождении
        public static bool CheckingBirthdayCertificate(string birthdayCertificate)
        {
            if (string.IsNullOrEmpty(birthdayCertificate.Trim()))
                return true;
            string patternBirthdayCertificate = @"^\d{11}$";
            bool isCorrect = Regex.IsMatch(birthdayCertificate, patternBirthdayCertificate);

            return isCorrect;
        }

        //метод проверки номера телефона
        public static bool CheckingPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber.Trim()))
                return true;
            string patternPhoneNumber = @"^9\d{9}$";
            bool isCorrect = Regex.IsMatch(phoneNumber, patternPhoneNumber);

            return isCorrect;
        }

        //метод проверки пола
        public static bool CheckingGender(string gender)
        {
            gender = gender.Trim().ToLower();

            if (gender == "м" || gender == "ж" || gender[..3] == "муж" || gender[..3] == "жен")
                return true;

            return false;
        }

        //метод проверки должности
        public static bool CheckingPost(string post)
        {
            post = post.Trim().ToLower();

            if (post == "директор" || post == "администратор")
                return true;

            return false;
        }

        //метод проверки целого числа
        public static int CheckingInt(string input, int minValue, int maxValue)
        {
            try
            {
                input = input.Trim();
                int output = int.Parse(input);
                if (output < minValue || output > maxValue)
                    return -1;
                return output;
            }
            catch
            {
                return -1;
            }
        }

        //метод для проверки того, является ли турист ребенков
        public static bool CheckAgeDifference(DateTime date)
        {
            DateTime currentDate = DateTime.Now;
            TimeSpan difference = currentDate - date;
            int yearsDifference = (int)(difference.TotalDays / 365.25); // Учитываем високосные годы

            return yearsDifference < 14;
        }
    }
}
