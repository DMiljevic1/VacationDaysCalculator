using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.DtoModels
{
	public class HolidayDetails
	{
		public DateTime Date { get; set; }
		public string LocalName { get; set; }
		public string Name { get; set; }
		public string CountryCode { get; set; }
		public bool Fixed { get; set; }
		public bool Global { get; set; }
		public string? Countries { get; set; }
		public string? LaunchYear { get; set; }
		public string[] Types { get; set; }
	}
}
