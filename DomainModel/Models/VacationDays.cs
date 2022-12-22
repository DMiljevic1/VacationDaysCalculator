using DomainModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class VacationDays
    {
        public int Id { get; set; }
        public DateTime VacationFrom { get; set; }
        public DateTime VacationTo { get; set; }
        public int Year { get; set; }
        public VacationStatus Status { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

    }
}
