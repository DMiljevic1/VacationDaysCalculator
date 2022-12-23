using DomainModel.DtoModels;
using DomainModel.Enums;
using DomainModel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using VacationDaysCalculatorWebAPI.DatabaseContext;

namespace VacationDaysCalculatorWebAPI.Repositories
{
    public class EmployeeRepository
    {
        private readonly int TOTAL_YEARLY_VACATION_DAYS = 20;
        private readonly VacationDbContext _vacationDbContext;

        public EmployeeRepository(VacationDbContext vacationDbContext)
        {
            _vacationDbContext = vacationDbContext;
        }

        public List<User> GetUsers()
        {
            return _vacationDbContext.Users.ToList();
        }

        public void InsertUser(User user)
        {
            _vacationDbContext.Users.Add(user);
            _vacationDbContext.SaveChanges();
        }

        public EmployeeDetails GetEmployeeDetails(int userId)
        {
            var employeeVacationDays = _vacationDbContext.RemainingVacation.FirstOrDefault(vd => vd.UserId == userId);
            var employee = _vacationDbContext.Users.FirstOrDefault(u => u.Id == userId);
            var employeeDetails = new EmployeeDetails();
            employeeDetails.FirstName = employee.FirstName;
            employeeDetails.LastName = employee.LastName;
            employeeDetails.Email = employee.Email;
            employeeDetails.RemainingDaysOffLastYear = employeeVacationDays.RemainingDaysOffLastYear;
            employeeDetails.RemainingDaysOffCurrentYear = employeeVacationDays.RemainingDaysOffCurrentYear;
            employeeDetails.VacationDays = GetEmployeeVacation(userId);
            return employeeDetails;
        }
        public List<Vacation> GetEmployeeVacation(int userId)
        {
            var employeeVacation = _vacationDbContext.Vacation.Include(vd => vd.User).Where(vd => vd.UserId.Equals(userId) && (vd.Status.Equals(VacationStatus.Approved) || vd.Status.Equals(VacationStatus.OnVacation) || vd.Status.Equals(VacationStatus.Pending))).ToList();
            return employeeVacation;
        }

        private void setVacationStatus(List<Vacation> vacationDays)
        {
            var currentDate = DateTime.Today;
            var yesterday = DateTime.Today.AddDays(-1);
            foreach (var vacation in vacationDays)
            {
                if (vacation.VacationFrom.Equals(currentDate) && vacation.Year == currentDate.Year && vacation.Status == VacationStatus.Approved)
                {
                    vacation.Status = VacationStatus.OnVacation;
                    UpdateVacationDayStatus(vacation);
                }
                else if (vacation.VacationTo.Equals(yesterday.Date) && vacation.Year == currentDate.Year && vacation.Status == VacationStatus.OnVacation)
                {
                    vacation.Status = VacationStatus.Arhived;
                    UpdateVacationDayStatus(vacation);
                }
                else if (vacation.VacationFrom.Equals(currentDate) && vacation.Year == currentDate.Year && vacation.Status == VacationStatus.Pending)
                {
                    vacation.Status = VacationStatus.Cancelled;
                    UpdateVacationDayStatus(vacation);
                }
            }
        }
        public void UpdateVacationDayStatus(Vacation vacationDays)
        {
            var vacationForUpdate = GetVacationDaysByVacationId(vacationDays.Id);
            if (vacationForUpdate != null)
            {
                vacationForUpdate.Status = vacationDays.Status;

                _vacationDbContext.SaveChanges();
            }
        }
        public Vacation GetVacationDaysByVacationId(int vacationId)
        {
            return _vacationDbContext.Vacation.FirstOrDefault(vd => vd.Id == vacationId);
        }
        public List<EmployeeHistory> GetEmployeeHistory(int userId)
        {
            var archivedVacationDays = _vacationDbContext.Vacation.Include(vd => vd.User).Where(vd => vd.UserId.Equals(userId) && vd.Status.Equals(VacationStatus.Arhived)).ToList();
            var employeeHistoryList = new List<EmployeeHistory>();
            foreach(var vacationDay in archivedVacationDays)
            {
                employeeHistoryList.Add(ConvertVacationDaysToEmployeeHistory(vacationDay));
            }
            return employeeHistoryList;
        }
        public EmployeeHistory ConvertVacationDaysToEmployeeHistory(Vacation vacationDay)
        {
            var employeeHistory = new EmployeeHistory();
            employeeHistory.VacationFrom = vacationDay.VacationFrom;
            employeeHistory.VacationTo = vacationDay.VacationTo;
            employeeHistory.Year = vacationDay.Year;
            employeeHistory.FirstName = vacationDay.User.FirstName;
            employeeHistory.LastName = vacationDay.User.LastName;
            employeeHistory.TotalVacationSpent = CalculateTotalVacationForGivenPeriod(employeeHistory.VacationFrom, employeeHistory.VacationTo);
            return employeeHistory;
        }

        private int CalculateTotalVacationForGivenPeriod(DateTime vacationFrom, DateTime vacationTo)
        {
            var dayNow = DateTime.Now;
            var holidays = _vacationDbContext.Holidays.ToList();
            int totalVacationDaysSpent = 0;
            var oneDay = TimeSpan.FromDays(1);

            for (DateTime currentDay = vacationFrom; currentDay <= vacationTo; currentDay += oneDay)
            {
                if (isCurrentDayWeekend(currentDay.Date))
                    continue;
                int numberOfDaysThatAreNotOnHolidays = 0;
                foreach(var holiday in holidays)
                {
                    if (!holiday.HolidayDate.Equals(currentDay.Date))
                        numberOfDaysThatAreNotOnHolidays++;
                }
                if(numberOfDaysThatAreNotOnHolidays == holidays.Count)
                    totalVacationDaysSpent++;
            }
            return totalVacationDaysSpent;
        }

        private bool isCurrentDayWeekend(DateTime currentDay)
        {
            DayOfWeek day = currentDay.DayOfWeek;
            if(day.Equals(DayOfWeek.Sunday) || day.Equals(DayOfWeek.Saturday))
                return true;
            return false;
        }

        public void InsertVacation(Vacation vacation)
        {
            CalculateRemainingVacation(vacation);
            _vacationDbContext.Vacation.Add(vacation);
            _vacationDbContext.SaveChanges();
        }
        private void CalculateRemainingVacation(Vacation vacation)
        {
            int currentYear = DateTime.Now.Year;
            int vacationDaysSpent = CalculateTotalVacationForGivenPeriod(vacation.VacationFrom, vacation.VacationTo);
            var employeeRemainingVacation = _vacationDbContext.RemainingVacation.FirstOrDefault(rv => rv.UserId.Equals(vacation.UserId) && rv.CurrentYear.Equals(currentYear));
            int remainingVacationLastYear = employeeRemainingVacation.RemainingDaysOffLastYear;
            int remainingVacationCurrentYear = employeeRemainingVacation.RemainingDaysOffCurrentYear;
            if (remainingVacationLastYear > vacationDaysSpent)
            {
                employeeRemainingVacation.RemainingDaysOffLastYear = remainingVacationLastYear - vacationDaysSpent;
                UpdateEmployeeRemainingVacation(employeeRemainingVacation);
            }
            else if(remainingVacationLastYear <= vacationDaysSpent && (remainingVacationCurrentYear + remainingVacationLastYear) >= vacationDaysSpent)
            {
                employeeRemainingVacation.RemainingDaysOffLastYear = 0;
                employeeRemainingVacation.RemainingDaysOffCurrentYear = remainingVacationCurrentYear + remainingVacationLastYear - vacationDaysSpent;
                UpdateEmployeeRemainingVacation(employeeRemainingVacation);
            }

        }
        private void UpdateEmployeeRemainingVacation(RemainingVacation remainingVacation)
        {
            var remainingVacationForUpdate = _vacationDbContext.RemainingVacation.FirstOrDefault(rm => rm.Id == remainingVacation.Id);
            remainingVacationForUpdate.RemainingDaysOffLastYear = remainingVacation.RemainingDaysOffLastYear;
            remainingVacationForUpdate.RemainingDaysOffCurrentYear = remainingVacation.RemainingDaysOffCurrentYear;

            _vacationDbContext.SaveChanges();
        }

        public void DeleteVacationRequestAndRestoreRemainingVacation(int vacationId)
        {
            var vacationForDelete = GetVacationDaysByVacationId(vacationId);
            if(vacationForDelete != null)
            {
                int vacationDaysToRestore = CalculateTotalVacationForGivenPeriod(vacationForDelete.VacationFrom, vacationForDelete.VacationTo);
                RestoreRemainingVacation(vacationForDelete.UserId, vacationDaysToRestore);
                _vacationDbContext.Vacation.Remove(vacationForDelete);
                _vacationDbContext.SaveChanges();
            }
        }
        private void RestoreRemainingVacation(int userId, int vacationDaysToRestore)
        {
            var currentYear = DateTime.Now.Year;
            var employeeRemainingVacation = _vacationDbContext.RemainingVacation.FirstOrDefault(rv => rv.UserId.Equals(userId) && rv.CurrentYear.Equals(currentYear));
            int remainingVacationLastYear = employeeRemainingVacation.RemainingDaysOffLastYear;
            int remainingVacationCurrentYear = employeeRemainingVacation.RemainingDaysOffCurrentYear;
            if(remainingVacationCurrentYear + vacationDaysToRestore <= TOTAL_YEARLY_VACATION_DAYS)
            {
                employeeRemainingVacation.RemainingDaysOffCurrentYear = remainingVacationCurrentYear + vacationDaysToRestore;
                UpdateEmployeeRemainingVacation(employeeRemainingVacation);
            }
            else
            {
                employeeRemainingVacation.RemainingDaysOffCurrentYear = TOTAL_YEARLY_VACATION_DAYS;
                employeeRemainingVacation.RemainingDaysOffLastYear = vacationDaysToRestore - (TOTAL_YEARLY_VACATION_DAYS - remainingVacationCurrentYear);
                UpdateEmployeeRemainingVacation(employeeRemainingVacation);
            }
        }
    }
}
