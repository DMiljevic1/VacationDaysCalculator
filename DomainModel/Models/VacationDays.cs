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
        public int CurrentYear { get; set; }
        public int LastYear { get; set; }
        public int RemainingDaysOffCurrentYear { get; set; }
        public int RemainingDaysOffLastYear { get; set; }
        public int SpentDaysOffCurrentYear { get; set; }
        public int SpentDaysOffLastYear { get; set; }
    }
}
