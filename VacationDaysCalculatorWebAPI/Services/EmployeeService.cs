using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using VacationDaysCalculatorWebAPI.DatabaseContext;
using VacationDaysCalculatorWebAPI.Repositories;

namespace VacationDaysCalculatorWebAPI.Services
{
    public class EmployeeService
    {
        private readonly int TOTAL_GIVEN_VACATION_PER_YEAR = 20;
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeService(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public List<User> GetUsers()
        {
            return _employeeRepository.GetUsers();
        }
        public EmployeeDetails GetEmployeeDetails(int userId)
        {
            var employee = _employeeRepository.GetUserById(userId);
            var employeeDetails = new EmployeeDetails();
            employeeDetails.FirstName = employee.FirstName;
            employeeDetails.LastName = employee.LastName;
            employeeDetails.Email = employee.Email;
            employeeDetails.RemainingDaysOffLastYear = employee.RemainingDaysOffLastYear;
            employeeDetails.RemainingDaysOffCurrentYear = employee.RemainingDaysOffCurrentYear;
            employeeDetails.VacationDays = GetVacationRequestsWithPendingOrApprovedStatus(userId);
            return employeeDetails;
        }
        public List<Vacation> GetVacationRequestsWithPendingOrApprovedStatus(int userId)
        {
            return _employeeRepository.GetVacationRequestsWithPendingOrApprovedStatus(userId);
        }

        public void SetVacationStatus(DateTime currentDate)
        {
            var vacations = _employeeRepository.GetVacationRequests();
            var yesterday = currentDate.AddDays(-1);
            foreach (var vacation in vacations)
            {
                if (vacation.VacationFrom.Equals(currentDate) && vacation.Status == VacationStatus.Approved)
                {
                    vacation.Status = VacationStatus.OnVacation;
                    UpdateEmployeeVacationStatus(vacation);
                }
                else if (vacation.VacationTo.Equals(yesterday.Date) && vacation.Status == VacationStatus.OnVacation)
                {
                    vacation.Status = VacationStatus.Arhived;
                    UpdateEmployeeVacationStatus(vacation);
                }
                else if (vacation.VacationFrom.Equals(currentDate) && vacation.Status == VacationStatus.Pending)
                {
                    vacation.Status = VacationStatus.Cancelled;
                    UpdateEmployeeVacationStatus(vacation);
                }
            }
        }
        private void UpdateEmployeeVacationStatus(Vacation vacation)
        {
            var vacationForUpdate = GetVacationByVacationId(vacation.Id);
            if (vacationForUpdate != null)
            {
                vacationForUpdate.Status = vacation.Status;
                if (vacationForUpdate.Status == VacationStatus.Cancelled)
                {
                    int vacationDaysToRestore = CalculateTotalVacationForGivenPeriod(vacation.VacationFrom, vacation.VacationTo);
                    RestoreRemainingVacation(vacation.UserId, vacationDaysToRestore);
                }

                _employeeRepository.SaveChanges();
            }
        }
        public void ApproveVacation(Vacation vacation)
        {
            var vacationForUpdate = GetVacationByVacationId(vacation.Id);
            if (vacationForUpdate != null)
            {
                vacationForUpdate.Status = vacation.Status;
                vacationForUpdate.ApprovedBy = vacation.ApprovedBy;

                _employeeRepository.SaveChanges();
            }
        }
        public void CancelVacation(Vacation vacation)
        {
            var vacationForUpdate = GetVacationByVacationId(vacation.Id);
            if (vacationForUpdate != null)
            {
                vacationForUpdate.Status = vacation.Status;
                int vacationDaysToRestore = CalculateTotalVacationForGivenPeriod(vacation.VacationFrom, vacation.VacationTo);
                RestoreRemainingVacation(vacation.UserId, vacationDaysToRestore);

                _employeeRepository.SaveChanges();
            }
        }
        public Vacation GetVacationByVacationId(int vacationId)
        {
            return _employeeRepository.GetVacationByVacationId(vacationId);
        }
        public List<EmployeeHistory> GetEmployeeHistory(int userId)
        {
            var archivedVacationDays = _employeeRepository.GetArchivedVacations(userId);
            var employeeHistoryList = new List<EmployeeHistory>();
            foreach (var vacationDay in archivedVacationDays)
            {
                employeeHistoryList.Add(ConvertVacationToEmployeeHistory(vacationDay));
            }
            return employeeHistoryList;
        }
        public EmployeeHistory ConvertVacationToEmployeeHistory(Vacation vacation)
        {
            var employeeHistory = new EmployeeHistory();
            employeeHistory.VacationFrom = vacation.VacationFrom;
            employeeHistory.VacationTo = vacation.VacationTo;
            employeeHistory.FirstName = vacation.User.FirstName;
            employeeHistory.LastName = vacation.User.LastName;
            employeeHistory.VacationSpent = vacation.VacationSpent;
            employeeHistory.VacationRequestDate = vacation.VacationRequestDate;
            employeeHistory.ApprovedBy = vacation.ApprovedBy;
            return employeeHistory;
        }

        public int CalculateTotalVacationForGivenPeriod(DateTime vacationFrom, DateTime vacationTo)
        {
            var dayNow = DateTime.Now;
            var holidays = _employeeRepository.GetHolidays();
            int totalVacationDaysSpent = 0;
            var oneDay = TimeSpan.FromDays(1);

            for (DateTime currentDay = vacationFrom; currentDay <= vacationTo; currentDay += oneDay)
            {
                if (isCurrentDayWeekend(currentDay.Date))
                    continue;
                int numberOfDaysThatAreNotOnHolidays = 0;
                foreach (var holiday in holidays)
                {
                    if (!holiday.HolidayDate.Equals(currentDay.Date))
                        numberOfDaysThatAreNotOnHolidays++;
                }
                if (numberOfDaysThatAreNotOnHolidays == holidays.Count)
                    totalVacationDaysSpent++;
            }
            return totalVacationDaysSpent;
        }

        private bool isCurrentDayWeekend(DateTime currentDay)
        {
            DayOfWeek day = currentDay.DayOfWeek;
            if (day.Equals(DayOfWeek.Sunday) || day.Equals(DayOfWeek.Saturday))
                return true;
            return false;
        }

        public void InsertVacation(Vacation vacation)
        {
            var user = _employeeRepository.GetUserById(vacation.UserId);
            vacation.VacationSpent = CalculateTotalVacationForGivenPeriod(vacation.VacationFrom, vacation.VacationTo);
            if (vacation.VacationSpent <= user.RemainingDaysOffLastYear + user.RemainingDaysOffCurrentYear)
            {
                CalculateRemainingVacation(vacation);
                _employeeRepository.AddVacation(vacation);
            }
        }
        private void CalculateRemainingVacation(Vacation vacation)
        {
            int vacationDaysSpent = CalculateTotalVacationForGivenPeriod(vacation.VacationFrom, vacation.VacationTo);
            var employee = _employeeRepository.GetUserById(vacation.UserId);
            int? remainingVacationLastYear = employee.RemainingDaysOffLastYear;
            int? remainingVacationCurrentYear = employee.RemainingDaysOffCurrentYear;
            if (remainingVacationLastYear > vacationDaysSpent)
            {
                employee.RemainingDaysOffLastYear = remainingVacationLastYear - vacationDaysSpent;
                UpdateEmployeeRemainingVacation(employee);
            }
            else if (remainingVacationLastYear <= vacationDaysSpent && remainingVacationCurrentYear + remainingVacationLastYear >= vacationDaysSpent)
            {
                employee.RemainingDaysOffLastYear = 0;
                employee.RemainingDaysOffCurrentYear = remainingVacationCurrentYear + remainingVacationLastYear - vacationDaysSpent;
                UpdateEmployeeRemainingVacation(employee);
            }

        }
        private void UpdateEmployeeRemainingVacation(User user)
        {
            var employeeForUpdate = _employeeRepository.GetUserById(user.Id);
            employeeForUpdate.RemainingDaysOffLastYear = user.RemainingDaysOffLastYear;
            employeeForUpdate.RemainingDaysOffCurrentYear = user.RemainingDaysOffCurrentYear;

            _employeeRepository.SaveChanges();
        }

        public void DeleteVacationRequestAndRestoreRemainingVacation(int vacationId)
        {
            var vacationForDelete = GetVacationByVacationId(vacationId);
            if (vacationForDelete != null)
            {
                int vacationDaysToRestore = CalculateTotalVacationForGivenPeriod(vacationForDelete.VacationFrom, vacationForDelete.VacationTo);
                RestoreRemainingVacation(vacationForDelete.UserId, vacationDaysToRestore);
                _employeeRepository.RemoveVacation(vacationForDelete);
            }
        }
        private void RestoreRemainingVacation(int userId, int vacationDaysToRestore)
        {
            var employee = _employeeRepository.GetUserById(userId);
            int? remainingVacationLastYear = employee.RemainingDaysOffLastYear;
            int? remainingVacationCurrentYear = employee.RemainingDaysOffCurrentYear;
            if (remainingVacationCurrentYear + vacationDaysToRestore <= TOTAL_GIVEN_VACATION_PER_YEAR)
            {
                employee.RemainingDaysOffCurrentYear = remainingVacationCurrentYear + vacationDaysToRestore;
                UpdateEmployeeRemainingVacation(employee);
            }
            else
            {
                employee.RemainingDaysOffCurrentYear = TOTAL_GIVEN_VACATION_PER_YEAR;
                employee.RemainingDaysOffLastYear = vacationDaysToRestore - (TOTAL_GIVEN_VACATION_PER_YEAR - remainingVacationCurrentYear);
                UpdateEmployeeRemainingVacation(employee);
            }
        }
        public void SetRemainingVacationOnFirstDayOfYear()
        {
            var currentYear = DateTime.Now.Year;
            var users = _employeeRepository.GetUsers();
            foreach (var user in users)
            {
                user.CurrentYear = currentYear;
                user.RemainingDaysOffLastYear = user.RemainingDaysOffCurrentYear;
                user.RemainingDaysOffCurrentYear = TOTAL_GIVEN_VACATION_PER_YEAR;
            }
            _employeeRepository.SaveChanges();
        }
    }
}
