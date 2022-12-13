using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.DtoModels
{
    public class EmployeeDetails
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int RemainingDaysOffLastYear { get; set; }
        public int RemainingDaysOffCurrentYear { get; set; }
    }
}
