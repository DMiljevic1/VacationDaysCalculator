using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using VacationDaysCalculatorWebAPI.DatabaseContext;
using VacationDaysCalculatorWebAPI.Repositories;
using VacationDaysCalculatorWebAPI.ValidationModels;

namespace VacationDaysCalculatorWebAPI.Services
{
    public class AdminService
    {
        private readonly int TOTAL_GIVEN_VACATION_PER_YEAR = 20;
        private readonly AdminRepository _adminRepository;

        public AdminService(AdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public AdminDetails GetAdminDetails(int userId)
        {
            var user = _adminRepository.GetUserById(userId);
            if (user == null)
                return null;
            var adminDetails = new AdminDetails();
            adminDetails.FirstName = user.FirstName;
            adminDetails.LastName = user.LastName;
            adminDetails.Email = user.Email;
            adminDetails.EmployeeVacationDays = GetEmployeeVacationByStatus(VacationStatus.Pending);
            return adminDetails;
        }
        public List<Vacation> GetEmployeeVacationByStatus(VacationStatus status)
        {
            return _adminRepository.GetEmployeeVacationByStatus(status);
        }
        //get all approved vacations not just vacations with status approved
        public List<Vacation> GetApprovedVacations()
        {
            return _adminRepository.GetApprovedVacations();
        }
        public void AddUser(UserDetails userDetails)
        {
            List<ValidationError> errors = ValidateUser(userDetails);
            if (!errors.Any())
            {
                var user = FillUserProperties(userDetails);
                _adminRepository.AddUser(user);
            }

        }
        private User FillUserProperties(UserDetails userDetails)
        {
            var user = new User();
            var currentYear = DateTime.Today.Year;
            user.UserName = userDetails.Username;
            user.Password = HashUserPassword(userDetails.Password);
            user.Email = userDetails.Email;
            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.CurrentYear = currentYear;
            user.RemainingDaysOffCurrentYear = userDetails.RemainingDaysOffCurrentYear;
            user.RemainingDaysOffLastYear = userDetails.RemainingDaysOffLastYear;
            user.Role = userDetails.Role;
            return user;
        }
        private string HashUserPassword(string plainTextPassword)
        {
            string hashedPassword;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                hashedPassword = GetHash(sha256Hash, plainTextPassword);
            }
            return hashedPassword;
        }
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
        private List<ValidationError> ValidateUser(UserDetails userDetails)
        {
            var validationErrors = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(userDetails.Username))
                validationErrors.Add(new ValidationError { Description = "Please insert username!" });

            if (string.IsNullOrWhiteSpace(userDetails.Password))
                validationErrors.Add(new ValidationError { Description = "Please insert password!" });

            if (userDetails.Password != userDetails.ConfirmPassword)
                validationErrors.Add(new ValidationError { Description = "Passwords value must match!" });

            if (string.IsNullOrWhiteSpace(userDetails.FirstName))
                validationErrors.Add(new ValidationError { Description = "Please insert first name!" });

            if (string.IsNullOrWhiteSpace(userDetails.LastName))
                validationErrors.Add(new ValidationError { Description = "Please insert last name!" });

            if (userDetails.Role == null)
                validationErrors.Add(new ValidationError { Description = "Please choose user's role!" });

            if (userDetails.RemainingDaysOffCurrentYear > TOTAL_GIVEN_VACATION_PER_YEAR && userDetails.Role == "Employee")
                validationErrors.Add(new ValidationError { Description = "User can have max " + TOTAL_GIVEN_VACATION_PER_YEAR + " days of vacation per year!" });

            if (userDetails.RemainingDaysOffCurrentYear < TOTAL_GIVEN_VACATION_PER_YEAR && userDetails.RemainingDaysOffLastYear != 0 && userDetails.Role == "Employee")
                validationErrors.Add(new ValidationError { Description = "If vacation from current year is less than " + TOTAL_GIVEN_VACATION_PER_YEAR + ", vacation from last year must be 0!" });

            return validationErrors;
        }
    }
}
