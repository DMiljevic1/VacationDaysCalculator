using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class AddVacationDays
    {
        public int Id { get; set; }
        public DateTime VacationFrom { get; set; }
        public DateTime VacationTo { get; set; }
        public int Year { get; set; }
        public VacationStatus Status { get; set; }

    }

    public enum VacationStatus
    {
        Pending, Cancelled, Approved, OnVacation, Arhived
    }
}
