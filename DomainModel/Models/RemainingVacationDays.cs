using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class RemainingVacationDays
    {
        public int Id { get; set; }
        public int CurrentYear { get; set; }
        public int RemainingDaysOffCurrentYear { get; set; }
        public int RemainingDaysOffLastYear { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
