using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Models
{
    public class Holiday
    {
        public int Id { get; set; }
        public string HolidayName { get; set; }
        public DateTime HolidayDate { get; set; }
    }
}
