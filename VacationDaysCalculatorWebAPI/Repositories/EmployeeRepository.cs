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
        public EmployeeDetails GetEmployeeDetails(int userId)
        {
            var employee = _vacationDbContext.Users.FirstOrDefault(u => u.Id == userId);
            var employeeDetails = new EmployeeDetails();
            employeeDetails.FirstName = employee.FirstName;
            employeeDetails.LastName = employee.LastName;
            employeeDetails.Email = employee.Email;
            employeeDetails.RemainingDaysOffLastYear = employee.RemainingDaysOffLastYear;
            employeeDetails.RemainingDaysOffCurrentYear = employee.RemainingDaysOffCurrentYear;
            employeeDetails.VacationDays = GetEmployeeVacation(userId);
            return employeeDetails;
        }
        public List<Vacation> GetEmployeeVacation(int userId)
        {
            var employeeVacation = _vacationDbContext.Vacation.Include(vd => vd.User).Where(vd => vd.UserId.Equals(userId) && (vd.Status.Equals(VacationStatus.Approved) || vd.Status.Equals(VacationStatus.OnVacation) || vd.Status.Equals(VacationStatus.Pending))).ToList();
            return employeeVacation;
        }

        public void SetVacationStatus(DateTime currentDate)
        {
            var vacations = _vacationDbContext.Vacation.ToList();
            var yesterday = currentDate.AddDays(-1);
            foreach (var vacation in vacations)
            {
                if (vacation.VacationFrom.Equals(currentDate) && vacation.Year == currentDate.Year && vacation.Status == VacationStatus.Approved)
                {
                    vacation.Status = VacationStatus.OnVacation;
                    UpdateEmployeeVacationStatus(vacation);
                }
                else if (vacation.VacationTo.Equals(yesterday.Date) && vacation.Year == currentDate.Year && vacation.Status == VacationStatus.OnVacation)
                {
                    vacation.Status = VacationStatus.Arhived;
                    UpdateEmployeeVacationStatus(vacation);
                }
                else if (vacation.VacationFrom.Equals(currentDate) && vacation.Year == currentDate.Year && vacation.Status == VacationStatus.Pending)
                {
                    vacation.Status = VacationStatus.Cancelled;
                    UpdateEmployeeVacationStatus(vacation);
                }
            }
        }
        public void UpdateEmployeeVacationStatus(Vacation vacation)
        {
            var vacationForUpdate = GetVacationDaysByVacationId(vacation.Id);
            if (vacationForUpdate != null)
            {
                vacationForUpdate.Status = vacation.Status;
                if (vacationForUpdate.Status == VacationStatus.Cancelled)
                {
                    int vacationDaysToRestore = CalculateTotalVacationForGivenPeriod(vacation.VacationFrom, vacation.VacationTo);
                    RestoreRemainingVacation(vacation.UserId, vacationDaysToRestore);
                }

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
        public EmployeeHistory ConvertVacationDaysToEmployeeHistory(Vacation vacation)
        {
            var employeeHistory = new EmployeeHistory();
            employeeHistory.VacationFrom = vacation.VacationFrom;
            employeeHistory.VacationTo = vacation.VacationTo;
            employeeHistory.Year = vacation.Year;
            employeeHistory.FirstName = vacation.User.FirstName;
            employeeHistory.LastName = vacation.User.LastName;
            employeeHistory.TotalVacationSpent = vacation.VacationSpent;
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
            vacation.VacationSpent = CalculateTotalVacationForGivenPeriod(vacation.VacationFrom, vacation.VacationTo);
            _vacationDbContext.Vacation.Add(vacation);
            _vacationDbContext.SaveChanges();
        }
        private void CalculateRemainingVacation(Vacation vacation)
        {
            int vacationDaysSpent = CalculateTotalVacationForGivenPeriod(vacation.VacationFrom, vacation.VacationTo);
            var employee = _vacationDbContext.Users.FirstOrDefault(rv => rv.Id.Equals(vacation.UserId));
            int remainingVacationLastYear = employee.RemainingDaysOffLastYear;
            int remainingVacationCurrentYear = employee.RemainingDaysOffCurrentYear;
            if (remainingVacationLastYear > vacationDaysSpent)
            {
                employee.RemainingDaysOffLastYear = remainingVacationLastYear - vacationDaysSpent;
                UpdateEmployeeRemainingVacation(employee);
            }
            else if(remainingVacationLastYear <= vacationDaysSpent && (remainingVacationCurrentYear + remainingVacationLastYear) >= vacationDaysSpent)
            {
                employee.RemainingDaysOffLastYear = 0;
                employee.RemainingDaysOffCurrentYear = remainingVacationCurrentYear + remainingVacationLastYear - vacationDaysSpent;
                UpdateEmployeeRemainingVacation(employee);
            }

        }
        private void UpdateEmployeeRemainingVacation(User employee)
        {
            var employeeForUpdate = _vacationDbContext.Users.FirstOrDefault(rm => rm.Id == employee.Id);
            employeeForUpdate.RemainingDaysOffLastYear = employee.RemainingDaysOffLastYear;
            employeeForUpdate.RemainingDaysOffCurrentYear = employee.RemainingDaysOffCurrentYear;

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
            var employee = _vacationDbContext.Users.FirstOrDefault(rv => rv.Id.Equals(userId));
            int remainingVacationLastYear = employee.RemainingDaysOffLastYear;
            int remainingVacationCurrentYear = employee.RemainingDaysOffCurrentYear;
            if(remainingVacationCurrentYear + vacationDaysToRestore <= TOTAL_YEARLY_VACATION_DAYS)
            {
                employee.RemainingDaysOffCurrentYear = remainingVacationCurrentYear + vacationDaysToRestore;
                UpdateEmployeeRemainingVacation(employee);
            }
            else
            {
                employee.RemainingDaysOffCurrentYear = TOTAL_YEARLY_VACATION_DAYS;
                employee.RemainingDaysOffLastYear = vacationDaysToRestore - (TOTAL_YEARLY_VACATION_DAYS - remainingVacationCurrentYear);
                UpdateEmployeeRemainingVacation(employee);
            }
        }
    }
}
