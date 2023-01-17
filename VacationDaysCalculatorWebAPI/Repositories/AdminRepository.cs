﻿using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using VacationDaysCalculatorWebAPI.DatabaseContext;
using VacationDaysCalculatorWebAPI.ValidationModels;

namespace VacationDaysCalculatorWebAPI.Repositories
{
    public class AdminRepository
    {
        private readonly int TOTAL_GIVEN_VACATION_PER_YEAR = 20;
        private readonly VacationDbContext _vacationDbContext;

        public AdminRepository(VacationDbContext vacationDbContext)
        {
            _vacationDbContext = vacationDbContext;
        }

        public AdminDetails GetAdminDetails(int userId)
        {
            var user = _vacationDbContext.Users.FirstOrDefault(u => u.Id == userId);
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
            var employeeVacation = _vacationDbContext.Vacation.Include(vd => vd.User).Where(vd => vd.Status.Equals(status)).ToList();
            return employeeVacation;
        }
        public void AddUser(UserDetails userDetails)
        {
            List<ValidationError> errors = ValidateUser(userDetails);
            if(!errors.Any())
            {
                var user = fillUserProperties(userDetails);
                _vacationDbContext.Users.Add(user);
                _vacationDbContext.SaveChanges();
            }

        }
        private User fillUserProperties(UserDetails userDetails)
        {
            var user = new User();
            var currentYear = DateTime.Today.Year;
            user.UserName = userDetails.Username;
            user.Password = userDetails.Password;
            user.Email = userDetails.Email;
            user.FirstName = userDetails.FirstName;
            user.LastName = userDetails.LastName;
            user.CurrentYear = currentYear;
            user.RemainingDaysOffCurrentYear = userDetails.RemainingDaysOffCurrentYear;
            user.RemainingDaysOffLastYear = userDetails.RemainingDaysOffLastYear;
            user.Role = userDetails.Role;
            return user;
        }
        private List<ValidationError> ValidateUser(UserDetails userDetails)
        {
            var validationErrors = new List<ValidationError>();

            if (String.IsNullOrWhiteSpace(userDetails.Username))
                validationErrors.Add(new ValidationError { Description = "Please insert username!" });

            if (String.IsNullOrWhiteSpace(userDetails.Password))
                validationErrors.Add(new ValidationError { Description = "Please insert password!" });

            if (userDetails.Password != userDetails.ConfirmPassword)
                validationErrors.Add(new ValidationError { Description = "Passwords value must match!" });

            if (String.IsNullOrWhiteSpace(userDetails.FirstName))
                validationErrors.Add(new ValidationError { Description = "Please insert first name!" });

            if (String.IsNullOrWhiteSpace(userDetails.LastName))
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
